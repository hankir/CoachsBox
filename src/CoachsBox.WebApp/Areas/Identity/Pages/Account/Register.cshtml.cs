using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using CoachsBox.WebApp.AppFacade.Primitives.Commands;
using CoachsBox.WebApp.Areas.Admin.Facade;
using CoachsBox.WebApp.Areas.Identity.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace CoachsBox.WebApp.Areas.Identity.Pages.Account
{
  [AllowAnonymous]
  public class RegisterModel : PageModel
  {
    private readonly SignInManager<CoachsBoxWebAppUser> signInManager;
    private readonly UserManager<CoachsBoxWebAppUser> userManager;
    private readonly RoleManager<CoachsBoxWebAppRole> roleManager;
    private readonly ILogger<RegisterModel> logger;
    private readonly IEmailSender emailSender;
    private readonly IAdministrationServiceFacade administrationService;

    public RegisterModel(
        UserManager<CoachsBoxWebAppUser> userManager,
        RoleManager<CoachsBoxWebAppRole> roleManager,
        SignInManager<CoachsBoxWebAppUser> signInManager,
        ILogger<RegisterModel> logger,
        IEmailSender emailSender,
        IAdministrationServiceFacade administrationService)
    {
      this.userManager = userManager;
      this.roleManager = roleManager;
      this.signInManager = signInManager;
      this.logger = logger;
      this.emailSender = emailSender;
      this.administrationService = administrationService;
    }

    [BindProperty]
    public InputModel Input { get; set; }

    public string ReturnUrl { get; set; }

    public class InputModel : CreatePersonCommandBase
    {
      [Required]
      [StringLength(100, ErrorMessage = "{0} должен быть не менее {2} не более {1} символов.", MinimumLength = 6)]
      [DataType(DataType.Password)]
      [Display(Name = "Пароль")]
      public string Password { get; set; }

      [DataType(DataType.Password)]
      [Display(Name = "Подтверждение пароля")]
      [Compare("Password", ErrorMessage = "Пароль и подтверждение пароля не совпадают.")]
      public string ConfirmPassword { get; set; }

      [Required(ErrorMessage = "Телефон обязателен для заполнения")]
      public override string PhoneNumber { get; set; }

      [Required(ErrorMessage = "Эл. почта обязательна для заполнения")]
      public override string Email { get; set; }
    }

    public void OnGet(string returnUrl = null)
    {
      ReturnUrl = returnUrl;
    }

    public async Task<IActionResult> OnPostAsync(string returnUrl = null)
    {
      returnUrl ??= Url.Content("~/");
      if (ModelState.IsValid)
      {
        if (!await this.roleManager.RoleExistsAsync(CoachsBoxWebAppRole.Administrator))
        {
          var createRoleResult = await this.roleManager.CreateAsync(CoachsBoxWebAppRole.CreateAdmin());
          if (!createRoleResult.Succeeded)
          {
            this.ModelState.AddModelError("canNotCreateAdministrator", "Не удалось зарегистрироваться в системе, обратитесь в поддержку.");
            return this.Page();
          }
        }
        else
        {
          // Есть пользователи в группе администраторы.
          var administrators = await this.userManager.GetUsersInRoleAsync(CoachsBoxWebAppRole.Administrator);
          if (administrators.Any())
          {
            this.ModelState.AddModelError("administratorAlreadyExist", "В системе уже зарегистрирован администратор.");
            return this.Page();
          }
        }

        var personId = this.administrationService.CreatePerson(this.Input);
        var user = new CoachsBoxWebAppUser
        {
          UserName = this.Input.Email,
          Email = Input.Email,
          PhoneNumber = this.Input.PhoneNumber,
          PersonId = personId
        };
        var result = await userManager.CreateAsync(user, Input.Password);
        if (result.Succeeded)
        {
          var addInRoleResult = await this.userManager.AddToRoleAsync(user, CoachsBoxWebAppRole.Administrator);
          if (addInRoleResult.Succeeded)
          {
            logger.LogInformation("User created a new account with password.");

            var code = await userManager.GenerateEmailConfirmationTokenAsync(user);
            var callbackUrl = Url.Page(
                "/Account/ConfirmEmail",
                pageHandler: null,
                values: new { userId = user.Id, code },
                protocol: Request.Scheme);

            await emailSender.SendEmailAsync(Input.Email, "Подвердите вашу электронную почту",
                $"Пожалуйста, подтвердите учетную запись. Перейдите по <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>ссылке</a>." +
                $"<br />Если учетную запись в цифровой тренерской создавали не вы, то просто проигнорируйте это письмо.");

            await signInManager.SignInAsync(user, isPersistent: false);
            return LocalRedirect(returnUrl);
          }
        }
        foreach (var error in result.Errors)
        {
          ModelState.AddModelError(string.Empty, error.Description);
        }
      }

      // If we got this far, something failed, redisplay form
      return Page();
    }
  }
}
