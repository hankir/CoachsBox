using System.Collections.Generic;
using System.Threading.Tasks;
using CoachsBox.Coaching.Accounting.SalaryModel;
using CoachsBox.WebApp.AppFacade.Accounting.DTO;
using CoachsBox.WebApp.Resources;
using Microsoft.Extensions.Localization;

namespace CoachsBox.WebApp.AppFacade.Accounting.Internal
{
  public class SalaryServiceFacade : ISalaryServiceFacade
  {
    private readonly ISalaryRepository salaryRepository;
    private readonly ISalaryQueryService salaryQueryService;
    private readonly IStringLocalizer<SharedResource> localizer;
    private readonly IDisplayNameServiceFacade displayNameService;

    public SalaryServiceFacade(
      ISalaryRepository salaryRepository,
      ISalaryQueryService salaryQueryService,
      IStringLocalizer<SharedResource> localizer,
      IDisplayNameServiceFacade displayNameService)
    {
      this.salaryRepository = salaryRepository;
      this.salaryQueryService = salaryQueryService;
      this.localizer = localizer;
      this.displayNameService = displayNameService;
    }

    public async Task<IReadOnlyList<CoachSalaryCalculationDTO>> ViewCoachSalaryCalculationDetails(int salaryId, int coachId)
    {
      var salaryAssembler = new SalaryDTOAssembler(this.localizer, this.displayNameService);
      var coachSalaryCalculations = await this.salaryQueryService.GetCoachSalaryCalculations(salaryId, coachId);
      return await salaryAssembler.ToCoachSalaryCalculationsDTO(coachSalaryCalculations);
    }

    public async Task<SalaryCardDTO> ViewSalary(int salaryId)
    {
      var salarySpecification = new SalarySpecification(salaryId).WithCalculations();
      var salary = await this.salaryRepository.GetBySpecAsync(salarySpecification.AsReadOnly());
      var salaryAssembler = new SalaryDTOAssembler(this.localizer, this.displayNameService);
      return await salaryAssembler.ToDTOAsync(salary);
    }
  }
}
