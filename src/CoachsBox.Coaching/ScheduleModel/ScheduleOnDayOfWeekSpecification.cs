using System;
using System.Linq;
using CoachsBox.Core;

namespace CoachsBox.Coaching.ScheduleModel
{
  public class ScheduleOnDayOfWeekSpecification : BaseSpecification<Schedule>
  {
    public ScheduleOnDayOfWeekSpecification(int coachId, DayOfWeek dayOfWeek)
      : base(schedule =>
      schedule.CoachId == coachId &&
      schedule.TrainingList.Any(t => t.DayOfWeek == dayOfWeek))
    {
      this.AddInclude(schedule => schedule.Group);
    }

    public ScheduleOnDayOfWeekSpecification(DayOfWeek dayOfWeek)
      : base(schedule => schedule.TrainingList.Any(t => t.DayOfWeek == dayOfWeek))
    {
      this.AddInclude(schedule => schedule.Group);
    }

    public ScheduleOnDayOfWeekSpecification WithCoach()
    {
      this.AddInclude(schedule => schedule.Coach.Person);
      return this;
    }
  }
}
