using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoachsBox.Coaching.Accounting.StudentAccountEntryModel;
using CoachsBox.Coaching.Accounting.StudentAccountModel;
using CoachsBox.Core.Primitives;
using Microsoft.EntityFrameworkCore;

namespace CoachsBox.Coaching.Infrastructure.Accounting
{
  public class StudentAccountQueryService : IStudentAccountQueryService
  {
    private readonly ReadOnlyCoachsBoxContext context;

    public StudentAccountQueryService(ReadOnlyCoachsBoxContext readOnlyContext)
    {
      this.context = readOnlyContext;
    }

    public async Task<IReadOnlyDictionary<int, Money>> GetStudentsBalancesByGroup(int groupId, DateTime accrualsEndPeriod, DateTime paymentsEndPeriod)
    {
      var result = new Dictionary<int, Money>();
      var studentAccountEntries = (await this.context
       .StudentAccountingEvents
       .Include(studentAccountingEvent => studentAccountingEvent.ProcessingState.ResultingEntries)
       .ThenInclude(resultingEntries => resultingEntries.AccountEntry)
       .ThenInclude(resultingEntry => ((StudentAccountEntry)resultingEntry).StudentAccount)
       .Where(paymentEvent => paymentEvent.GroupId == groupId)
       .Where(accountingEvent => accountingEvent.EventType.Name == StudentAccountEntryType.Payment.Name || (accountingEvent.WhenOccured <= accrualsEndPeriod))
       .SelectMany(studentAccountingEvent => studentAccountingEvent.ProcessingState.ResultingEntries)
       .Select(studentAccountingEvent => studentAccountingEvent.AccountEntry)
       .OfType<StudentAccountEntry>()
       .Where(studentAccountEntry => studentAccountEntry.Date.Date <= paymentsEndPeriod)
       .ToListAsync())
       .GroupBy(studentAccountEntry => studentAccountEntry.StudentAccount.StudentId)
       .Select(studentEntryGroup => new { StudentId = studentEntryGroup.Key, Balance = studentEntryGroup.Sum(a => a.Amount.Quantity) });

      return studentAccountEntries.ToDictionary(e => e.StudentId, e => Money.CreateRuble(e.Balance));
    }

    public async Task<IReadOnlyDictionary<int, Money>> GetStudentsBalances(IEnumerable<int> studentIds)
    {
      var result = new Dictionary<int, Money>();
      var studentAccountEntries = await this.context.StudentAccountEntries
        .Where(studentAccountEntry => studentIds.Contains(studentAccountEntry.StudentAccount.StudentId))
        .Select(studentAccountEntry => new { StudentId = studentAccountEntry.StudentAccount.StudentId, Amount = studentAccountEntry.Amount.Quantity })
        .GroupBy(studentAccountEntry => studentAccountEntry.StudentId)
        .Select(studentEntryGroup => new { StudentId = studentEntryGroup.Key, Balance = studentEntryGroup.Sum(a => a.Amount) })
        .ToListAsync();

      return studentAccountEntries.ToDictionary(e => e.StudentId, e => Money.CreateRuble(e.Balance));
    }

    public async Task<IReadOnlyList<int>> GetStudentsIdsWithDebts()
    {
      return await this.context.StudentAccountEntries
        .Select(studentAccountEntry => new { StudentId = studentAccountEntry.StudentAccount.StudentId, Amount = studentAccountEntry.Amount.Quantity })
        .GroupBy(studentAccountEntry => studentAccountEntry.StudentId)
        .Select(studentEntryGroup => new { StudentId = studentEntryGroup.Key, Balance = studentEntryGroup.Sum(a => a.Amount) })
        .Where(studentBalance => studentBalance.Balance < 0)
        .Select(studentBalance => studentBalance.StudentId)
        .ToListAsync();
    }

    public async Task<IReadOnlyList<int>> GetStudentsIdsWithDebtsByCoach(int coachId)
    {
      var coachsStudentIds = await this.context
        .Schedules
        .Include(schedule => schedule.Group)
        .ThenInclude(group => group.EnrolledStudents)
        .Where(schedule => schedule.CoachId == coachId)
        .SelectMany(schedule => schedule.Group.EnrolledStudents)
        .Select(enrolled => enrolled.StudentId)
        .ToListAsync();

      return await this.context.StudentAccountEntries
        .Where(studentAccountEntry => coachsStudentIds.Contains(studentAccountEntry.StudentAccount.StudentId))
        .Select(studentAccountEntry => new { StudentId = studentAccountEntry.StudentAccount.StudentId, Amount = studentAccountEntry.Amount.Quantity })
        .GroupBy(studentAccountEntry => studentAccountEntry.StudentId)
        .Select(studentEntryGroup => new { StudentId = studentEntryGroup.Key, Balance = studentEntryGroup.Sum(a => a.Amount) })
        .Where(studentBalance => studentBalance.Balance < 0)
        .Select(studentBalance => studentBalance.StudentId)
        .ToListAsync();
    }
  }
}
