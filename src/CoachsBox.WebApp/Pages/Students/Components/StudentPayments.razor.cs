using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CoachsBox.WebApp.AppFacade.Students.DTO;
using CoachsBox.WebApp.Areas.Admin.Facade;
using CoachsBox.WebApp.Areas.Admin.Facade.DTO;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;

namespace CoachsBox.WebApp.Pages.Students.Components
{
  public partial class StudentPayments : OwningComponentBase
  {
    private bool isPaymentsHistoryLoading = true;
    private IReadOnlyCollection<PaymentDTO> studentPayments = new List<PaymentDTO>();

    [Parameter]
    public StudentDetailsDTO Student { get; set; }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
      if (firstRender)
      {
        await this.LoadPayments();
        this.StateHasChanged();
      }

      await base.OnAfterRenderAsync(firstRender);
    }

    private async Task LoadPayments()
    {
      this.isPaymentsHistoryLoading = true;
      var paymentServiceFacade = this.ScopedServices.GetRequiredService<IAccountingServiceFacade>();
      this.studentPayments = await paymentServiceFacade.ListStudentPayments(this.Student.StudentId, 1000, DateTime.MinValue);
      this.isPaymentsHistoryLoading = false;
    }
  }
}
