using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoachsBox.Coaching.Accounting.StudentAccountingEventModel;
using CoachsBox.Coaching.Application;
using CoachsBox.Coaching.Application.Data;
using CoachsBox.Coaching.AttendanceLogModel;
using CoachsBox.Coaching.GroupModel;
using CoachsBox.Coaching.ScheduleModel;
using CoachsBox.Coaching.StudentModel;
using CoachsBox.Core;
using CoachsBox.Core.Primitives;
using CoachsBox.WebApp.AppFacade.Attendance.DTO;
using CoachsBox.WebApp.Pages.Facade.DTO;

namespace CoachsBox.WebApp.Pages.Facade.Internal
{
  public class AttendanceLogServiceFacade : IAttendanceLogServiceFacade
  {
    private readonly IGroupRepository groupRepository;
    private readonly IStudentRepository studentRepository;
    private readonly IStudentAccountingEventRepository coachingAccountingEventRepository;
    private readonly IAttendanceLogRepository attendanceLogRepository;
    private readonly IScheduleRepository scheduleRepository;

    public AttendanceLogServiceFacade(
      IGroupRepository groupRepository,
      IStudentRepository studentRepository,
      IStudentAccountingEventRepository coachingAccountingEventRepository,
      IAttendanceLogRepository attendanceLogRepository,
      IScheduleRepository scheduleRepository)
    {
      this.groupRepository = groupRepository;
      this.studentRepository = studentRepository;
      this.coachingAccountingEventRepository = coachingAccountingEventRepository;
      this.attendanceLogRepository = attendanceLogRepository;
      this.scheduleRepository = scheduleRepository;
    }

    public async Task MarkAllStudents(int groupId, MarkAttendanceCommand markCommand)
    {
      if (markCommand == null)
        return;

      var group = await this.groupRepository.GetByIdAsync(groupId);
      if (group.EnrolledStudents.Count > 0)
      {
        var attendanceLog = await this.attendanceLogRepository.GetByIdAsync(markCommand.AttendanceId);
        var coachId = await this.GetCoachId(groupId, markCommand);
        foreach (var student in group.EnrolledStudents)
        {
          var date = Date.Create(markCommand.Date);
          var start = TimeOfDay.Create(markCommand.Start);
          var end = TimeOfDay.Create(markCommand.End);

          var studentId = student.StudentId;
          var existsAttendance = attendanceLog.MarkExists(studentId, date, start, end);
          if (existsAttendance != null)
            continue;

          var absence = Absence.GetAll<Absence>().Where(a => a.Reason == markCommand.AbsenceReason).SingleOrDefault();
          if (absence == null)
            attendanceLog.MarkStudent(studentId, coachId, date, start, end, student.HasTrialTrainings());
          else
            attendanceLog.MarkStudent(studentId, coachId, date, start, end, absence);
        }
      }

      await this.attendanceLogRepository.SaveAsync();
    }

    public async Task ClearAllMark(int groupId, MarkAttendanceCommand markCommand)
    {
      if (markCommand == null)
        return;

      var group = await this.groupRepository.GetByIdAsync(groupId);
      if (group.EnrolledStudents.Count > 0)
      {
        var attendanceLog = await this.attendanceLogRepository.GetByIdAsync(markCommand.AttendanceId);
        foreach (var student in group.EnrolledStudents)
        {
          var date = Date.Create(markCommand.Date);
          var start = TimeOfDay.Create(markCommand.Start);
          var end = TimeOfDay.Create(markCommand.End);

          var studentId = student.StudentId;
          var existsAttendance = attendanceLog.MarkExists(studentId, date, start, end);
          if (existsAttendance != null)
            attendanceLog.ClearMark(existsAttendance);
        }
      }

      await this.attendanceLogRepository.SaveAsync();
    }

    public async Task ClearMarkStudent(int studentId, MarkAttendanceCommand markCommand)
    {
      if (markCommand == null)
        return;

      var date = Date.Create(markCommand.Date);
      var start = TimeOfDay.Create(markCommand.Start);
      var end = TimeOfDay.Create(markCommand.End);

      var attendanceLog = await this.attendanceLogRepository.GetByIdAsync(markCommand.AttendanceId);
      var existsAttendance = attendanceLog.MarkExists(studentId, date, start, end);
      if (existsAttendance != null)
        attendanceLog.ClearMark(existsAttendance);

      await this.attendanceLogRepository.SaveAsync();
    }

    public async Task MarkStudent(int studentId, MarkAttendanceCommand markCommand)
    {
      if (markCommand == null)
        return;

      var date = Date.Create(markCommand.Date);
      var start = TimeOfDay.Create(markCommand.Start);
      var end = TimeOfDay.Create(markCommand.End);

      var attendanceLog = await this.attendanceLogRepository.GetByIdAsync(markCommand.AttendanceId);
      var existsAttendance = attendanceLog.MarkExists(studentId, date, start, end);
      if (existsAttendance != null)
        attendanceLog.ClearMark(existsAttendance);

      var coachId = await this.GetCoachId(attendanceLog.GroupId, markCommand);
      var absence = Absence.GetAll<Absence>().Where(a => a.Reason == markCommand.AbsenceReason).SingleOrDefault();
      if (absence == null)
        attendanceLog.MarkStudent(studentId, coachId, date, start, end, markCommand.IsTrialTraining);
      else
        attendanceLog.MarkStudent(studentId, coachId, date, start, end, absence);

      await this.attendanceLogRepository.SaveAsync();
    }

