using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CoachsBox.WebApp.Areas.Identity.Pages.Account
{
  [AllowAnonymous]
  public class ConfirmInvitationConfirmationModel : PageModel
  {
    public void OnGet()
    {

    }
  }
}
