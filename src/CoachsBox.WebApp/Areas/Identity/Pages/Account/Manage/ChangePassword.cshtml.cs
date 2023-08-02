using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using CoachsBox.WebApp.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace CoachsBox.WebApp.Areas.Identity.Pages.Account.Manage
{
  public class ChangePasswordModel : PageModel
  {
    private readonly UserManager<CoachsBoxWebAppUser> _userManager;
    private readonly SignInManager<CoachsBoxWebAppUser> _signInManager;
    private readonly ILogger<ChangePasswordModel> _logger;

    public ChangePasswordModel(
        UserManager<CoachsBoxWebAppUser> userManager,
        SignInManager<CoachsBoxWebAppUser> signInManager,
        ILogger<ChangePasswordModel> logger)
    {
      _userManager = userManager;
      _signInManager = signInManager;
      _logger = logger;
    }

    [BindProperty]
    public InputModel Input { get; set; }

    [TempData]
    public string StatusMessage { get; set; }

    public class InputModel
    {
      [Required]
      [DataType(DataType.Password)]
      [Display(Name = "Старый пароль")]
      public string OldPassword { get; set; }

      [Required]
      [StringLength(100, ErrorMessage = "{0} должен быть длиной от {2} до {1} символов.", MinimumLength = 6)]
      [DataType(DataType.Password)]
      [Display(Name = "Новый пароль")]
      public string NewPassword { get; set; }

      [DataType(DataType.Password)]
      [Display(Name = "Подтверждение нового пароля")]
      [Compare("NewPassword", ErrorMessage = "Новый пароль и подтверждение нового пароля не совпадают.")]
      public string ConfirmPassword { get; set; }
    }

    public async Task<IActionResult> OnGetAsync()
    {
      var user = await _userManager.GetUserAsync(User);
      if (user == null)
      {
        return NotFound($"Не удалось загрузить пользователя с ID '{_userManager.GetUserId(User)}'.");
      }

      var hasPassword = await _userManager.HasPasswordAsync(user);
      if (!hasPassword)
      {
        return RedirectToPage("./SetPassword");
      }

      return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
      if (!ModelState.IsValid)
      {
        return Page();
      }

      var user = await _userManager.GetUserAsync(User);
      if (user == null)
      {
        return NotFound($"Не удалось загрузить пользователя с ID '{_userManager.GetUserId(User)}'.");
      }

      var changePasswordResult = await _userManager.ChangePasswordAsync(user, Input.OldPassword, Input.NewPassword);
      if (!changePasswordResult.Succeeded)
      {
        foreach (var error in changePasswordResult.Errors)
        {
          ModelState.AddModelError(string.Empty, error.Description);
        }
        return Page();
      }

      await _signInManager.RefreshSignInAsync(user);
      _logger.LogInformation($"User with email {user.Email} changed their password successfully.");
      StatusMessage = "Ваш пароль был изменен.";

      return RedirectToPage();
    }
  }
}
