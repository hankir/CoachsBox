using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoachsBox.WebApp.Areas.Admin.Facade;
using CoachsBox.WebApp.Areas.Admin.Facade.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CoachsBox.WebApp.Areas.Admin.Pages.Tariffs
{
  public class DetailsModel : PageModel
  {
    private readonly IAccountingServiceFacade accountingServiceFacade;

    public DetailsModel(IAccountingServiceFacade accountingServiceFacade)
    {
      this.accountingServiceFacade = accountingServiceFacade;
    }

    public TariffDTO Tariff { get; set; }

    public IActionResult OnGet(int tariffId)
    {
      if (tariffId == 0)
        return this.NotFound();

      this.Tariff = this.accountingServiceFacade.ViewTariff(tariffId);
      if (this.Tariff == null)
        return this.NotFound();

      return this.Page();
    }
  }
}
