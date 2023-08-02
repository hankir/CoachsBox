using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CoachsBox.WebApp.AppFacade.Accounting;
using CoachsBox.WebApp.AppFacade.Accounting.DTO;
using Microsoft.AspNetCore.Components;

namespace CoachsBox.WebApp.Areas.Admin.Pages.Salaries.Components
{
  public partial class SalaryCoachCalculationDetails : OwningComponentBase<ISalaryServiceFacade>
  {
    private IReadOnlyList<CoachSalaryCalculationDTO> calculations = new List<CoachSalaryCalculationDTO>();

    [Parameter]
    public int SalaryId { get; set; }

    [Parameter]
    public int CoachId { get; set; }

    [Parameter]
    public DateTime PeriodBeginning { get; set; }

    [Parameter]
    public DateTime PaymentsEndPeriod { get; set; }

    protected override async Task OnInitializedAsync()
    {
      this.calculations = await this.Service.ViewCoachSalaryCalculationDetails(this.SalaryId, this.CoachId);
    }
  }
}
