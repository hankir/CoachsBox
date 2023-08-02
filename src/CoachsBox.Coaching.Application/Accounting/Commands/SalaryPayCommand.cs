using System;
using System.Collections.Generic;
using System.Text;
using MediatR;

namespace CoachsBox.Coaching.Application.Accounting.Commands
{
  public class SalaryPayCommand : IRequest<bool>
  {
    public SalaryPayCommand(int salaryId)
    {
      this.SalaryId = salaryId;
    }

    public int SalaryId { get; }
  }
}
