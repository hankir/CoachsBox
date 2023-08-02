using CoachsBox.Coaching.Accounting.SalaryAccountingEventModel;
using CoachsBox.Infrastructure;

namespace CoachsBox.Coaching.Infrastructure
{
  public class SalaryAccountingEventRepository : EfRepository<SalaryAccountingEvent, CoachsBoxContext>, ISalaryAccountingEventRepository
  {
    public SalaryAccountingEventRepository(CoachsBoxContext dbContext)
      : base(dbContext)
    {
    }
  }
}
