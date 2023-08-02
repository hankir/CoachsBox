using CoachsBox.Coaching.Accounting.StudentAccountPostingRuleModel;
using CoachsBox.Infrastructure;

namespace CoachsBox.Coaching.Infrastructure
{
  public class StudentAccountPostingRuleRepository : EfRepository<StudentAccountPostingRule, CoachsBoxContext>, IStudentAccountPostingRuleRepository
  {
    public StudentAccountPostingRuleRepository(CoachsBoxContext dbContext)
      : base(dbContext)
    {
    }
  }
}
