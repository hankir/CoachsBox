using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CoachsBox.Coaching.Accounting.CoachingServiceAgreementModel;
using CoachsBox.Coaching.Accounting.StudentAccountingEventModel;
using CoachsBox.Coaching.Accounting.StudentAccountModel;
using CoachsBox.Coaching.ScheduleModel;
using CoachsBox.Core;
using CoachsBox.Core.Primitives;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CoachsBox.Coaching.Application.Commands
{
  public class AccrueFeeMonthlyTrainingCommandHandler : IRequestHandler<AccrueFeeMonthlyTrainingCommand, bool>
  {
    private readonly ILogger<AccrueFeeMonthlyTrainingCommandHandler> logger;
    private readonly IStudentAccountingEventRepository eventRepository;
    private readonly ICoachingServiceAgreementRepository agreementRepository;
    private readonly IStudentAccountRepository studentAccountRepository;
    private readonly IScheduleRepository scheduleRepository;

    public AccrueFeeMonthlyTrainingCommandHandler(
      ILogger<AccrueFeeMonthlyTrainingCommandHandler> logger,
      IStudentAccountingEventRepository eventRepository,
      ICoachingServiceAgreementRepository agreementRepository,
      IStudentAccountRepository studentAccountRepository,
      IScheduleRepository scheduleRepository)
    {
      this.logger = logger;
      this.eventRepository = eventRepository;
      this.agreementRepository = agreementRepository;
      this.studentAccountRepository = studentAccountRepository;
      this.scheduleRepository = scheduleRepository;
    }

    public async Task<bool> Handle(AccrueFeeMonthlyTrainingCommand request, CancellationToken cancellationToken)
    {
      if (request == null)
        return false;

      var groupId = request.GroupId;
      var studentId = request.StudentId;
      var agreementId = request.AgreementId;
      var month = request.Month;
      var year = request.Year;

      var agreement = await this.agreementRepository.GetByIdAsync(agreementId);
      if (agreement == null)
        return false;

      var studentAccountSpecification = new StudentAccountSpecification(studentId);
      var account = await this.studentAccountRepository.GetBySpecAsync(studentAccountSpecification);
      if (account == null)
      {
        logger.LogWarning("Skip accrual [GroupId:{GroupId};AgreementId:{AgreementId};Period:{Month}/{Year};StudentId:{StudentId}]. Account not found.",
          groupId, agreementId, month, year, studentId);
        return false;
      }

      var monthlyAccrualSpecification = new AccrualForMonthSpecification(studentId, month, year);
      if (await this.eventRepository.CountAsync(monthlyAccrualSpecification) > 0)
      {
        logger.LogInformation("Skip accrual [GroupId:{GroupId};AgreementId:{AgreementId};Period:{Month}/{Year};StudentId:{StudentId}]. Accrual already exist.",
          groupId, agreementId, month, year, studentId);
        return false;
      }

      var groupScheduleSpecification = new ScheduleByGroupSpecification(groupId);
      var groupSchedule = await this.scheduleRepository.GetBySpecAsync(groupScheduleSpecification.AsReadOnly());
      if (groupSchedule == null)
      {
        logger.LogInformation("Skip accrual [GroupId:{GroupId};AgreementId:{AgreementId};Period:{Month}/{Year}]. Schedule not found.",
          groupId, agreement.Id, month, year);
        return false;
      }

      // Расчет принимается из учета идеального месяца, где кол-во дней 30 и кол-во тренировок всегда одинакого.
      var trainingsPerWeek = groupSchedule.TrainingList.Count(training => training.IsRegular());
      var trainingsPerMonth = (byte)(trainingsPerWeek * 4);

      var accruralEvent = new MonthlyAccrualAccountingEvent(
        trainingsPerMonth,
        Month.Create(month),
        year,
        Watch.Now.DateTime,
        Watch.Now.DateTime,
        groupId,
        account,
        agreement);

      accruralEvent.Process();

      logger.LogInformation("Process accrual event. Account entries will be created. [GroupId:{GroupId};AgreementId:{AgreementId};Period:{Month}/{Year};StudentId:{StudentId};TrainingsCount:{TrainingsCount}].",
        groupId, agreementId, month, year, studentId, trainingsPerMonth);

      await this.eventRepository.AddAsync(accruralEvent);
      await this.eventRepository.SaveAsync();

      logger.LogInformation("Added accrual [GroupId:{GroupId};AgreementId:{AgreementId};Period:{Month}/{Year};StudentId:{StudentId};TrainingsCount:{TrainingsCount}].",
        groupId, agreementId, month, year, studentId, trainingsPerMonth);

      return true;
    }
  }
}
