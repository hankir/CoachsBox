using System;
using System.Collections.Generic;
using CoachsBox.Accounting;
using CoachsBox.Coaching.Accounting.GroupAccountModel;
using CoachsBox.Coaching.Accounting.StudentAccountEntryModel;
using CoachsBox.Coaching.Accounting.StudentAccountingEventModel;
using CoachsBox.Core.Primitives;

namespace CoachsBox.Coaching.Accounting.StudentAccountPostingRuleModel
{
  public class PaymentPostingRule : StudentAccountPostingRule
  {
    public PaymentPostingRule(StudentAccountEntryType entryType)
      : base(entryType)
    {
    }

    protected override Money CalculateAmout(AccountingEvent accountingEvent)
    {
      var paymentEvent = (PaymentAccountingEvent)accountingEvent;
      return Money.CreateRuble(paymentEvent.Amount.Quantity);
    }

    protected override IEnumerable<AccountEntry> MakeEntries(AccountingEvent accountingEvent, Money amount)
    {
      if (accountingEvent is PaymentAccountingEvent paymentAccountingEvent)
      {
        var studentAccountEntryType = StudentAccountEntryType.CreateFrom((StudentAccountEntryType)this.EntryType);
        var description = paymentAccountingEvent.BuildAccountEntryDescription();
        var studentAccountEntry = new StudentAccountEntry(studentAccountEntryType, Money.CreateRuble(amount.Quantity), accountingEvent.WhenOccured, description);

        var account = paymentAccountingEvent.Account;
        account.AddEntry(studentAccountEntry);
        yield return studentAccountEntry;

        var agreementRegistryEntry = paymentAccountingEvent.AgreementRegistryEntry;
        var groupAccount = this.GetGroupAccount(agreementRegistryEntry);
        yield return groupAccount.Deposit(Money.CreateRuble(amount.Quantity), accountingEvent.WhenOccured);
      }
      else
        throw new ArgumentException($"Unsupported event type [{accountingEvent?.GetType()}]", nameof(accountingEvent));
    }

    private GroupAccount GetGroupAccount(AgreementRegistryEntryModel.AgreementRegistryEntry agreementRegistryEntry)
    {
      return agreementRegistryEntry.GroupAccount ??
        throw new InvalidOperationException($"Group account not found for group by aggrement registry entry. GroupId: {agreementRegistryEntry.GroupId}, AggrementId: {agreementRegistryEntry.Id}");
    }

    private PaymentPostingRule()
    {
      // Требует Entity framework core
    }
  }
}
