using System;
using System.Threading;
using System.Threading.Tasks;
using CoachsBox.Coaching.Accounting.SalaryModel;
using CoachsBox.Core.Primitives;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CoachsBox.Coaching.Application.Commands
{
  public class CreateSalaryCommandHandler : IRequestHandler<CreateSalaryCommand, CommandResult>
  {
    private readonly ILogger<CreateSalaryCommandHandler> logger;
    private readonly ISalaryRepository salaryRepository;
    private readonly ISalaryQueryService salaryQueryService;

    public CreateSalaryCommandHandler(
      ILogger<CreateSalaryCommandHandler> logger,
      ISalaryRepository salaryRepository,
      ISalaryQueryService salaryQueryService)
    {
      this.logger = logger;
      this.salaryRepository = salaryRepository;
      this.salaryQueryService = salaryQueryService;
    }

    public async Task<CommandResult> Handle(CreateSalaryCommand request, CancellationToken cancellationToken)
    {
      if (request == null)
        return CommandResult.RequestNullError();

      var notPaidSalaries = await this.salaryQueryService.GetNotPaidSalaries();
      if (notPaidSalaries.Count > 0)
      {
        var errorMessage = "Cannot create new salary while have other not payed salary.";
        this.logger.LogWarning(errorMessage);
        return CommandResult.Fail(errorMessage);
      }

      try
      {
        var paymentsPeriodEnding = Date.Create(request.PaymentsPeriodEnds);
        var salary = new Salary(request.Year, Month.Create(request.Month), paymentsPeriodEnding);
        await this.salaryRepository.AddAsync(salary);
        await this.salaryRepository.SaveAsync();
        return NewEntityCommandResult.Success(salary.Id);
      }
      catch (Exception ex)
      {
        this.logger.LogError(ex, "Error on create salary");
        return CommandResult.Fail(ex.Message);
      }
    }
  }
}
