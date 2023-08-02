using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoachsBox.Coaching.AttendanceLogModel;
using Microsoft.EntityFrameworkCore;

namespace CoachsBox.Coaching.Infrastructure.Coaching
{
  public class AttendanceQueryService : IAttendanceQueryService
  {
    private readonly ReadOnlyCoachsBoxContext readOnlyCoachsBox;

    public AttendanceQueryService(ReadOnlyCoachsBoxContext readOnlyCoachsBox)
    {
      this.readOnlyCoachsBox = readOnlyCoachsBox;
    }

    public async Task<IReadOnlyDictionary<int, AttendanceStatisticsInfo>> GetAttendanceMarksByCoachAsync(int coachId, DateTime from, DateTime to)
    {
      var attendanceLogEntries = await this.readOnlyCoachsBox
        .AttendanceLogs
        .SelectMany(log => log.Entries)
        .Where(entry => entry.CoachId == coachId && entry.Date.Year == from.Year)
        .Where(entry => entry.Date.Month.Number >= from.Month && entry.Date.Month.Number <= to.Month)
        .Select(entry => new { GroupId = entry.AttendanceLog.GroupId, Entry = entry })
        .ToListAsync();

      var allGroupMarks = attendanceLogEntries.GroupBy(entry => entry.GroupId);
      var result = new Dictionary<int, AttendanceStatisticsInfo>(allGroupMarks.Count());
      foreach (var groupMarks in allGroupMarks)
      {
        var markEntries = groupMarks.Select(mark => mark.Entry).ToList();
        var numberAttendedTotal = markEntries.Where(entry => entry.AbsenceReason == null).Count();
        var numberAttendedTrial = markEntries.Where(entry => entry.IsTrialTraining).Count();
        var statistic = new AttendanceStatisticsInfo()
        {
          NumberAttendedTotal = numberAttendedTotal,
          NumberAttendedTrial = numberAttendedTrial,
          NumberAttended = numberAttendedTotal - numberAttendedTrial,
          NumberSickened = markEntries.Where(entry => entry.AbsenceReason != null && entry.AbsenceReason.Equals(Absence.Sickness)).Count(),
          NumberAbsent = markEntries.Where(entry => entry.AbsenceReason != null && entry.AbsenceReason.Equals(Absence.WithoutValidExcuse)).Count()
        };
        result.Add(groupMarks.Key, statistic);
      }

      return result;
    }

    public async Task<IReadOnlyDictionary<int, IReadOnlyDictionary<int, AttendanceStatisticsInfo>>> GetStudentAttendanceMarksByCoachAndGroupAsync(int coachId, int groupId, DateTime from, DateTime to)
    {
      var attendanceLogEntries = await this.readOnlyCoachsBox
        .AttendanceLogs
        .SelectMany(log => log.Entries)
        .Where(entry => entry.CoachId == coachId && entry.AttendanceLog.GroupId == groupId && entry.Date.Year == from.Year)
        .Where(entry => entry.Date.Month.Number >= from.Month && entry.Date.Month.Number <= to.Month)
        .Select(entry => new { GroupId = entry.AttendanceLog.GroupId, Entry = entry })
        .ToListAsync();

      var allGroupMarks = attendanceLogEntries.GroupBy(entry => entry.GroupId);
      var result = new Dictionary<int, IReadOnlyDictionary<int, AttendanceStatisticsInfo>>(allGroupMarks.Count());

      foreach (var groupMarks in allGroupMarks)
      {
        var allGroupStudentsMarks = groupMarks.GroupBy(m => m.Entry.StudentId);
        var studentsStatistics = new Dictionary<int, AttendanceStatisticsInfo>(allGroupStudentsMarks.Count());
        foreach (var studentMarks in allGroupStudentsMarks)
        {
          var markEntries = studentMarks.Select(mark => mark.Entry).ToList();
          var numberAttendedTotal = markEntries.Where(entry => entry.AbsenceReason == null).Count();
          var numberAttendedTrial = markEntries.Where(entry => entry.IsTrialTraining).Count();
          var statistic = new AttendanceStatisticsInfo()
          {
            NumberAttendedTotal = numberAttendedTotal,
            NumberAttendedTrial = numberAttendedTrial,
            NumberAttended = numberAttendedTotal - numberAttendedTrial,
            NumberSickened = markEntries.Where(entry => entry.AbsenceReason != null && entry.AbsenceReason.Equals(Absence.Sickness)).Count(),
            NumberAbsent = markEntries.Where(entry => entry.AbsenceReason != null && entry.AbsenceReason.Equals(Absence.WithoutValidExcuse)).Count()
          };
          studentsStatistics.Add(studentMarks.Key, statistic);
        }
        result.Add(groupMarks.Key, studentsStatistics);
      }

      return result;
    }
  }
}
