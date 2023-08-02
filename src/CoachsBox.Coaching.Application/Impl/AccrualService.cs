using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using CoachsBox.Coaching.Accounting.AgreementRegistryEntryModel;
using CoachsBox.Coaching.Accounting.CoachingServiceAgreementModel;
using CoachsBox.Coaching.Accounting.StudentAccountingEventModel;
using CoachsBox.Coaching.Accounting.StudentAccountModel;
using CoachsBox.Coaching.AttendanceLogModel;
using CoachsBox.Coaching.GroupModel;
using CoachsBox.Coaching.ScheduleModel;
using CoachsBox.Core;
using CoachsBox.Core.Interfaces;
using CoachsBox.Core.Primitives;
using Microsoft.Extensions.Logging;

namespace CoachsBox.Coaching.Application.Impl
{
  public class AccrualService : IAccrualService
  {
    private readonly ILogger<AccrualService> logger;
    private readonly IUnitOfWork unitOfWork;
    private readonly IStudentAccountingEventRepository eventRepository;
    private readonly IAgreementRegistryEntryRepository agreementRegistry;
    private readonly IAttendanceLogRepository attendanceLogRepository;
    private readonly IScheduleRepository scheduleRepository;
    private readonly IStudentAccountRepository studentAccountRepository;

    public AccrualService(
      ILogger<AccrualService> logger,
      IUnitOfWork unitOfWork,
      IStudentAccountingEventRepository eventRepository,
      IAgreementRegistryEntryRepository agreementRegistry,
      IAttendanceLogRepository attendanceLogRepository,
      IScheduleRepository scheduleRepository,
      IStudentAccountRepository studentAccountRepository)
    {
      this.logger = logger;
      this.unitOfWork = unitOfWork;
      this.eventRepository = eventRepository;
      this.agreementRegistry = agreementRegistry;
      this.attendanceLogRepository = attendanceLogRepository;
      this.scheduleRepository = scheduleRepository;
      this.studentAccountRepository = studentAccountRepository;
    }

    public void CalculateAccrualForDate(Date date)
    {
      logger.LogInformation("Calculating accruals starting");
      var time = Stopwatch.StartNew();
      var agreementList = this.agreementRegistry.ListAllAsync().Result;
      this.CalculateAccrualForDate(date, agreementList);
      time.Stop();
      logger.LogInformation("Calculate accruals finished at {Elapsed}", time.Elapsed);
    }

    public void CalculateAccrualForPeriod(Date from, Date to)
    {
      logger.LogInformation("Calculating accruals starting");
      var agreementList = this.agreementRegistry.ListAllAsync().Result;

      var time = Stopwatch.StartNew();
      var toDateTime = new DateTime(to.Year, to.Month.Number, to.Day);
      var fromDateTime = new DateTime(from.Year, from.Month.Number, from.Day);

      var agreementsWithPersonalTrainingAccrual = agreementList.Where(a => a.Agreement.AccrualEventType.Equals(StudentAccountingEventType.PersonalTrainingAccrual)).ToList();
      var days = toDateTime.Subtract(fromDateTime).TotalDays;
      for (int i = 0; i < days; i++)
      {
        var date = Date.Create(fromDateTime.AddDays(i));
        this.CalculateAccrualForDate(date, agreementsWithPersonalTrainingAccrual);
      }

      time.Stop();
      logger.LogInformation("Calculate personal trainings accruals finished at {Elapsed}", time.Elapsed);

      time.Restart();
      var agreementsWithMonthlyAccrual = agreementList.Where(a => a.Agreement.AccrualEventType.Equals(StudentAccountingEventType.Accrual)).ToList();
      var currentDate = new DateTime(from.Year, from.Month.Number, 1);
      while (currentDate <= toDateTime)
      {
        var date = Date.Create(currentDate);
        this.CalculateAccrualForDate(date, agreementsWithMonthlyAccrual);
        currentDate = currentDate.AddMonths(1);
      }
      time.Stop();
      logger.LogInformation("Calculate monthly accruals finished at {Elapsed}", time.Elapsed);
    }

    public void ProcessAccruals()
    {
      logger.LogInformation("Processing accruals starting");
      var time = Stopwatch.StartNew();
      var previoslyUnprocessedAccrualsSpec = new PreviouslyUnprocessedAccrualsSpecification(1500);
      var unprocessedAccruals = this.eventRepository.ListAsync(previoslyUnprocessedAccrualsSpec).Result;

      foreach (var accrual in unprocessedAccruals)
        accrual.Process();

      if (unprocessedAccruals.Any())
        this.unitOfWork.Save();

      time.Stop();
      logger.LogInformation("Processing accruals finished at {Elapsed}", time.Elapsed);
    }

