using System.Collections.Generic;
using CoachsBox.Core.Interfaces;

namespace CoachsBox.Coaching.AttendanceLogModel
{
  public interface IAttendanceLogRepository : IAsyncRepository<AttendanceLog>
  {
    public bool HasTrialTrainingMark(int groupId, int studentId);

    public IReadOnlyDictionary<int, bool> HasTrialTrainingMarks(int groupId);
  }
}
