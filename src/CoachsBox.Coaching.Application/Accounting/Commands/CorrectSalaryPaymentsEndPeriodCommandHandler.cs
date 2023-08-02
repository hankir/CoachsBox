using System;
using System.Threading;
using System.Threading.Tasks;
using CoachsBox.Coaching.Accounting.SalaryModel;
using CoachsBox.Core.Primitives;
using MediatR;

namespace CoachsBox.Coaching.Application.Accounting.Commands
{
  public class CorrectSalaryPaymentsEndPeriodCommandHandler : IRequestHandler<CorrectSalaryPaymentsEndPeriodCommand, bool>
  {
    private readonly ISalaryRepository salaryRepository;

    public CorrectSalaryPaymentsEndPeriodCommandHandler(ISalaryRepository salaryRepository)
    {
      this.salaryRepository = salaryRepository;
    }

    public async Task<bool> Handle(CorrectSalaryPaymentsEndPeriodCommand request, CancellationToken cancellationToken)
    {
      if (request == null)
        throw new ArgumentNullException(nameof(request));

      var salarySpecification = new SalarySpecification(request.SalaryId).WithCalculations();
      var salary = await this.salaryRepository.GetBySpecAsync(salarySpecification);

      salary.CorrectPaymentsPeriodEnding(Date.Create(request.PaymentsEndPeriod));

      await this.salaryRepository.UpdateAsync(salary);
      await this.salaryRepository.SaveAsync();

      return true;
    }
  }
}
