using System;
using System.Collections.Generic;
using System.Text;
using MediatR;

namespace CoachsBox.Coaching.Application.Accounting.Commands
{
  public class CalculateCoachSalaryFromDebtsCommand : IRequest<bool>
  {
    public CalculateCoachSalaryFromDebtsCommand(int salaryId)
    {
      this.SalaryId = salaryId;
    }

    public int SalaryId { get; }
  }
}
