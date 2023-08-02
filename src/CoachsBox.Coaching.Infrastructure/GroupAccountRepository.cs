using CoachsBox.Coaching.Accounting.GroupAccountModel;
using CoachsBox.Infrastructure;

namespace CoachsBox.Coaching.Infrastructure
{
  public class GroupAccountRepository : EfRepository<GroupAccount, CoachsBoxContext>, IGroupAccountRepository
  {
    public GroupAccountRepository(CoachsBoxContext dbContext)
      : base(dbContext)
    {
    }
  }
}
