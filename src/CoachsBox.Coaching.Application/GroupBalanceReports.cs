using System;
using System.Collections.Generic;
using System.Linq;
using CoachsBox.Coaching.Accounting.StudentAccountEntryModel;
using CoachsBox.Coaching.Accounting.StudentAccountingEventModel;
using CoachsBox.Core.Primitives;

namespace CoachsBox.Coaching.Application
{
  public class GroupBalanceReports
  {
    private readonly IReadOnlyList<StudentAccountingEvent> groupCoachingAccountingEventsList;

    public GroupBalanceReports(IReadOnlyList<StudentAccountingEvent> coachingAccountingEvents)
    {
      this.groupCoachingAccountingEventsList = coachingAccountingEvents;
    }

    public Money CalculateBalanceByStudentId(int studentId)
    {
      var amounts = new List<Money>();
      foreach (var accountingEvent in this.groupCoachingAccountingEventsList)
      {
        if (accountingEvent.Account.StudentId != studentId)
          continue;

        amounts.AddRange(this.ListStudentAccountResultingEntriesAmounts(accountingEvent));
      }

      var result = Money.CreateRuble(0);
      foreach (var amount in amounts)
        result = result.Add(amount);
      return result;
    }

    public Money CalculateBalance()
    {
      var amounts = new List<Money>();
      foreach (var accountingEvent in this.groupCoachingAccountingEventsList)
      {
        amounts.AddRange(this.ListStudentAccountResultingEntriesAmounts(accountingEvent));
      }

      var result = Money.CreateRuble(0);
      foreach (var amount in amounts)
        result = result.Add(amount);
      return result;
    }

    public Money CalculatePaymentsByPeriod(DateTime paymentsStartPeriod, DateTime paymentsEndPeriod)
    {
      return Money.CreateRuble(
        this.groupCoachingAccountingEventsList
          .OfType<PaymentAccountingEvent>()
          .Where(e => e.WhenOccured >= paymentsStartPeriod && e.WhenOccured <= paymentsEndPeriod)
          .Sum(e => e.Amount.Quantity));
    }

    public Money CalculatePayments()
    {
      return Money.CreateRuble(
        this.groupCoachingAccountingEventsList
          .OfType<PaymentAccountingEvent>()
          .Sum(e => e.Amount.Quantity));
    }

    private IReadOnlyList<Money> ListStudentAccountResultingEntriesAmounts(StudentAccountingEvent accountingEvent)
    {
      return accountingEvent
        .ProcessingState
        .ResultingEntries
        .Where(resultingEntry => resultingEntry.AccountEntry is StudentAccountEntry)
        .Select(resultingEntry => resultingEntry.AccountEntry.Amount)
        .ToList();
    }
  }
}
