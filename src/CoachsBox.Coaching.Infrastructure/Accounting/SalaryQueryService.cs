using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoachsBox.Coaching.Accounting.SalaryAccountingEventModel;
using CoachsBox.Coaching.Accounting.SalaryModel;
using CoachsBox.Core.Primitives;
using Microsoft.EntityFrameworkCore;

namespace CoachsBox.Coaching.Infrastructure.Accounting
{
  public class SalaryQueryService : ISalaryQueryService
  {
    private readonly ReadOnlyCoachsBoxContext context;

    public SalaryQueryService(ReadOnlyCoachsBoxContext readOnlyContext)
    {
      this.context = readOnlyContext;
    }

    public async Task<IReadOnlyList<CoachSalaryCalculation>> GetCoachSalaryCalculations(int salaryId, int coachId)
    {
      var calculations = await this.context
        .Salaries
        .Include(salary => salary.Calculations)
        .Where(salary => salary.Id == salaryId)
        .SelectMany(salary => salary.Calculations)
        .OfType<CoachSalaryCalculation>()
        .Where(coachSalary => coachSalary.CoachId == coachId)
        .ToListAsync();

      return calculations;
    }

    public async Task<IReadOnlyList<SalaryDebtAccountingEvent>> GetCoachSalaryDebts()
    {
      return await this.context
        .SalaryAccountingEvents
        .OfType<SalaryDebtAccountingEvent>()
        .Where(debtEvent => !debtEvent.ProcessingState.IsProcessed)
        .ToListAsync();
    }

    public async Task<IReadOnlyList<int>> GetNotPaidSalaries()
    {
      return await this.context
        .Salaries
        .Where(salary => !salary.IsPaid)
        .Select(salary => salary.Id)
        .ToListAsync();
    }

    public async Task<SalaryFund> GetSalaryFundAsync(DateTime to)
    {
      var groupAccountEntries = await this.context
       .GroupAccountEntries
       .Include(groupAccountEntry => groupAccountEntry.GroupAccount)
       .Where(groupAccountEntry => groupAccountEntry.Date.Date <= to)
       .ToListAsync();

      var salaryFundSharesSet = groupAccountEntries
        .GroupBy(groupAccountEntry => groupAccountEntry.GroupAccount)
        .Select(groupAccountEntriesGroup => new SalaryFundShare(
          groupAccountEntriesGroup.Key.GroupId,
          Money.CreateRuble(groupAccountEntriesGroup.Sum(entry => entry.Amount.Quantity))))
        .ToHashSet();

      return new SalaryFund(salaryFundSharesSet);
    }
  }
}
