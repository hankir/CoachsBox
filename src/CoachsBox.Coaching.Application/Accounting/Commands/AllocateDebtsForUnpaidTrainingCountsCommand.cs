using MediatR;

namespace CoachsBox.Coaching.Application.Accounting.Commands
{
  public class AllocateDebtsForUnpaidTrainingCountsCommand : IRequest<bool>
  {
    public AllocateDebtsForUnpaidTrainingCountsCommand(int salaryId)
    {
      this.SalaryId = salaryId;
    }

    public int SalaryId { get; }
  }
}
