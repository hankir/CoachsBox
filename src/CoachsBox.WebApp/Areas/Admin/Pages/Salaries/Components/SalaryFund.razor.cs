using System;
using System.Threading.Tasks;
using CoachsBox.Coaching.Accounting.SalaryModel;
using CoachsBox.WebApp.Extensions;
using Microsoft.AspNetCore.Components;

namespace CoachsBox.WebApp.Areas.Admin.Pages.Salaries.Components
{
  public partial class SalaryFund : OwningComponentBase<ISalaryQueryService>
  {
    private bool isLoading = true;
    private decimal salaryFundBalance;

    [Parameter]
    public DateTime From { get; set; }

    [Parameter]
    public DateTime To { get; set; }

    protected override async Task OnInitializedAsync()
    {
      var cacheKey = $"{this.From}-{this.To}";
      if (!this.ScopedServices.TryGetCachedData(cacheKey, out this.salaryFundBalance))
      {
        var salaryFund = await this.Service.GetSalaryFundAsync(this.To);
        this.salaryFundBalance = salaryFund.Total().Quantity;
        this.ScopedServices.SetCacheData(cacheKey, this.salaryFundBalance, TimeSpan.FromSeconds(5));
      }
      this.isLoading = false;
    }
  }
}
