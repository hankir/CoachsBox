using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoachsBox.Coaching.Accounting.StudentAccountEntryModel;
using CoachsBox.Coaching.Accounting.StudentAccountingEventModel;
using CoachsBox.Core.Primitives;
using CoachsBox.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace CoachsBox.Coaching.Infrastructure
{
  public class StudentAccountingEventRepository : EfRepository<StudentAccountingEvent, CoachsBoxContext>, IStudentAccountingEventRepository
  {
    public override async Task<StudentAccountingEvent> GetByIdAsync(int id)
    {
      return await this.context.Set<StudentAccountingEvent>()
        .Include(e => e.Account)
        .Include(e => e.ServiceAgreement)
        .SingleOrDefaultAsync(e => e.Id == id);
    }

    public async Task<IReadOnlyList<IGrouping<int, StudentAccountingEvent>>> ListGroupsEvents(DateTime from, DateTime to)
    {
      return (await this.context.Set<StudentAccountingEvent>()
        .AsNoTracking()
        .Include(accountingEvent => accountingEvent.Account)
        .Include(accountingEvent => accountingEvent.ServiceAgreement)
        .ThenInclude(agreement => agreement.PostingRules)
        .ThenInclude(postingRules => postingRules.PostingRule)
        .Include(accountingEvent => accountingEvent.ProcessingState.ResultingEntries)
        .ThenInclude(resultingEntities => resultingEntities.AccountEntry)
        .Where(accountingEvent => accountingEvent.WhenOccured >= from && accountingEvent.WhenOccured <= to)
        .ToListAsync())
        .GroupBy(accountingEvent => accountingEvent.GroupId)
        .ToList();
    }

    public async Task<IReadOnlyList<StudentAccountingEvent>> ListEventsByGroupId(int groupId)
    {
      return await this.context.Set<StudentAccountingEvent>()
        .AsNoTracking()
        .Include(accountingEvent => accountingEvent.Account)
        .Include(accountingEvent => accountingEvent.ServiceAgreement)
        .ThenInclude(agreement => agreement.PostingRules)
        .ThenInclude(postingRules => postingRules.PostingRule)
        .Include(accountingEvent => accountingEvent.ProcessingState.ResultingEntries)
        .ThenInclude(resultingEntities => resultingEntities.AccountEntry)
        .Where(accountingEvent => accountingEvent.GroupId == groupId)
        .ToListAsync();
    }

    public async Task<IReadOnlyDictionary<int, Money>> GetGroupsDebtsOn(DateTime to)
    {
      var groupBalances = await this.context.StudentAccountingEvents
        .AsNoTracking()
        .Where(e => e.WhenOccured <= to)
        .Join(
          this.context.StudentAccountingEvents,
          accountingEvent => accountingEvent.Id,
          accountingEvent => accountingEvent.Id,
          (leftAccountingEvent, rightAccountingEvent) => new
          {
            leftAccountingEvent.GroupId,
            leftAccountingEvent.AccountId,
            Amount = leftAccountingEvent
              .ProcessingState
              .ResultingEntries
              .Select(r => r.AccountEntry)
              .OfType<StudentAccountEntry>()
              .Sum(r => r.Amount.Quantity)
          })
        .GroupBy(g => new { g.GroupId, g.AccountId })
        .Select(g => new { GroupId = g.Key.GroupId, AccountId = g.Key.AccountId, Balance = g.Sum(gv => gv.Amount) })
        .Where(g => g.Balance < 0)
        .GroupBy(g => g.GroupId)
        .Select(g => new { GroupId = g.Key, Balance = g.Sum(gv => gv.Balance) })
        .ToListAsync();

      return groupBalances.ToDictionary(gb => gb.GroupId, gb => Money.CreateRuble(gb.Balance));
    }

    public StudentAccountingEventRepository(CoachsBoxContext dbContext)
      : base(dbContext)
    {
    }
  }
}
