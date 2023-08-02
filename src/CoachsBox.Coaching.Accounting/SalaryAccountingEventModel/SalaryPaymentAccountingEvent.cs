using System;
using CoachsBox.Accounting;
using CoachsBox.Coaching.Accounting.GroupAccountModel;
using CoachsBox.Coaching.Accounting.SalaryModel;

namespace CoachsBox.Coaching.Accounting.SalaryAccountingEventModel
{
  public class SalaryPaymentAccountingEvent : SalaryAccountingEvent
  {
    public SalaryPaymentAccountingEvent(Salary salary, Account account, int calculationId, DateTime whenOccured, DateTime whenNoticed)
      : base(SalaryAccountingEventType.CoachSalaryPayment, salary, whenOccured, whenNoticed)
    {
      this.Account = account;
      this.AccountId = account.Id;
      this.CalculationId = calculationId;
    }

    public int AccountId { get; private set; }

    public Account Account { get; private set; }

    /// <summary>
    /// Получить идентификатор расчета.
    /// </summary>
    public int CalculationId { get; private set; }

    private SalaryPaymentAccountingEvent()
    {
      // Требует Entity framework core
    }
  }
}
