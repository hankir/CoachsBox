using CoachsBox.Coaching.Accounting.SalaryModel;
using CoachsBox.Infrastructure;

namespace CoachsBox.Coaching.Infrastructure
{
  public class SalaryRepository : EfRepository<Salary, CoachsBoxContext>, ISalaryRepository
  {
    public SalaryRepository(CoachsBoxContext dbContext) : base(dbContext)
    {
    }
  }
}
