using System;
using System.Threading;
using System.Threading.Tasks;
using CoachsBox.Coaching.Accounting.AgreementRegistryEntryModel;
using CoachsBox.Coaching.Accounting.SalaryModel;
using CoachsBox.Coaching.AttendanceLogModel;
using CoachsBox.Coaching.CoachModel;
using CoachsBox.Core;
using CoachsBox.Core.Primitives;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CoachsBox.Coaching.Application.Accounting.Commands
{
  public class CalculateCoachSalaryCommandHandler : IRequestHandler<CalculateCoachSalaryCommand, bool>
  {
    private readonly ILogger<CalculateCoachSalaryCommandHandler> logger;
    private readonly ISalaryRepository salaryRepository;
    private readonly ICoachQueryService coachQueryService;
    private readonly IAttendanceQueryService attendanceQueryService;
    private readonly IAccrualService accrualService;
    private readonly IAgreementRegistryEntryRepository agreementRegistryEntryRepository;

    public CalculateCoachSalaryCommandHandler(
      ILogger<CalculateCoachSalaryCommandHandler> logger,
      ISalaryRepository salaryRepository,
      ICoachQueryService coachQueryService,
      IAttendanceQueryService attendanceQueryService,
      IAccrualService accrualService,
      IAgreementRegistryEntryRepository agreementRegistryEntryRepository)
    {
      this.logger = logger;
      this.salaryRepository = salaryRepository;
      this.coachQueryService = coachQueryService;
      this.attendanceQueryService = attendanceQueryService;
      this.accrualService = accrualService;
      this.agreementRegistryEntryRepository = agreementRegistryEntryRepository;
    }

    public async Task<bool> Handle(CalculateCoachSalaryCommand request, CancellationToken cancellationToken)
    {
      if (request == null)
        throw new ArgumentNullException(nameof(request));

      var salarySpecification = new SalarySpecification(request.SalaryId).WithCalculations();
      var salary = await this.salaryRepository.GetBySpecAsync(salarySpecification);

      if (salary.IsPaid)
      {
        this.logger.LogWarning($"Attempting to calculate payed salary. Command aborted.");
        return false;
      }

      salary.ResetCoachCalculation();

      var coachIds = await this.coachQueryService.ListAllCoachIdsAsync();
      foreach (var coachId in coachIds)
      {
        var from = salary.PeriodBeginning.ToDateTime().Value;
        var to = salary.PeriodEnding.ToDateTime().Value;
        var groupMarksMap = await this.attendanceQueryService.GetAttendanceMarksByCoachAsync(coachId, from, to);

        foreach (var groupMarks in groupMarksMap)
        {
          var trainingCounts = this.accrualService.AccountableTrainingCount(groupMarks.Value);
          if (trainingCounts > 0)
          {
            var groupAggrementSpec = new AgreementRegistryEntryByGroupSpecification(groupMarks.Key).AsReadOnly();
            var groupAggrement = await this.agreementRegistryEntryRepository.GetBySpecAsync(groupAggrementSpec);
            if (groupAggrement != null)
            {
              salary.AddCoachSalaryCalculation(coachId, groupMarks.Key, trainingCounts, GetTrainingCost(groupAggrement))
                .Describe("Посещения и пропуски без уважительной причины");
            }
          }
        }
      }

      await this.salaryRepository.UpdateAsync(salary);
      await this.salaryRepository.SaveAsync();

      return true;
    }

    private static Money GetTrainingCost(AgreementRegistryEntry groupAggrement)
    {
      return Money.CreateRuble(groupAggrement.Agreement.Rate.Quantity);
    }
  }
}
