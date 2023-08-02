using System;
using System.Threading.Tasks;
using CoachsBox.Coaching.Application.Accounting.Commands;
using CoachsBox.WebApp.AppFacade.Accounting;
using CoachsBox.WebApp.AppFacade.Accounting.DTO;
using CoachsBox.WebApp.Extensions;
using MediatR;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;

namespace CoachsBox.WebApp.Areas.Admin.Pages.Salaries.Components
{
  public partial class SalaryCard : OwningComponentBase<ISalaryServiceFacade>
  {
    private bool isAddingCalculations;
    private bool isPropagating;
    private SalaryCardDTO salary;
    private CoachSalaryCalculationGroupDTO showedDetails = null;
    private bool isSalaryPaying;
    private string paymentsPeriodValidation = string.Empty;
    private DateTime paymentsPeriod;
    private bool showFortyPercent = true;

    [Parameter]
    public int SalaryId { get; set; }

    private bool IsPaymentsPeriodValid => string.IsNullOrWhiteSpace(this.paymentsPeriodValidation);

    protected override async Task OnInitializedAsync()
    {
      var cacheKey = $"salary:{this.SalaryId}";
      if (!this.ScopedServices.TryGetCachedData(cacheKey, out this.salary))
      {
        this.salary = await this.LoadSalary();
        this.ScopedServices.SetCacheData(cacheKey, this.salary, TimeSpan.FromSeconds(3));
      }
      await this.LoadSalary();
    }

    private async Task Refresh()
    {
      this.salary = await this.LoadSalary();
    }

    private async Task<SalaryCardDTO> LoadSalary()
    {
      var result = await this.Service.ViewSalary(this.SalaryId);
      this.paymentsPeriod = result.PaymentsEndPeriod;
      return result;
    }

    private async Task AddCalculations()
    {
      this.isAddingCalculations = true;
      try
      {
        var mediator = this.ScopedServices.GetRequiredService<IMediator>();

        // Расчет долгов с прошлого периода.
        await mediator.Send(new CalculateCoachSalaryFromDebtsCommand(this.SalaryId));
        // Расчет выполненных работ тренера.
        await mediator.Send(new CalculateCoachSalaryCommand(this.SalaryId));

        // Начисление долга по неоплаченным тренировкам.
        await mediator.Send(new AllocateDebtsForUnpaidTrainingCountsCommand(this.SalaryId));

        await this.Refresh();
      }
      finally
      {
        this.isAddingCalculations = false;
        this.StateHasChanged();
      }
    }

    private async Task PropagateSalaryFund()
    {
      this.isPropagating = true;
      try
      {
        var mediator = this.ScopedServices.GetRequiredService<IMediator>();
        await mediator.Send(new PropagateSalaryCommand(this.SalaryId));
        await this.Refresh();
      }
      finally
      {
        this.isPropagating = false;
        this.StateHasChanged();
      }
    }

    private async Task PaySalary()
    {
      this.isSalaryPaying = true;
      try
      {
        var mediator = this.ScopedServices.GetRequiredService<IMediator>();
        await mediator.Send(new SalaryPayCommand(this.SalaryId));
        await this.Refresh();
      }
      finally
      {
        this.isSalaryPaying = false;
        this.StateHasChanged();
      }
    }

    private void ShowDetails(CoachSalaryCalculationGroupDTO coachSalaryCalculationDTO)
    {
      this.showedDetails = coachSalaryCalculationDTO;
      this.StateHasChanged();
    }

    private async Task ChangePaymentsEndPeriod()
    {
      this.isPropagating = true;
      try
      {
        var mediator = this.ScopedServices.GetRequiredService<IMediator>();
        await mediator.Send(new CorrectSalaryPaymentsEndPeriodCommand(this.salary.Id, this.paymentsPeriod));
        this.salary.PaymentsEndPeriod = this.paymentsPeriod;
      }
      finally
      {
        this.isPropagating = false;
        this.StateHasChanged();
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
      var minValue = this.salary.EndPeriod;
      if (this.paymentsPeriod < minValue)
      {
        this.paymentsPeriodValidation = "Период учета платежей должен быть больше или равен концу указанного месяца расчета зарплаты";
      }
      this.StateHasChanged();
    }

    private void CloseDetails()
    {
      this.showedDetails = null;
    }

    private void OnShowFortyPercentClick()
    {
      this.showFortyPercent = !this.showFortyPercent;
    }
  }
}