    public Task<AttendanceLogViewDTO> ViewAttendanceLog(int groupId, int month, int year)
    {
      return this.ViewAttendanceLog(groupId, month, year, false);
    }

    public async Task<AttendanceLogViewDTO> ViewAttendanceLog(int groupId, int month, int year, bool showMarkedStudents)
    {
      AttendanceLog attendanceLog = await this.GetGroupAttendanceLog(groupId, year);
      Group group = await this.GetGroup(attendanceLog);
      Schedule schedule = await this.GetSchedule(group);

      var fromDate = new DateTime(year, month, 1);
      var beforeDate = new DateTime(year, month, DateTime.DaysInMonth(year, month));
      IReadOnlyCollection<Student> students = showMarkedStudents ?
        await this.GetMarkedStudents(group, attendanceLog, fromDate, beforeDate) :
        await this.GetEnrolledStudent(group);

      var entries = new List<AttendanceLogEntryDTO>();
      var columns = new List<TrainingTimeInfo>();
      var rows = new List<AttendanceLogViewRowTitle>();
      if (schedule != null)
      {
        var attendanceReportCardBuilder = new AttendanceReportCardBuilder(month, schedule, attendanceLog);
        var monthAttendanceLog = attendanceReportCardBuilder.Build();
        columns = monthAttendanceLog.Keys.ToList();
        foreach (var student in students)
        {
          var isEnrolled = group.EnrolledStudents.Any(enrolled => enrolled.StudentId == student.Id);
          rows.Add(new AttendanceLogViewRowTitle()
          {
            StudentId = student.Id,
            StudentFullName = student.Person.Name.FullName(),
            StudentShortName = ShortStudentName(student.Person.Name),
            Excluded = !isEnrolled
          });

          var studentAttendanceLog = monthAttendanceLog.Values.SelectMany(e => e.Where(s => s.StudentId == student.Id));
          foreach (var attendance in studentAttendanceLog)
          {
            entries.Add(new AttendanceLogEntryDTO()
            {
              GroupId = groupId,
              StudentId = student.Id,
              StudentFullName = student.Person.Name.FullName(),
              Date = attendance.Date.ToDateTime().Value,
              Start = attendance.Start.ToTimeSpan(),
              End = attendance.End.ToTimeSpan(),
              AbsenceReason = attendance.AbsenceReason?.Reason,
              IsExists = true,
              IsTrial = attendance.IsTrialTraining,
              // GroupBalance = groupBalanceReports.CalculatePreliminaryBalanceByStudentId(student.Id).Quantity
            });
          }
        }
      }
      else
      {
        foreach (var student in students)
        {
          var isEnrolled = group.EnrolledStudents.Any(enrolled => enrolled.StudentId == student.Id);
          rows.Add(new AttendanceLogViewRowTitle()
          {
            StudentId = student.Id,
            StudentFullName = student.Person.Name.FullName(),
            StudentShortName = ShortStudentName(student.Person.Name),
            Excluded = !isEnrolled
          });
        }
      }

      return new AttendanceLogViewDTO()
      {
        ColumnsTitle = columns,
        RowsTitle = rows,
        AttendanceLog = new AttendanceLogDTO()
        {
          AttendanceId = attendanceLog.Id,
          GroupId = groupId,
          GroupName = group.Name,
          Entries = entries
        }
      };
    }

    private static string ShortStudentName(Coaching.PersonModel.PersonName personName)
    {
      var result = personName.Surname;
      if (personName.Name != null)
        result += " " + personName.Name;
      return result;
    }

    private async Task<Schedule> GetSchedule(Group group)
    {
      var branchId = group.BranchId;
      var groupId = group.Id;
      var scheduleSpecification = new ScheduleSpecification(groupId, branchId).WithCoach();
      return await this.scheduleRepository.GetBySpecAsync(scheduleSpecification.AsReadOnly());
    }

