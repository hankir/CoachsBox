using System.Collections.Generic;
using System.Linq;
using CoachsBox.Core;

namespace CoachsBox.Coaching.Accounting.GroupAccountModel
{
  public class GroupAccountSpecification : BaseSpecification<GroupAccount>
  {
    public GroupAccountSpecification(int groupId)
      : base(groupAccount => groupAccount.GroupId == groupId)
    {
    }

    public GroupAccountSpecification(IEnumerable<int> groupIds)
      : base(groupAccount => groupIds.Contains(groupAccount.GroupId))
    {
    }

    public GroupAccountSpecification WithEntries()
    {
      this.AddInclude(groupAccount => groupAccount.Entries);
      return this;
    }
  }
}
