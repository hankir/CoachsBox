using System.Collections.Generic;
using System.Threading.Tasks;
using CoachsBox.WebApp.AppFacade.Accounting.DTO;

namespace CoachsBox.WebApp.AppFacade.Accounting
{
  public interface ISalaryServiceFacade
  {
    Task<SalaryCardDTO> ViewSalary(int salaryId);

    Task<IReadOnlyList<CoachSalaryCalculationDTO>> ViewCoachSalaryCalculationDetails(int salaryId, int coachId);
  }
}
