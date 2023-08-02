using CoachsBox.Coaching.GroupModel;
using CoachsBox.Infrastructure;

namespace CoachsBox.Coaching.Infrastructure
{
  public class GroupRepository : EfRepository<Group, CoachsBoxContext>, IGroupRepository
  {
    public GroupRepository(CoachsBoxContext dbContext)
        : base(dbContext)
    {
    }
  }
}
