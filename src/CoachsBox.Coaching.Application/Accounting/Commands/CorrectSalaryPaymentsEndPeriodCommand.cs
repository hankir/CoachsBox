using System;
using System.Collections.Generic;
using System.Text;
using MediatR;

namespace CoachsBox.Coaching.Application.Accounting.Commands
{
  public class CorrectSalaryPaymentsEndPeriodCommand : IRequest<bool>
  {
    public CorrectSalaryPaymentsEndPeriodCommand(int salaryId, DateTime newPaymentsEndPeriod)
    {
      this.SalaryId = salaryId;
      this.PaymentsEndPeriod = newPaymentsEndPeriod;
    }

    public int SalaryId { get; }

    public DateTime PaymentsEndPeriod { get; }
  }
}
