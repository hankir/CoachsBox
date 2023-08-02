using System.Collections.Generic;
using System.Linq;
using CoachsBox.Core;

namespace CoachsBox.Coaching.AttendanceLogModel
{
  public class GroupAttendanceSpecification : BaseSpecification<AttendanceLog>
  {

    public GroupAttendanceSpecification(int groupId)
      : base(attendanceLog => attendanceLog.GroupId == groupId)
    {
    }

    public GroupAttendanceSpecification(int groupId, int year)
      : base(attendanceLog => attendanceLog.GroupId == groupId && attendanceLog.Year == year)
    {
    }

    public GroupAttendanceSpecification(IEnumerable<int> groupIds)
      : base(attendanceLog => groupIds.Contains(attendanceLog.GroupId))
    {
    }

    public GroupAttendanceSpecification WithGroup()
    {
      this.Includes.Add(attendanceLog => attendanceLog.Group);
      return this;
    }
  }
}
