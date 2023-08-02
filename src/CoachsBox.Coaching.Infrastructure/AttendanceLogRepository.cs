using System.Collections.Generic;
using System.Linq;
using CoachsBox.Coaching.AttendanceLogModel;
using CoachsBox.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace CoachsBox.Coaching.Infrastructure
{
  public class AttendanceLogRepository : EfRepository<AttendanceLog, CoachsBoxContext>, IAttendanceLogRepository
  {
    public AttendanceLogRepository(CoachsBoxContext dbContext) : base(dbContext)
    {
    }

    public bool HasTrialTrainingMark(int groupId, int studentId)
    {
      return this.context
        .AttendanceLogs
        .Include(log => log.Entries)
        .Where(log => log.GroupId == groupId)
        .Any(log => log.Entries.Any(entry => entry.StudentId == studentId && entry.IsTrialTraining));
    }

    public IReadOnlyDictionary<int, bool> HasTrialTrainingMarks(int groupId)
    {
      var entries = this.context
        .AttendanceLogs
        .Include(log => log.Entries)
        .Where(log => log.GroupId == groupId)
        .SelectMany(log => log.Entries.Where(entry => entry.IsTrialTraining))
        .Select(entry => new { entry.StudentId, entry.IsTrialTraining })
        .ToList();

      var result = new Dictionary<int, bool>();
      foreach (var logEntry in entries)
      {
        result[logEntry.StudentId] = true;
      }

      return result;
    }
  }
}
