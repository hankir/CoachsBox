using CoachsBox.WebApp.Resources;
using MediatR;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Localization;

namespace CoachsBox.WebApp.Areas.Admin.Pages.Salaries
{
  public class IndexModel : PageModel
  {
    public IStringLocalizer<SharedResource> Localizer { get; set; }

    public IndexModel(
      IStringLocalizer<SharedResource> localizer)
    {
      this.Localizer = localizer;
    }

    public void OnGet()
    {
    }
  }
}
