using System;
using System.Threading;
using System.Threading.Tasks;
using CoachsBox.Coaching.Accounting.SalaryModel;
using CoachsBox.Core;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CoachsBox.Coaching.Application.Accounting.Commands
{
  public class CalculateCoachSalaryFromDebtsCommandHandler : IRequestHandler<CalculateCoachSalaryFromDebtsCommand, bool>
  {
    private readonly ILogger<CalculateCoachSalaryFromDebtsCommandHandler> logger;
    private readonly ISalaryQueryService salaryQueryService;
    private readonly ISalaryRepository salaryRepository;

    public CalculateCoachSalaryFromDebtsCommandHandler(
      ILogger<CalculateCoachSalaryFromDebtsCommandHandler> logger,
      ISalaryQueryService salaryQueryService,
      ISalaryRepository salaryRepository)
    {
      this.logger = logger;
      this.salaryQueryService = salaryQueryService;
      this.salaryRepository = salaryRepository;
    }

    public async Task<bool> Handle(CalculateCoachSalaryFromDebtsCommand request, CancellationToken cancellationToken)
    {
      if (request == null)
        throw new ArgumentNullException(nameof(request));

      var salarySpecification = new SalarySpecification(request.SalaryId).WithCalculations();
      var salary = await this.salaryRepository.GetBySpecAsync(salarySpecification);

      if (salary.IsPaid)
      {
        this.logger.LogWarning($"Attempting to calculate debts payed salary. Command aborted.");
        return false;
      }

      salary.ResetCoachDebtCalculation();

      var salaryDebts = await this.salaryQueryService.GetCoachSalaryDebts();
      foreach (var debtEvent in salaryDebts)
      {
        salary.AddCoachSalaryDebtCalculation(debtEvent)
          .Describe($"Долг по зарплате с прошлого периода");
      }

      await this.salaryRepository.UpdateAsync(salary);
      await this.salaryRepository.SaveAsync();

      return true;
    }
  }
}
