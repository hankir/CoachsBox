using System;
using System.Collections.Generic;
using System.Linq;
using CoachsBox.Coaching.Application;
using CoachsBox.Coaching.Application.Data;
using CoachsBox.Coaching.AttendanceLogModel;
using CoachsBox.Coaching.GroupModel;
using CoachsBox.Coaching.ScheduleModel;
using CoachsBox.Core;
using CoachsBox.WebApp.Pages.Facade.Internal.Assembler;

namespace CoachsBox.WebApp.AppFacade.Groups.DTO
{
  public class GroupDetailsDTOAssembler
  {
    public GroupDetailsDTO ToDTO(
      Group group,
      Schedule schedule,
      GroupBalanceReports groupBalanceReports,
      IReadOnlyDictionary<TrainingTimeInfo, IReadOnlyList<AttendanceLogEntry>> attendanceReportCard,
      IReadOnlyDictionary<int, bool> hasTrialTrainings)
    {
      if (group == null)
        throw new ArgumentNullException(nameof(group));

      if (groupBalanceReports == null)
        throw new ArgumentNullException(nameof(groupBalanceReports));

      var result = new GroupDetailsDTO()
      {
        GroupId = group.Id,
        GroupName = group.Name,
        CoachId = schedule?.CoachId,
        CoachFullName = schedule?.Coach?.Person.Name.FullName()
      };

      if (schedule != null)
      {
        result.ScheduleId = schedule.Id;
        var assembler = new TrainingTimeDTOAssembler();
        var trainingsDTO = assembler.ToDTOList(schedule.TrainingList);
        result.Schedule = trainingsDTO;
      }

      foreach (var enrolledStudent in group.EnrolledStudents)
      {
        var studentPerson = enrolledStudent.Student.Person;
        var studentDTO = new GroupDetailsStudentDTO()
        {
          StudentId = enrolledStudent.StudentId,
          StudentFullName = studentPerson.Name.FullName(),
          StudentShortName = ShortStudentName(studentPerson.Name),
          Phone = studentPerson.PersonalInformation?.PhoneNumber?.Value,
          Email = studentPerson.PersonalInformation?.Email?.Value,
          GroupBalance = groupBalanceReports.CalculateBalanceByStudentId(enrolledStudent.StudentId).Quantity,
          TrialTrainingCount = enrolledStudent.TrialTrainingCount,
          CanEnableTrialTraining = !hasTrialTrainings.ContainsKey(enrolledStudent.StudentId)
        };

        if (attendanceReportCard != null)
        {
          foreach (var attendance in attendanceReportCard.Values.SelectMany(e => e.Where(s => s.StudentId == enrolledStudent.StudentId)))
          {
            studentDTO.AttendanceLog.Add(new GroupDetailsStudentAttendanceDTO()
            {
              Date = attendance.Date.ToDateTime().Value,
              Start = attendance.Start.ToTimeSpan(),
              End = attendance.End.ToTimeSpan(),
              AbsenceReason = attendance.AbsenceReason?.Reason,
              IsTrialTraining = attendance.IsTrialTraining
            });
          }
        }

        result.Students.Add(studentDTO);
      }

      if (attendanceReportCard != null)
        result.AttendanceReportCardTitle.AddRange(attendanceReportCard.Keys);

      return result;
    }

    private static string ShortStudentName(Coaching.PersonModel.PersonName personName)
    {
      var result = personName.Surname;
      if (personName.Name != null)
        result += " " + personName.Name;
      return result;
    }
  }
}