    /// <summary>
    /// Проверить является ли отметка посещаемости подотчетная для начислений.
    /// </summary>
    /// <param name="isTrialTraining">Является ли тренировка пробной.</param>
    /// <param name="absence">Отсутсвие.</param>
    /// <returns>True если отметка посещаемости является подотчетной, иначе - false.</returns>
    public bool IsAttendanceLogEntryAccountable(bool isTrialTraining, Absence absence)
    {
      // Посещение подотчетное, кроме пробной тренировки.
      if (absence == null)
        return !isTrialTraining;

      // Отсутствие без уважительной причины тоже подотчетное.
      return Absence.WithoutValidExcuse.Equals(absence);
    }

    private void CalculateAccrualForDate(Date date, IReadOnlyList<AgreementRegistryEntry> agreementList)
    {
      foreach (var agreement in agreementList)
      {
        var year = date.Year;
        var groupId = agreement.GroupId;
        var groupAttendanceSpecification = new GroupAttendanceSpecification(groupId, year).WithGroup().AsReadOnly();
        var groupAttendance = this.attendanceLogRepository.ListAsync(groupAttendanceSpecification).Result.FirstOrDefault();
        if (groupAttendance == null)
        {
          logger.LogInformation("Skip accrual [GroupId:{GroupId};AgreementId:{AgreementId};Year:{AttendanceYear}]. Attendance log not found.",
            groupId, agreement.Id, year);
          continue;
        }

        var group = groupAttendance.Group;
        if (group == null)
        {
          logger.LogError("Skip accrual [GroupId:{GroupId};AgreementId:{AgreementId}]. Group not found.", groupId, agreement.Id);
          continue;
        }

        if (agreement.Agreement.AccrualEventType.Equals(StudentAccountingEventType.Accrual))
        {
          var month = date.Month.Number;
          this.CalculateAccrualPerMonth(month, year, agreement.Agreement, group, groupAttendance);
        }
        else if (agreement.Agreement.AccrualEventType.Equals(StudentAccountingEventType.PersonalTrainingAccrual))
        {
          this.CalculateAccrualPerTraining(date, agreement.Agreement, group, groupAttendance);
        }
      }
      this.unitOfWork.Save();
    }

    private void CalculateAccrualPerMonth(int month, int year, CoachingServiceAgreement agreement, Group group, AttendanceLog groupAttendance)
    {
      var groupScheduleSpecification = new ScheduleByGroupSpecification(group.Id);
      var groupSchedule = this.scheduleRepository.GetBySpecAsync(groupScheduleSpecification.AsReadOnly()).Result;
      if (groupSchedule == null)
      {
        logger.LogInformation("Skip accrual [GroupId:{GroupId};AgreementId:{AgreementId};Period:{Month}/{Year}]. Schedule not found.",
          group.Id, agreement.Id, month, year);
        return;
      }

      // Расчет принимается из учета идеального месяца, где кол-во дней 30 и кол-во тренировок всегда одинакого.
      var trainingsPerWeek = groupSchedule.TrainingList.Count(training => training.IsRegular());
      var trainingsPerMonth = (byte)(trainingsPerWeek * 4);

      foreach (var enrolledStudent in group.EnrolledStudents)
      {
        var studentId = enrolledStudent.StudentId;
        var studentAccountSpecification = new StudentAccountSpecification(studentId).AsReadOnly();
        var account = this.studentAccountRepository.ListAsync(studentAccountSpecification).Result.FirstOrDefault();
        if (account == null)
        {
          logger.LogWarning("Skip accrual [GroupId:{GroupId};AgreementId:{AgreementId};Period:{Month}/{Year};StudentId:{StudentId}]. Account not found.",
            group.Id, agreement.Id, month, year, studentId);
          continue;
        }

        var monthlyAccrualSpecification = new AccrualForMonthSpecification(studentId, month, year).AsReadOnly();
        if (this.eventRepository.CountAsync(monthlyAccrualSpecification).Result > 0)
        {
          logger.LogTrace("Skip accrual [GroupId:{GroupId};AgreementId:{AgreementId};Period:{Month}/{Year};StudentId:{StudentId}]. Accrual already exist.",
            group.Id, agreement.Id, month, year, studentId);
          continue;
        }

        // Проверить, есть ли вообще отметка посещения в этом месяце.
        if (!groupAttendance.HaveAttendanceInMonth(month, studentId))
        {
          logger.LogTrace("Skip accrual [GroupId:{GroupId};AgreementId:{AgreementId};Period:{Month}/{Year};StudentId:{StudentId}]. Student have not attendance in month",
            group.Id, agreement.Id, month, year, studentId);
          continue;
        }

        var firstTrainingInMonth = groupAttendance.GetAttendanceInMonth(month, studentId)
          // Берем все подотчетные отметки в месяце.
          .Where(attendance => this.IsAttendanceLogEntryAccountable(attendance.IsTrialTraining, attendance.AbsenceReason))
          .OrderBy(attendance => attendance.WhenStarted())
          .FirstOrDefault();

        // Если в указанном месяце нет тренировок (пробные не учитываются), значит и начислять нечего.
        if (firstTrainingInMonth == null)
          continue;

        var monthYear = Date.Create(firstTrainingInMonth.WhenStarted());
        var accruralEvent = new MonthlyAccrualAccountingEvent(
          trainingsPerMonth, monthYear.Month, monthYear.Year, Watch.Now.DateTime, Watch.Now.DateTime, group.Id, account.Id, agreement.Id);
        this.eventRepository.AddAsync(accruralEvent).Wait();
        logger.LogInformation("Added accrual [GroupId:{GroupId};AgreementId:{AgreementId};Period:{Month}/{Year};StudentId:{StudentId};TrainingsCount:{TrainingsCount}]. First in month attendance absence:{Absence}",
          group.Id, agreement.Id, monthYear.Month.Number, monthYear.Year, studentId, trainingsPerMonth, firstTrainingInMonth.AbsenceReason?.Reason);
      }
    }

