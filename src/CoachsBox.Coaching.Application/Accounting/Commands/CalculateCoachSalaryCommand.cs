using MediatR;

namespace CoachsBox.Coaching.Application.Accounting.Commands
{
  public class CalculateCoachSalaryCommand : IRequest<bool>
  {
    public CalculateCoachSalaryCommand(int salaryId)
    {
      this.SalaryId = salaryId;
    }

    public int SalaryId { get; }
  }
}
