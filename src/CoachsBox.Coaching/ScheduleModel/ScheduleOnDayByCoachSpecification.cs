using System.Linq;
using CoachsBox.Core;
using CoachsBox.Core.Primitives;

namespace CoachsBox.Coaching.ScheduleModel
{
  public class ScheduleOnDayByCoachSpecification : BaseSpecification<Schedule>
  {
    public ScheduleOnDayByCoachSpecification(int coachId, Date day)
      : base(schedule =>
      schedule.TrainingList.Any(t =>
        t.Date.Day == day.Day &&
        t.Date.Month.Number == day.Month.Number &&
        t.Date.Year == day.Year) &&
      schedule.CoachId == coachId)
    {
      this.AddInclude(schedule => schedule.Group);
    }
  }
}
