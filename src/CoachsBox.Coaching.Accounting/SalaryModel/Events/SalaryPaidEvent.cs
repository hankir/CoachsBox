using MediatR;

namespace CoachsBox.Coaching.Accounting.SalaryModel.Events
{
  public class SalaryPaidEvent : INotification
  {
    public SalaryPaidEvent(Salary salary)
    {
      this.Salary = salary;
    }

    public Salary Salary { get; }
  }
}