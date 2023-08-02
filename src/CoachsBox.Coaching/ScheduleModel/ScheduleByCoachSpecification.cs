using CoachsBox.Core;

namespace CoachsBox.Coaching.ScheduleModel
{
  public class ScheduleByCoachSpecification : BaseSpecification<Schedule>
  {
    public ScheduleByCoachSpecification(int coachId)
      : base(schedule => schedule.CoachId == coachId)
    {
    }
  }
}
