using CoachsBox.WebApp.Areas.Admin.Facade;
using CoachsBox.WebApp.Areas.Admin.Facade.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CoachsBox.WebApp.Areas.Admin.Pages.Tariffs
{
  public class EditModel : PageModel
  {
    private readonly IAccountingServiceFacade accountingServiceFacade;

    public EditModel(IAccountingServiceFacade accountingServiceFacade)
    {
      this.accountingServiceFacade = accountingServiceFacade;
    }

    public SelectList AccrualTypeList { get; set; }

    [BindProperty]
    public TariffDTO Tariff { get; set; }

    public IActionResult OnGet(int tariffId)
    {
      if (tariffId == 0)
        return this.NotFound();

      this.Tariff = this.accountingServiceFacade.ViewTariff(tariffId);
      if (this.Tariff == null)
        return this.NotFound();

      var accrualTypes = new AccrualTypeDTOCollection().ListAll();
      this.AccrualTypeList = new SelectList(accrualTypes, "AccrualType", "AccrualTypeName", this.Tariff.AccrualType.AccrualType);

      return this.Page();
    }

    public IActionResult OnPost()
    {
      if (!this.ModelState.IsValid)
        return this.Page();

      this.accountingServiceFacade.UpdateTariff(this.Tariff);
      return this.RedirectToPage("Index");
    }
  }
}
