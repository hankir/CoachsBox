using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoachsBox.Coaching.CoachModel;
using CoachsBox.Coaching.GroupModel;
using CoachsBox.Coaching.PersonModel;
using Microsoft.Extensions.Caching.Memory;

namespace CoachsBox.WebApp.AppFacade.Internal
{
  public class DisplayNameServiceFacade : IDisplayNameServiceFacade
  {
    private readonly IMemoryCache memoryCache;
    private readonly ICoachQueryService coachQueryService;
    private readonly IGroupQueryService groupQueryService;

    public DisplayNameServiceFacade(
      IMemoryCache memoryCache,
      ICoachQueryService coachQueryService,
      IGroupQueryService groupQueryService)
    {
      this.memoryCache = memoryCache;
      this.coachQueryService = coachQueryService;
      this.groupQueryService = groupQueryService;
    }

    public async Task<PersonName> GetCoachNameAsync(int coachId)
    {
      var cacheKey = GetCacheKey("coach", coachId);
      PersonName name;
      if (!this.memoryCache.TryGetValue(cacheKey, out name))
      {
        name = await this.coachQueryService.GetName(coachId);
        this.memoryCache.Set(cacheKey, name, TimeSpan.FromHours(8));
      }
      return name;
    }

    public async Task<IReadOnlyDictionary<int, PersonName>> ListCoachsNamesAsync(int[] coachIds)
    {
      var cacheKey = GetCacheKey("coach", coachIds);
      IReadOnlyDictionary<int, PersonName> personNames;
      if (!this.memoryCache.TryGetValue(cacheKey, out personNames))
      {
        personNames = await this.coachQueryService.ListNames(coachIds);
        this.memoryCache.Set(cacheKey, personNames, TimeSpan.FromHours(8));
      }
      return personNames;
    }

    public async Task<string> GetGroupNameAsync(int groupId)
    {
      var cacheKey = GetCacheKey("coach", groupId);
      string name;
      if (!this.memoryCache.TryGetValue(cacheKey, out name))
      {
        name = await this.groupQueryService.GetName(groupId);
        this.memoryCache.Set(cacheKey, name, TimeSpan.FromHours(8));
      }
      return name;
    }

    public async Task<IReadOnlyDictionary<int, string>> ListGroupNamesAsync(int[] groupIds)
    {
      var cacheKey = GetCacheKey("group", groupIds);
      IReadOnlyDictionary<int, string> groupNames;
      if (!this.memoryCache.TryGetValue(cacheKey, out groupNames))
      {
        groupNames = await this.groupQueryService.ListNames(groupIds);
        this.memoryCache.Set(cacheKey, groupNames, TimeSpan.FromHours(8));
      }
      return groupNames;
    }

    private static string GetCacheKey(string entityName, int entityId)
    {
      return GetCacheKey(entityName, new[] { entityId });
    }

    private static string GetCacheKey(string entityName, int[] entityIds)
    {
      var entityIdsIdentity = string.Join(",", entityIds.OrderBy(id => id));
      return $"{nameof(DisplayNameServiceFacade)}:{entityName}:id:[{entityIdsIdentity}]";
    }
  }
}
