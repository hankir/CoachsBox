using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoachsBox.Coaching.Accounting.SalaryModel;
using CoachsBox.Coaching.Application.Commands;
using CoachsBox.Core;
using CoachsBox.WebApp.AppFacade;
using CoachsBox.WebApp.AppFacade.Accounting.DTO;
using CoachsBox.WebApp.Extensions;
using CoachsBox.WebApp.Resources;
using MediatR;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;

namespace CoachsBox.WebApp.Areas.Admin.Pages.Salaries.Components
{
  public partial class SalariesList : OwningComponentBase<ISalaryRepository>
  {
    private bool isSalaryAdding = false;

    private bool isSalaryLoading = false;

    private bool isCanAddSalary = true;

    private DateTime monthToAdd = DateTime.Today.AddMonths(-1);

    private List<SalaryListItemDTO> salaries = new List<SalaryListItemDTO>();

    private DateTime paymentsPeriod = DateTime.Today;

    private string paymentsPeriodValidation = string.Empty;

    [Parameter]
    public string EmptyListMessage { get; set; }

    private bool IsPaymentsPeriodValid => string.IsNullOrWhiteSpace(this.paymentsPeriodValidation);

    protected override async Task OnInitializedAsync()
    {
      this.paymentsPeriod = new DateTime(this.monthToAdd.Year, this.monthToAdd.Month, DateTime.DaysInMonth(this.monthToAdd.Year, this.monthToAdd.Month));
      await this.LoadCanAddSalary();
      await this.LoadSalaries();
    }

    private async Task LoadCanAddSalary()
    {
      var salaryQueryService = this.ScopedServices.GetRequiredService<ISalaryQueryService>();
      this.isCanAddSalary = (await salaryQueryService.GetNotPaidSalaries()).Count == 0;
    }

    private async Task LoadSalaries()
    {
      try
      {
        this.isSalaryLoading = true;
        var salariesKey = "salaries";
        if (!this.ScopedServices.TryGetCachedData(salariesKey, out this.salaries))
        {
          // TODO: Фильтр по году.
          var salarySpec = new SalaryListSpecification().WithCalculations();
          var salaries = await this.Service.ListAsync(salarySpec.AsReadOnly());
          var localizer = this.ScopedServices.GetRequiredService<IStringLocalizer<SharedResource>>();
          var displayNameService = this.ScopedServices.GetRequiredService<IDisplayNameServiceFacade>();
          var salaryAssembler = new SalaryDTOAssembler(localizer, displayNameService);
          this.salaries = salaryAssembler.ToList(salaries).OrderBy(salary => salary.PeriodBeginning).ToList();
          this.ScopedServices.SetCacheData(salariesKey, this.salaries, TimeSpan.FromSeconds(3));
        }
      }
      finally
      {
        this.isSalaryLoading = false;
      }
    }

    private void PeriodChanged(ChangeEventArgs changeEvent)
    {
      if (DateTime.TryParse(changeEvent.Value as string, out this.monthToAdd))
      {
        this.paymentsPeriod = new DateTime(this.monthToAdd.Year, this.monthToAdd.Month, DateTime.DaysInMonth(this.monthToAdd.Year, this.monthToAdd.Month));
        this.ValidatePaymentsPeriod();
      }
    }

    private void PaymentsPeriodChanged(ChangeEventArgs changeEvent)
    {
      if (DateTime.TryParse(changeEvent.Value as string, out this.paymentsPeriod))
        this.ValidatePaymentsPeriod();
    }

    private void ValidatePaymentsPeriod()
    {
      this.paymentsPeriodValidation = string.Empty;
      var minValue = new DateTime(this.monthToAdd.Year, this.monthToAdd.Month, DateTime.DaysInMonth(this.monthToAdd.Year, this.monthToAdd.Month));
      if (this.paymentsPeriod < minValue)
      {
        this.paymentsPeriodValidation = "Период учета платежей должен быть больше или равен концу указанного месяца расчета зарплаты";
      }
      this.StateHasChanged();
    }

    private async Task AddNewSalary()
    {
      if (!this.IsPaymentsPeriodValid)
        return;

      try
      {
        this.isSalaryAdding = true;
        var mediator = this.ScopedServices.GetRequiredService<IMediator>();
        var createSalaryCommandResult = await mediator.Send(new CreateSalaryCommand(this.monthToAdd.Year, this.monthToAdd.Month, this.paymentsPeriod));
        if (createSalaryCommandResult.IsSuccess())
        {
          await this.LoadSalaries();
          await this.LoadCanAddSalary();
        }
      }
      finally
      {
        this.isSalaryAdding = false;
        this.StateHasChanged();
      }

    }
  }
}
