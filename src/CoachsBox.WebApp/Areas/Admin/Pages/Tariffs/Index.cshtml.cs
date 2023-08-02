using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using CoachsBox.Coaching.Application;
using CoachsBox.Core.Primitives;
using CoachsBox.WebApp.Areas.Admin.Facade;
using CoachsBox.WebApp.Areas.Admin.Facade.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CoachsBox.WebApp.Areas.Admin.Pages.Tariffs
{
  public class IndexModel : PageModel
  {
    private readonly IAccountingServiceFacade accountingServiceFacade;
    private readonly IAccrualService accrualService;

    public IndexModel(
      IAccountingServiceFacade accountingServiceFacade,
      IAccrualService accrualService)
    {
      this.accountingServiceFacade = accountingServiceFacade;
      this.accrualService = accrualService;
    }

    public List<TariffDTO> Tariffs { get; private set; }

    [BindProperty]
    [Display(Name = "Начало периода")]
    public DateTime AccruePeriodStart { get; set; }

    [BindProperty]
    [Display(Name = "Конец периода")]
    public DateTime AccruePeriodEnd { get; set; }

    public void OnGet()
    {
      this.Tariffs = this.accountingServiceFacade.ListTariffs().ToList();
      this.AccruePeriodStart = this.User.GetUserNow().AddDays(-1).Date;
      this.AccruePeriodEnd = this.User.GetUserNow().Date;
    }

    public IActionResult OnPostCalculateAccrualsForNow()
    {
      var date = Date.Create(this.User.FromUserTime(this.User.GetUserNow()));
      this.accrualService.CalculateAccrualForDate(date);
      return this.RedirectToPage("Index");
    }

    public IActionResult OnPostCalculateAccruals()
    {
      if (this.ModelState.IsValid)
      {
        var from = Date.Create(this.User.FromUserTime(this.AccruePeriodStart));
        var to = Date.Create(this.User.FromUserTime(this.AccruePeriodEnd));
        this.accrualService.CalculateAccrualForPeriod(from, to);
      }
      return this.RedirectToPage("Index");
    }

    public IActionResult OnPostProcessAccrual()
    {
      this.accrualService.ProcessAccruals();
      return this.RedirectToPage("Index");
    }
  }
}
