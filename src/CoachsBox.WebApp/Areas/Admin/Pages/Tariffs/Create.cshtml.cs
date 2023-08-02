using CoachsBox.WebApp.Areas.Admin.Facade;
using CoachsBox.WebApp.Areas.Admin.Facade.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CoachsBox.WebApp.Areas.Admin.Pages.Tariffs
{
  public class CreateModel : PageModel
  {
    private readonly IAccountingServiceFacade accountingServiceFacade;

    public CreateModel(IAccountingServiceFacade accountingServiceFacade)
    {
      this.accountingServiceFacade = accountingServiceFacade;
    }

    public SelectList AccrualTypeList { get; set; }

    [BindProperty]
    public TariffDTO Tariff { get; set; }

    public IActionResult OnGet()
    {
      this.Tariff = new TariffDTO()
      {
        Description = "Новый тарифный план",
        TrainingRate = 150
      };

      var accrualTypes = new AccrualTypeDTOCollection().ListAll();
      this.AccrualTypeList = new SelectList(accrualTypes, "AccrualType", "AccrualTypeName");

      return this.Page();
    }

    public IActionResult OnPost()
    {
      if (!this.ModelState.IsValid)
        return this.Page();

      this.accountingServiceFacade.CreateTariff(this.Tariff);
      return this.RedirectToPage("Index");
    }
  }
}
