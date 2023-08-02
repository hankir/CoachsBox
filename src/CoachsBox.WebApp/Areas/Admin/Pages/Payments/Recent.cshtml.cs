using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using CoachsBox.Coaching.Application.Commands;
using CoachsBox.WebApp.Areas.Admin.Facade;
using CoachsBox.WebApp.Areas.Admin.Facade.DTO;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CoachsBox.WebApp.Areas.Admin.Pages.Payments
{
  public class RecentModel : PageModel
  {
    private readonly IAccountingServiceFacade paymentServiceFacade;
    private readonly IMediator mediator;

    public RecentModel(IAccountingServiceFacade paymentServiceFacade, IMediator mediator)
    {
      this.paymentServiceFacade = paymentServiceFacade;
      this.mediator = mediator;
    }

    public IReadOnlyCollection<PaymentDTO> Payments { get; private set; }

    [BindProperty]
    public string StudentName { get; set; }

    public void OnGet(string studentName)
    {
      var payments = string.IsNullOrWhiteSpace(studentName) ?
        this.paymentServiceFacade.ListRecent(100) :
        this.paymentServiceFacade.FindPaymentsByStudentName(100, studentName);

      this.Payments = payments;
      this.StudentName = studentName;
    }

    public async Task<IActionResult> OnPostDeletePaymentAsync(int paymentId)
    {
      if (paymentId <= 0)
        return this.BadRequest();

      await this.mediator.Send(new DeletePaymentCommand(paymentId));
      return this.RedirectToPage("Recent", new { this.StudentName });
    }

    public IActionResult OnPost()
    {
      // PRG паттерн https://en.wikipedia.org/wiki/Post/Redirect/Get.
      return this.RedirectToPage("Recent", new { this.StudentName });
    }
  }
}