    public async Task<AttendanceLogDTO> ViewTrainingAttendance(int groupId, DateTime trainingDate, TimeSpan trainingStartTime, TimeSpan trainingEndTime, bool showMarkedStudents)
    {
      AttendanceLog attendanceLog = await this.GetGroupAttendanceLog(groupId, trainingDate.Year);
      Group group = await this.GetGroup(attendanceLog);

      var fromDate = new DateTime(trainingDate.Year, trainingDate.Month, 1);
      var beforeDate = new DateTime(trainingDate.Year, trainingDate.Month, DateTime.DaysInMonth(trainingDate.Year, trainingDate.Month));
      IReadOnlyCollection<Student> students = showMarkedStudents ?
        await this.GetMarkedStudents(group, attendanceLog, fromDate, beforeDate) :
        await this.GetEnrolledStudent(group);

      GroupBalanceReports groupBalanceReports = await this.GetGroupBalanceReports(groupId);

      var entryDate = Date.Create(trainingDate);
      var entryStart = TimeOfDay.Create(trainingStartTime);
      var entryEnd = TimeOfDay.Create(trainingEndTime);

      var entries = new List<AttendanceLogEntryDTO>();
      foreach (var student in students)
      {
        var enrolledStudent = group.EnrolledStudents.SingleOrDefault(enrolled => enrolled.StudentId == student.Id);
        var attendanceLogEntry = attendanceLog.MarkExists(student.Id, entryDate, entryStart, entryEnd);
        entries.Add(new AttendanceLogEntryDTO()
        {
          GroupId = groupId,
          StudentId = student.Id,
          CoachId = attendanceLogEntry?.CoachId,
          StudentFullName = student.Person.Name.FullName(),
          Date = trainingDate,
          Start = trainingStartTime,
          End = trainingEndTime,
          AbsenceReason = attendanceLogEntry?.AbsenceReason?.Reason,
          IsExists = attendanceLogEntry != null,
          IsTrial = attendanceLogEntry?.IsTrialTraining == true || enrolledStudent?.HasTrialTrainings() == true,
          GroupBalance = groupBalanceReports.CalculateBalanceByStudentId(student.Id).Quantity,
          Excluded = enrolledStudent == null
        });
      }

      return new AttendanceLogDTO()
      {
        AttendanceId = attendanceLog.Id,
        GroupId = groupId,
        GroupName = group.Name,
        Entries = entries
      };
    }

    private async Task<GroupBalanceReports> GetGroupBalanceReports(int groupId)
    {
      var groupAccountingEvents = await this.coachingAccountingEventRepository.ListEventsByGroupId(groupId);
      return new GroupBalanceReports(groupAccountingEvents);
    }

    private async Task<IReadOnlyCollection<Student>> GetMarkedStudents(Group group, AttendanceLog attendanceLog, DateTime fromDate, DateTime beforeDate)
    {
      var studentIds = attendanceLog.Entries
        .Where(entry => entry.WhenStarted() >= fromDate && entry.WhenEnded() <= beforeDate)
        .Select(entry => entry.StudentId)
        .Union(group.EnrolledStudents.Select(enrolled => enrolled.StudentId))
        .Distinct()
        .ToList();

      var studentsSpecification = new StudentByIdSpecification(studentIds).AsReadOnly();
      return await this.studentRepository.ListAsync(studentsSpecification);
    }

    private async Task<IReadOnlyCollection<Student>> GetEnrolledStudent(Group group)
    {
      var studentIds = group.EnrolledStudents.Select(enrolled => enrolled.StudentId);
      var enrolledStudentsSpecification = new StudentByIdSpecification(studentIds).AsReadOnly();
      return await this.studentRepository.ListAsync(enrolledStudentsSpecification);
    }

    /// <summary>
    /// Получить группу по журналу посещаемости.
    /// </summary>
    /// <param name="attendanceLog">Журнал посещаемости.</param>
    /// <returns>Группа.</returns>
    private async Task<Group> GetGroup(AttendanceLog attendanceLog)
    {
      return attendanceLog.Group ?? await this.groupRepository.GetByIdAsync(attendanceLog.GroupId);
    }

    /// <summary>
    /// Получить журнал посещаемости группы.
    /// </summary>
    /// <param name="groupId">Идентификатор группы.</param>
    /// <param name="year">Год.</param>
    /// <returns>Существующий журнал группы, или вновь созданный, если его до этого еще не было.</returns>
    /// <remarks>TODO: Это должно быть в домене.</remarks>
    private async Task<AttendanceLog> GetGroupAttendanceLog(int groupId, int year)
    {
      var attendanceLogSpecification = new GroupAttendanceSpecification(groupId, year).WithGroup();
      var attendanceLog = await this.attendanceLogRepository.GetBySpecAsync(attendanceLogSpecification.AsReadOnly());
      if (attendanceLog == null)
      {
        attendanceLog = new AttendanceLog(groupId, year);
        await this.attendanceLogRepository.AddAsync(attendanceLog);
        await this.attendanceLogRepository.SaveAsync();
      }
      return attendanceLog;
    }

    private async Task<int> GetCoachId(int groupId, MarkAttendanceCommand markCommand)
    {
      var coachId = markCommand.CoachId;
      if (coachId <= 0)
      {
        var scheduleSpec = new ScheduleByGroupSpecification(groupId).AsReadOnly();
        var schedule = await this.scheduleRepository.GetBySpecAsync(scheduleSpec);
        coachId = schedule.CoachId;
      }
      return coachId;
    }
  }
}
