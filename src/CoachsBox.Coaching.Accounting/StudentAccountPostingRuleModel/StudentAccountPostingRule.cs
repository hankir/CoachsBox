using System;
using System.Collections.Generic;
using CoachsBox.Accounting;
using CoachsBox.Coaching.Accounting.StudentAccountEntryModel;
using CoachsBox.Coaching.Accounting.StudentAccountingEventModel;
using CoachsBox.Core.Primitives;

namespace CoachsBox.Coaching.Accounting.StudentAccountPostingRuleModel
{
  public abstract class StudentAccountPostingRule : PostingRule
  {
    public static StudentAccountPostingRule CreateFromEventType(StudentAccountingEventType eventType)
    {
      if (eventType.Equals(StudentAccountingEventType.Accrual))
        return new MonthlyAccrualPostingRule(StudentAccountEntryType.Accrual);
      else if (eventType.Equals(StudentAccountingEventType.PersonalTrainingAccrual))
        return new PersonalTrainingAccrualPostingRule(StudentAccountEntryType.Accrual);
      else if (eventType.Equals(StudentAccountingEventType.Payment))
        return new PaymentPostingRule(StudentAccountEntryType.Payment);

      return null;
    }

    public StudentAccountPostingRule(StudentAccountEntryType entryType)
      : base(entryType)
    {
    }

    protected override IEnumerable<AccountEntry> MakeEntries(AccountingEvent accountingEvent, Money amount)
    {
      if (accountingEvent is StudentAccountingEvent coachingAccountingEvent)
      {
        var entryType = StudentAccountEntryType.CreateFrom((StudentAccountEntryType)this.EntryType);
        var description = coachingAccountingEvent.BuildAccountEntryDescription();
        var entry = new StudentAccountEntry(entryType, amount, accountingEvent.WhenOccured, description);
        var account = coachingAccountingEvent.Account;
        account.AddEntry(entry);
        yield return entry;
      }
      else
        throw new ArgumentException($"Unsupported event type [{accountingEvent?.GetType()}]", nameof(accountingEvent));
    }

    protected StudentAccountPostingRule()
    {
      // Требует Entity framework core
    }
  }
}