    private void CalculateAccrualPerTraining(Date date, CoachingServiceAgreement agreement, Group group, AttendanceLog groupAttendance)
    {
      var calculationDate = new DateTime(date.Year, date.Month.Number, date.Day);
      foreach (var enrolledStudent in group.EnrolledStudents)
      {
        var studentId = enrolledStudent.StudentId;
        var studentAccountSpecification = new StudentAccountSpecification(studentId).AsReadOnly();
        var account = this.studentAccountRepository.ListAsync(studentAccountSpecification).Result.FirstOrDefault();
        if (account == null)
        {
          logger.LogWarning("Skip accrual [GroupId:{GroupId};AgreementId:{AgreementId};Period:{Date};StudentId:{StudentId}]. Account not found.",
            group.Id, agreement.Id, calculationDate.ToShortDateString(), studentId);
          continue;
        }

        var personalTrainingAccrualByDateSpecification = new PersonalTrainingAccrualEventSpecification(studentId, date).AsReadOnly();
        var accrualsByDate = this.eventRepository.ListAsync(personalTrainingAccrualByDateSpecification).Result;
        var attendedTrainings = groupAttendance.GetAttendanceByDate(studentId, date).OrderBy(attendance => attendance.WhenStarted());

        if (!attendedTrainings.Any())
        {
          logger.LogTrace("Skip accrual [GroupId:{GroupId};AgreementId:{AgreementId};Period:{Date};StudentId:{StudentId}]. Student have not attendance for date",
            group.Id, agreement.Id, calculationDate.ToShortDateString(), studentId);
          continue;
        }

        foreach (var attendance in attendedTrainings)
        {
          // Если отметка в журнале не подотчетная, то пропускаем.
          if (!this.IsAttendanceLogEntryAccountable(attendance.IsTrialTraining, attendance.AbsenceReason))
            continue;

          var whenStarted = attendance.WhenStarted();
          var whenEnded = attendance.WhenEnded();

          var dateTime = whenStarted.Date;
          var start = whenStarted.TimeOfDay;
          var end = whenEnded.TimeOfDay;

          // Если начисление на дату для студента уже есть, то пропустим его.
          if (accrualsByDate.Any(a => a.StartOfTraining.Equals(attendance.Start) && a.EndOfTraining.Equals(attendance.End)))
          {
            logger.LogTrace("Skip accrual [GroupId:{GroupId};AgreementId:{AgreementId};Period:{Date} {Start}-{End};StudentId:{StudentId}]. Accrual already exist.",
              group.Id, agreement.Id, dateTime.ToShortDateString(), start, end, studentId);
            continue;
          }

          var trainingDate = Date.Create(dateTime);
          var trainingStart = TimeOfDay.Create(start);
          var trainingEnd = TimeOfDay.Create(end);

          var accruralEvent = new PersonalTrainingAccrualAccountingEvent(
            trainingDate, trainingStart, trainingEnd, whenStarted, Watch.Now.DateTime, group.Id, account.Id, agreement.Id);
          this.eventRepository.AddAsync(accruralEvent).Wait();
          logger.LogInformation("Added accrual [GroupId:{GroupId};AgreementId:{AgreementId};Period:{Date} {Start}-{End};StudentId:{StudentId}]. Attendance absence:{Absence}",
            group.Id, agreement.Id, dateTime.ToShortDateString(), start, end, studentId, attendance.AbsenceReason?.Reason);
        }
      }
    }

    public int AccountableTrainingCount(AttendanceStatisticsInfo attendanceStatistics)
    {
      var trainingCounts = 0;

      // Добавить начисления за посещения, если такая отметка подотчетная.
      if (attendanceStatistics.NumberAttended > 0 && this.IsAttendanceLogEntryAccountable(false, null))
      {
        trainingCounts += attendanceStatistics.NumberAttended;
      }

      // Добавить начисления за отметку отсутсвия без уважительной причины, если такая отметка подотчетная.
      if (attendanceStatistics.NumberAbsent > 0 && this.IsAttendanceLogEntryAccountable(false, Absence.WithoutValidExcuse))
      {
        trainingCounts += attendanceStatistics.NumberAbsent;
      }

      return trainingCounts;
    }
  }
}
