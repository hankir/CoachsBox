using MediatR;

namespace CoachsBox.Coaching.Accounting.SalaryModel.Events
{
  /// <summary>
  /// Событие о том, что расчет зарплаты создан.
  /// </summary>
  public class SalaryCreatedEvent : INotification
  {
    public SalaryCreatedEvent(Salary salary)
    {
      this.Salary = salary;
    }

    public Salary Salary { get; }
  }
}
