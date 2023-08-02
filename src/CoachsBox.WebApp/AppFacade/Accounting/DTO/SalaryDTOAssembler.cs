using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using CoachsBox.Coaching.Accounting.SalaryModel;
using CoachsBox.Core;
using CoachsBox.WebApp.Resources;
using Microsoft.Extensions.Localization;

namespace CoachsBox.WebApp.AppFacade.Accounting.DTO
{
  public class SalaryDTOAssembler
  {
    private readonly IStringLocalizer<SharedResource> localizer;
    private readonly IDisplayNameServiceFacade displayNameService;

    public List<SalaryListItemDTO> ToList(IEnumerable<Salary> salaries)
    {
      var dateTimeFormat = CultureInfo.CurrentCulture.DateTimeFormat;
      return salaries.Select(salary => new SalaryListItemDTO()
      {
        Id = salary.Id,
        PeriodBeginning = salary.PeriodBeginning.ToDateTime().Value,
        PeriodEnding = salary.PeriodEnding.ToDateTime().Value,
        PaymentsPeriodEnding = salary.PaymentsPeriodEnding.ToDateTime().Value,
        DisplayName = $"{this.localizer[dateTimeFormat.GetMonthName(salary.PeriodBeginning.Month.Number)]} {salary.PeriodBeginning.Year}",
        TotalAmount = salary.TotalAmount().Quantity,
        TotalAmountToIssued = salary.TotalAmountToIssued().Quantity,
        IsPaid = salary.IsPaid
      }).ToList();
    }

    public async Task<SalaryCardDTO> ToDTOAsync(Salary salary)
    {
      if (salary == null)
        throw new ArgumentNullException(nameof(salary));

      var dateTimeFormat = CultureInfo.CurrentCulture.DateTimeFormat;
      var result = new SalaryCardDTO()
      {
        Id = salary.Id,
        BeginPeriod = salary.PeriodBeginning.ToDateTime().Value,
        EndPeriod = salary.PeriodEnding.ToDateTime().Value,
        PaymentsEndPeriod = salary.PaymentsPeriodEnding.ToDateTime().Value,
        DisplayName = $"{this.localizer[dateTimeFormat.GetMonthName(salary.PeriodBeginning.Month.Number)]} {salary.PeriodBeginning.Year}",
        TotalAmount = salary.TotalAmount().Quantity,
        TotalAmountToIssued = salary.TotalAmountToIssued().Quantity,
        IsPaid = salary.IsPaid
      };

      int[] coachsIds = GetSalaryCoachsIds(salary);
      var coachsNames = await this.displayNameService.ListCoachsNamesAsync(coachsIds.ToArray());
      foreach (var coachId in coachsIds)
      {
        result.CoachSalaryCalculationsGroups.Add(new CoachSalaryCalculationGroupDTO()
        {
          CoachId = coachId,
          CoachFullName = coachsNames.TryGetValue(coachId, out var coachName) ? coachName.FullName() : $"Тренер [id: {coachId}]",
          TotalAmount = salary.TotalCoachAmount(coachId).Quantity,
          TotalToIssued = salary.TotalCoachAmountToIssued(coachId).Quantity
        });
      }

      return result;
    }

    public async Task<List<CoachSalaryCalculationDTO>> ToCoachSalaryCalculationsDTO(IEnumerable<CoachSalaryCalculation> coachSalaryCalculations)
    {
      if (coachSalaryCalculations == null)
        throw new ArgumentNullException(nameof(coachSalaryCalculations));

      var groupIds = GetGroupIds(coachSalaryCalculations);
      var groupNames = await this.displayNameService.ListGroupNamesAsync(groupIds);
      return coachSalaryCalculations.Select(calculation => new CoachSalaryCalculationDTO()
      {
        Id = $"{Guid.NewGuid()}",
        Amount = calculation.Amount.Quantity,
        AmountToIssued = calculation.AmountToIssued.Quantity,
        CoachId = calculation.CoachId,
        Description = calculation.Description,
        GroupId = calculation.GroupId,
        GroupName = groupNames.TryGetValue(calculation.GroupId, out var groupName) ? groupName : $"Группа [id: {calculation.GroupId}]",
        TrainingCost = calculation.TrainingCost.Quantity,
        TrainingCount = calculation.TrainingCount,
        DebtTrainingCount = calculation.CountDebtTrainings(),
        IsDebt = calculation is CoachSalaryDebtCalculation || calculation is CoachSalaryAllocatedDebtCalculation
      })
        .ToList();
    }

    private static int[] GetGroupIds(IEnumerable<CoachSalaryCalculation> coachSalaryCalculations)
    {
      return coachSalaryCalculations
        .Select(c => c.GroupId)
        .Distinct()
        .ToArray();
    }

    private static int[] GetSalaryCoachsIds(Salary salary)
    {
      return salary
        .Calculations
        .OfType<CoachSalaryCalculation>()
        .Select(c => c.CoachId)
        .Distinct()
        .ToArray();
    }

    public SalaryDTOAssembler(
      IStringLocalizer<SharedResource> localizer,
      IDisplayNameServiceFacade displayNameService)
    {
      this.localizer = localizer;
      this.displayNameService = displayNameService;
    }
  }
}
