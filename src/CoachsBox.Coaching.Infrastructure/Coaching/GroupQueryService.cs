using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoachsBox.Coaching.GroupModel;
using Microsoft.EntityFrameworkCore;

namespace CoachsBox.Coaching.Infrastructure.Coaching
{
  public class GroupQueryService : IGroupQueryService
  {
    private readonly ReadOnlyCoachsBoxContext context;

    public GroupQueryService(ReadOnlyCoachsBoxContext readOnlyCoachsBox)
    {
      this.context = readOnlyCoachsBox;
    }

    public Task<string> GetName(int groupId)
    {
      return this.context
        .Groups
        .Where(group => group.Id == groupId)
        .Select(group => group.Name)
        .SingleAsync();
    }

    public async Task<IReadOnlyDictionary<int, string>> ListNames(int[] groupIds)
    {
      var groupNames = await this.context
        .Groups
        .Where(group => groupIds.Contains(group.Id))
        .Select(group => new { GroupId = group.Id, Name = group.Name })
        .ToListAsync();

      var result = new Dictionary<int, string>();
      foreach (var groupName in groupNames)
        result[groupName.GroupId] = groupName.Name;

      return result;
    }
  }
}
