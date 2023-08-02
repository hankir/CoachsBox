using CoachsBox.Coaching.Accounting.CoachingAccountingEventModel;
using CoachsBox.Core;

namespace CoachsBox.Coaching.Accounting.StudentAccountingEventModel
{
  public class PaymentSpecification : BaseSpecification<PaymentAccountingEvent>
  {
    public PaymentSpecification(int paymentId)
      : base(payment => payment.Id == paymentId)
    {
    }

    public PaymentSpecification WithResultingAccountEntries()
    {
      this.AddInclude($"{nameof(PaymentAccountingEvent.ProcessingState)}.{nameof(PaymentAccountingEvent.ProcessingState.ResultingEntries)}.{nameof(AccountingEventResultingEntry.AccountEntry)}");
      return this;
    }
  }
}
