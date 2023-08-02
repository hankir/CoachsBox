using MediatR;

namespace CoachsBox.Coaching.Accounting.SalaryModel.Events
{
  /// <summary>
  /// Событие о том, что для зарплаты изменен период учета платежей.
  /// </summary>
  public class SalaryPaymentsPeriodEndingChangedEvent : INotification
  {
    public SalaryPaymentsPeriodEndingChangedEvent(Salary salary)
    {
      this.Salary = salary;
    }

    public Salary Salary { get; }
  }
}
