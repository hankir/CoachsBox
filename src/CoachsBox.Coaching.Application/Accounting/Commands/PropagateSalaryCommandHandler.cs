using System;
using System.Threading;
using System.Threading.Tasks;
using CoachsBox.Coaching.Accounting.SalaryModel;
using CoachsBox.Core;
using MediatR;

namespace CoachsBox.Coaching.Application.Accounting.Commands
{
  public class PropagateSalaryCommandHandler : IRequestHandler<PropagateSalaryCommand, bool>
  {
    private readonly ISalaryQueryService salaryQueryService;
    private readonly ISalaryRepository salaryRepository;

    public PropagateSalaryCommandHandler(
      ISalaryQueryService salaryQueryService,
      ISalaryRepository salaryRepository)
    {
      this.salaryQueryService = salaryQueryService;
      this.salaryRepository = salaryRepository;
    }

    public async Task<bool> Handle(PropagateSalaryCommand request, CancellationToken cancellationToken)
    {
      if (request == null)
        throw new ArgumentNullException(nameof(request));

      var salarySpecification = new SalarySpecification(request.SalaryId).WithCalculations();
      var salary = await this.salaryRepository.GetBySpecAsync(salarySpecification);
      var to = salary.PaymentsPeriodEnding.ToDateTime().Value;
      var salaryFund = await this.salaryQueryService.GetSalaryFundAsync(to);

      var calculatedPropagation = salary.CalculateSalaryPropagation(salaryFund);
      salary.PropagateSalary(salaryFund, calculatedPropagation);

      await this.salaryRepository.UpdateAsync(salary);
      await this.salaryRepository.SaveAsync();

      return true;
    }
  }
}
