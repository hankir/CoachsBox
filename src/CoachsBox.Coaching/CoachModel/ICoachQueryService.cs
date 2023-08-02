using System.Collections.Generic;
using System.Threading.Tasks;
using CoachsBox.Coaching.PersonModel;

namespace CoachsBox.Coaching.CoachModel
{
  public interface ICoachQueryService
  {
    Task<PersonName> GetName(int coachId);

    Task<IReadOnlyDictionary<int, PersonName>> ListNames(int[] coachIds);

    Task<IReadOnlyList<int>> ListAllCoachIdsAsync();
  }
}
