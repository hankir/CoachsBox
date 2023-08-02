using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CoachsBox.Core;

namespace CoachsBox.Coaching.ScheduleModel
{
  public class ScheduleByGroupSpecification : BaseSpecification<Schedule>
  {
    public ScheduleByGroupSpecification(int groupId)
      : base(schedule => schedule.GroupId == groupId)
    {
    }

    public ScheduleByGroupSpecification WithCoach()
    {
      this.AddInclude(schedule => schedule.Coach);
      this.AddInclude(schedule => schedule.Coach.Person);
      return this;
    }

    public ScheduleByGroupSpecification(IEnumerable<int> groupIds)
      : base(schedule => groupIds.Contains(schedule.GroupId))
    {
    }
  }
}
