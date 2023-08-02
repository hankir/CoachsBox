using System.Linq;
using CoachsBox.Coaching.ScheduleModel;
using CoachsBox.Infrastructure;

namespace CoachsBox.Coaching.Infrastructure
{
  public class ScheduleRepository : EfRepository<Schedule, CoachsBoxContext>, IScheduleRepository
  {
    public ScheduleRepository(CoachsBoxContext dbContext)
      : base(dbContext)
    {
    }
  }
}
