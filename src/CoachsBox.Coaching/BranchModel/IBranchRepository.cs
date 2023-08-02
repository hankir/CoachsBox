using System.Collections.Generic;
using System.Threading.Tasks;
using CoachsBox.Coaching.PersonModel;
using CoachsBox.Core.Interfaces;

namespace CoachsBox.Coaching.BranchModel
{
  public interface IBranchRepository : IAsyncRepository<Branch>
  {
    Task<IReadOnlyList<Person>> ListPersonForContact();
  }
}
