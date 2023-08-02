using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoachsBox.Coaching.BranchModel;
using CoachsBox.Coaching.PersonModel;
using CoachsBox.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace CoachsBox.Coaching.Infrastructure
{
  public class BranchRepository : EfRepository<Branch, CoachsBoxContext>, IBranchRepository
  {
    public override async Task<IReadOnlyList<Branch>> ListAllAsync()
    {
      return await this.context.Branches.Include(b => b.ContactPerson).ToListAsync();
    }

    public override async Task<Branch> GetByIdAsync(int id)
    {
      return await context.Branches
        .Include(branch => branch.ContactPerson)
        .Include("CoachingStaff.Coach")
        .SingleOrDefaultAsync(b => b.Id == id);
    }

    public async Task<IReadOnlyList<Person>> ListPersonForContact()
    {
      return await this.context.Persons
        .Where(person => context.Coaches.Any(coach => coach.PersonId == person.Id))
        .ToListAsync();
    }

    public BranchRepository(CoachsBoxContext dbContext)
      : base(dbContext)
    {
    }
  }
}
