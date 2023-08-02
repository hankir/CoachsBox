using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoachsBox.Coaching.CoachModel;
using CoachsBox.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace CoachsBox.Coaching.Infrastructure
{
  public class CoachRepository : EfRepository<Coach, CoachsBoxContext>, ICoachRepository
  {
    public override async Task<Coach> GetByIdAsync(int id)
    {
      var coach = await base.GetByIdAsync(id);
      if (coach == null)
        return coach;

      await this.context.Entry(coach).Reference(c => c.Person).LoadAsync();
      return coach;
    }

    public override async Task<IReadOnlyList<Coach>> ListAllAsync()
    {
      return await this.context.Coaches.Include(c => c.Person).ToListAsync();
    }

    public async Task<Coach> GetByPersonIdAsync(int personId)
    {
      var coach = await this.context.Coaches.Where(coach => coach.PersonId == personId).SingleOrDefaultAsync();
      if (coach == null)
        return coach;

      await this.context.Entry(coach).Reference(c => c.Person).LoadAsync();
      return coach;
    }

    public CoachRepository(CoachsBoxContext dbContext)
      : base(dbContext)
    {
    }
  }
}
