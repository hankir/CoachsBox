using System;
using CoachsBox.Coaching.Accounting.AgreementRegistryEntryModel;
using CoachsBox.Coaching.Accounting.StudentAccountModel;
using CoachsBox.Core.Primitives;

namespace CoachsBox.Coaching.Accounting.StudentAccountingEventModel
{
  public class PaymentAccountingEvent : StudentAccountingEvent
  {
    public Money Amount { get; private set; }

    public AgreementRegistryEntry AgreementRegistryEntry { get; private set; }

    public int AgreementRegistryEntryId { get; private set; }

    public override string BuildAccountEntryDescription()
    {
      return "Оплата тренировок";
    }

    public PaymentAccountingEvent(
      Money amount,
      DateTime whenOccured,
      DateTime whenNoticed,
      StudentAccount account,
      AgreementRegistryEntry agreementRegistryEntry)
      : base(StudentAccountingEventType.Payment, whenOccured, whenNoticed, agreementRegistryEntry.GroupId, account, agreementRegistryEntry.Agreement)
    {
      this.Amount = amount;
      this.AgreementRegistryEntry = agreementRegistryEntry ?? throw new ArgumentNullException(nameof(agreementRegistryEntry));
      this.AgreementRegistryEntryId = agreementRegistryEntry.Id;
    }

    private PaymentAccountingEvent()
    {
      // Требует Entity framework core
    }
  }
}
