using System.Collections.Generic;
using System.Threading.Tasks;
using CoachsBox.Coaching.PersonModel;

namespace CoachsBox.WebApp.AppFacade
{
  public interface IDisplayNameServiceFacade
  {
    Task<PersonName> GetCoachNameAsync(int coachId);

    Task<IReadOnlyDictionary<int, PersonName>> ListCoachsNamesAsync(int[] coachIds);

    Task<string> GetGroupNameAsync(int groupId);

    Task<IReadOnlyDictionary<int, string>> ListGroupNamesAsync(int[] groupIds);
  }
}
