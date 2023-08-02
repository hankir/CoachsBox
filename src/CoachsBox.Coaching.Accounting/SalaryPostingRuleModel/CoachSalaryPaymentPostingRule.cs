using System;
using System.Collections.Generic;
using CoachsBox.Accounting;
using CoachsBox.Coaching.Accounting.GroupAccountEntryModel;
using CoachsBox.Coaching.Accounting.GroupAccountModel;
using CoachsBox.Coaching.Accounting.SalaryAccountingEventModel;
using CoachsBox.Core.Primitives;

namespace CoachsBox.Coaching.Accounting.SalaryPostingRuleModel
{
  public class CoachSalaryPaymentPostingRule : PostingRule
  {
    protected override Money CalculateAmout(AccountingEvent accountingEvent)
    {
      if (accountingEvent is SalaryPaymentAccountingEvent salaryEvent)
      {
        var salary = salaryEvent.Salary;
        return salary.AmountToIssued(salaryEvent.CalculationId).Negate();
      }

      throw new InvalidOperationException($"Event type {accountingEvent?.EventType?.Name} not supported");
    }

    protected override IEnumerable<AccountEntry> MakeEntries(AccountingEvent accountingEvent, Money amount)
    {
      if (accountingEvent is SalaryPaymentAccountingEvent salaryEvent)
      {
        var groupAccount = (GroupAccount)salaryEvent.Account;
        yield return groupAccount.Withdraw(amount, salaryEvent.WhenOccured);
      }
      else
        throw new ArgumentException($"Unsupported event type [{accountingEvent?.GetType()}]", nameof(accountingEvent));
    }

    public CoachSalaryPaymentPostingRule()
      : base(GroupAccountEntryType.Withdraw)
    {
      // Требует Entity framework core
    }
  }
}
