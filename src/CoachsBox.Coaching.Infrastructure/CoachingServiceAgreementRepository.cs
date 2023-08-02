using System.Threading.Tasks;
using CoachsBox.Coaching.Accounting.CoachingServiceAgreementModel;
using CoachsBox.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace CoachsBox.Coaching.Infrastructure
{
  public class CoachingServiceAgreementRepository : EfRepository<CoachingServiceAgreement, CoachsBoxContext>, ICoachingServiceAgreementRepository
  {
    public override async Task<CoachingServiceAgreement> GetByIdAsync(int id)
    {
      return await this.context.Set<CoachingServiceAgreement>()
        .Include(a => a.PostingRules)
        .ThenInclude(p => p.PostingRule)
        .SingleOrDefaultAsync(a => a.Id == id);
    }

    public CoachingServiceAgreementRepository(CoachsBoxContext dbContext)
      : base(dbContext)
    {
    }
  }
}
