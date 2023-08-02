using System.Collections.Generic;
using System.Threading.Tasks;
using CoachsBox.Coaching.Accounting.AgreementRegistryEntryModel;
using CoachsBox.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace CoachsBox.Coaching.Infrastructure
{
  public class AgreementRegistryEntryRepository : EfRepository<AgreementRegistryEntry, CoachsBoxContext>, IAgreementRegistryEntryRepository
  {
    public override async Task<AgreementRegistryEntry> GetByIdAsync(int id)
    {
      return await this.context
        .AgreementRegistry
        .Include(entry => entry.Agreement)
        .SingleOrDefaultAsync(entry => entry.Id == id);
    }

    public override async Task<IReadOnlyList<AgreementRegistryEntry>> ListAllAsync()
    {
      return await this.context.Set<AgreementRegistryEntry>().Include(e => e.Agreement).ToListAsync();
    }

    public AgreementRegistryEntryRepository(CoachsBoxContext dbContext)
      : base(dbContext)
    {
    }
  }
}
