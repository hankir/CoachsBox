using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CoachsBox.Coaching.AttendanceLogModel
{
  public interface IAttendanceQueryService
  {
    Task<IReadOnlyDictionary<int, AttendanceStatisticsInfo>> GetAttendanceMarksByCoachAsync(int coachId, DateTime from, DateTime to);

    Task<IReadOnlyDictionary<int, IReadOnlyDictionary<int, AttendanceStatisticsInfo>>> GetStudentAttendanceMarksByCoachAndGroupAsync(int coachId, int groupId, DateTime from, DateTime to);
  }
}
