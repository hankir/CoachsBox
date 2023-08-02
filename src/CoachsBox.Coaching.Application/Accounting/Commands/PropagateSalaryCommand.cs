using MediatR;

namespace CoachsBox.Coaching.Application.Accounting.Commands
{
  public class PropagateSalaryCommand : IRequest<bool>
  {
    public PropagateSalaryCommand(int salaryId)
    {
      this.SalaryId = salaryId;
    }

    public int SalaryId { get; }
  }
}
