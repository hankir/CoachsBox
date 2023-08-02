using System;
using System.Threading.Tasks;
using CoachsBox.WebApp.Areas.Identity.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CoachsBox.WebApp.Areas.Identity.Pages.Account
{
  [AllowAnonymous]
  public class ConfirmEmailModel : PageModel
  {
    private readonly UserManager<CoachsBoxWebAppUser> _userManager;

    public ConfirmEmailModel(UserManager<CoachsBoxWebAppUser> userManager)
    {
      _userManager = userManager;
    }

    public async Task<IActionResult> OnGetAsync(string userId, string code)
    {
      if (userId == null || code == null)
      {
        return RedirectToPage("/Index");
      }

      var user = await _userManager.FindByIdAsync(userId);
      if (user == null)
      {
        return NotFound($"Не удалось загрузить пользователя с ID '{userId}'.");
      }

      var result = await _userManager.ConfirmEmailAsync(user, code);
      if (!result.Succeeded)
      {
        throw new InvalidOperationException($"Ошибка подтверждения эл.почты для пользователя с ID '{userId}':");
      }

      return Page();
    }
  }
}
