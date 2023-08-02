using CoachsBox.Coaching.Accounting.GroupAccountEntryModel;
using CoachsBox.Infrastructure;

namespace CoachsBox.Coaching.Infrastructure
{
  public class GroupAccountEntryRepository : EfRepository<GroupAccountEntry, CoachsBoxContext>, IGroupAccountEntryRepository
  {
    public GroupAccountEntryRepository(CoachsBoxContext dbContext)
      : base(dbContext)
    {
    }
  }
}
