using System;
using System.Collections.Generic;
using System.Linq;
using CoachsBox.Coaching.AttendanceLogModel;

namespace CoachsBox.Coaching.Application
{
  public class AttendanceReports
  {
    private readonly IEnumerable<AttendanceLog> attendanceLogs;

    public AttendanceReports(IEnumerable<AttendanceLog> attendanceLogs)
    {
      this.attendanceLogs = attendanceLogs;
    }

    public AttendanceStatisticsInfo AttendanceStatisticByGroup(int groupId, DateTime from, DateTime to)
    {
      var group = this.attendanceLogs.FirstOrDefault(attendanceLog => attendanceLog.GroupId == groupId)?.Group;
      if (group == null)
        return new AttendanceStatisticsInfo();

      var attendanceLogEntries = this.attendanceLogs
        .Where(attendanceLog => attendanceLog.GroupId == groupId)
        .SelectMany(attendanceLog => attendanceLog.GetAttendanceByDate(from, to))
        .GroupBy(entry => entry.StudentId);

      var numberAttended = attendanceLogEntries.Where(studentEntries => studentEntries.Any(entry => entry.AbsenceReason == null)).Count();
      var numberAttendedTrial = attendanceLogEntries.Where(studentEntries => studentEntries.Any(entry => entry.IsTrialTraining)).Count();
      return new AttendanceStatisticsInfo()
      {
        NumberAttendedTotal = numberAttended,
        NumberAttendedTrial = numberAttendedTrial,
        NumberAttended = numberAttended - numberAttendedTrial,
        NumberSickened = attendanceLogEntries.Where(studentEntries => studentEntries.Any(entry => entry.AbsenceReason != null && entry.AbsenceReason.Equals(Absence.Sickness))).Count(),
        NumberAbsent = attendanceLogEntries.Where(studentEntries => studentEntries.All(entry => entry.AbsenceReason != null && entry.AbsenceReason.Equals(Absence.WithoutValidExcuse))).Count()
      };
    }
  }
}
