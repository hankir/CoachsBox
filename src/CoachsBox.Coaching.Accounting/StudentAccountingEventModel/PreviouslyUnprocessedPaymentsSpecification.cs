using System;
using CoachsBox.Core;

namespace CoachsBox.Coaching.Accounting.StudentAccountingEventModel
{
  /// <summary>
  /// Спецификация ранее не обработанных платежей.
  /// </summary>
  public class PreviouslyUnprocessedPaymentsSpecification : BaseSpecification<PaymentAccountingEvent>
  {
    /// <summary>
    /// Создать спецификацию ранее не обработанных платежей.
    /// </summary>
    /// <param name="count">Количесво извлекаемых платежей.</param>
    public PreviouslyUnprocessedPaymentsSpecification(int count)
      : base(payment => !payment.ProcessingState.IsProcessed)
    {
      this.ApplyPaging(0, count);
      this.ApplyOrderBy(payment => payment.WhenOccured);
      this.AddInclude(payment => payment.Account);
      this.AddInclude(payment => payment.ServiceAgreement);
      this.AddInclude(payment => payment.AgreementRegistryEntry);
      this.AddInclude(payment => payment.AgreementRegistryEntry.GroupAccount);
      this.AddInclude("ServiceAgreement.PostingRules.PostingRule");
    }

    /// <summary>
    /// Создать спецификацию ранее не обработанных платежей.
    /// </summary>
    /// <param name="whenOccuredFrom">Дата, не ранее которой событие случилось.</param>
    /// <param name="whenOccuredTo">Дата, не позднее которой событие случилось.</param>
    public PreviouslyUnprocessedPaymentsSpecification(DateTime whenOccuredFrom, DateTime whenOccuredTo)
      : base(payment => !payment.ProcessingState.IsProcessed && payment.WhenOccured.Date >= whenOccuredFrom && payment.WhenOccured.Date <= whenOccuredTo)
    {
      this.ApplyOrderBy(payment => payment.WhenOccured);
      this.AddInclude(payment => payment.Account);
      this.AddInclude(payment => payment.ServiceAgreement);
      this.AddInclude(payment => payment.AgreementRegistryEntry);
      this.AddInclude(payment => payment.AgreementRegistryEntry.GroupAccount);
      this.AddInclude("ServiceAgreement.PostingRules.PostingRule");
    }
  }
}
