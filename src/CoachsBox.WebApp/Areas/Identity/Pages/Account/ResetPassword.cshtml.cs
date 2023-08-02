using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using CoachsBox.WebApp.Areas.Identity.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CoachsBox.WebApp.Areas.Identity.Pages.Account
{
  [AllowAnonymous]
  public class ResetPasswordModel : PageModel
  {
    private readonly UserManager<CoachsBoxWebAppUser> _userManager;

    public ResetPasswordModel(UserManager<CoachsBoxWebAppUser> userManager)
    {
      _userManager = userManager;
    }

    [BindProperty]
    public InputModel Input { get; set; }

    public class InputModel
    {
      [Required]
      [EmailAddress]
      public string Email { get; set; }

      [Required]
      [StringLength(100, ErrorMessage = "{0} должен быть не менее {2} и не более {1} символов.", MinimumLength = 6)]
      [DataType(DataType.Password)]
      [Display(Name = "Пароль")]
      public string Password { get; set; }

      [DataType(DataType.Password)]
      [Display(Name = "Повторите пароль")]
      [Compare(nameof(InputModel.Password), ErrorMessage = "Пароли не совпадают.")]
      public string ConfirmPassword { get; set; }

      public string Code { get; set; }
    }

    public IActionResult OnGet(string code = null)
    {
      if (code == null)
      {
        return BadRequest("По указанной ссылке вы не можете сбросить пароль.");
      }
      else
      {
        Input = new InputModel
        {
          Code = code
        };
        return Page();
      }
    }

    public async Task<IActionResult> OnPostAsync()
    {
      if (!ModelState.IsValid)
      {
        return Page();
      }

      var user = await _userManager.FindByEmailAsync(Input.Email);
      if (user == null)
      {
        // Don't reveal that the user does not exist
        return RedirectToPage("./ResetPasswordConfirmation");
      }

      var result = await _userManager.ResetPasswordAsync(user, Input.Code, Input.Password);
      if (result.Succeeded)
      {
        return RedirectToPage("./ResetPasswordConfirmation");
      }

      foreach (var error in result.Errors)
      {
        ModelState.AddModelError(string.Empty, error.Description);
      }
      return Page();
    }
  }
}
