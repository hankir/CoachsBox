using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using CoachsBox.WebApp.Areas.Identity.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CoachsBox.WebApp.Areas.Identity.Pages.Account
{
  [AllowAnonymous]
  public class ConfirmInvitationModel : PageModel
  {
    private readonly UserManager<CoachsBoxWebAppUser> _userManager;

    public ConfirmInvitationModel(UserManager<CoachsBoxWebAppUser> userManager)
    {
      _userManager = userManager;
    }

    [BindProperty]
    public InputModel Input { get; set; }

    public async Task<IActionResult> OnGetAsync(string userId, string code)
    {
      if (string.IsNullOrWhiteSpace(userId) || string.IsNullOrWhiteSpace(code))
        return RedirectToPage("/Index");

      var user = await _userManager.FindByIdAsync(userId);
      if (user == null)
        return NotFound($"Пользователь не найден '{userId}'.");

      if (user.EmailConfirmed)
        return this.RedirectToPage("Login");

      this.Input = new InputModel()
      {
        Code = code,
        UserId = userId
      };
      return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
      if (!this.ModelState.IsValid)
        return this.Page();

      var userId = this.Input.UserId;
      var user = await _userManager.FindByIdAsync(userId);
      if (user == null)
        return NotFound($"Пользователь не найден '{userId}'.");

      var errors = new List<IdentityError>();
      foreach (var validator in this._userManager.PasswordValidators)
      {
        var validatePasswordResult = await validator.ValidateAsync(this._userManager, user, this.Input.Password);
        if (!validatePasswordResult.Succeeded)
          errors.AddRange(validatePasswordResult.Errors);
      }

      if (errors.Count == 0)
      {
        var code = this.Input.Code;
        var confirmResult = await _userManager.ConfirmEmailAsync(user, code);
        if (!confirmResult.Succeeded)
          return BadRequest($"Ссылка подтверждения почты не действительна.");

        string resetPasswordToken = await this._userManager.GeneratePasswordResetTokenAsync(user);
        var result = await _userManager.ResetPasswordAsync(user, resetPasswordToken, Input.Password);
        if (result.Succeeded)
          return RedirectToPage("./ConfirmInvitationConfirmation");

        errors = new List<IdentityError>(result.Errors);
      }

      foreach (var error in errors)
        ModelState.AddModelError(string.Empty, error.Description);

      return Page();
    }

    public class InputModel
    {
      [Required]
      public string Code { get; set; }

      [Required]
      public string UserId { get; set; }

      [Required]
      [StringLength(100, ErrorMessage = "{0} должен быть не менее {2} и не более {1} символов.", MinimumLength = 6)]
      [DataType(DataType.Password)]
      [Display(Name = "Пароль")]
      public string Password { get; set; }

      [DataType(DataType.Password)]
      [Display(Name = "Повторите пароль")]
      [Compare(nameof(Password), ErrorMessage = "Пароли не совпадают.")]
      public string ConfirmPassword { get; set; }
    }
  }
}