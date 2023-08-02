using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using CoachsBox.Coaching.PersonModel;
using CoachsBox.WebApp.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace CoachsBox.WebApp.Areas.Identity.Pages.Account.Manage
{
  public class DeletePersonalDataModel : PageModel
  {
    private readonly UserManager<CoachsBoxWebAppUser> userManager;
    private readonly SignInManager<CoachsBoxWebAppUser> signInManager;
    private readonly ILogger<DeletePersonalDataModel> logger;
    private readonly IPersonRepository personRepository;

    public DeletePersonalDataModel(
        UserManager<CoachsBoxWebAppUser> userManager,
        SignInManager<CoachsBoxWebAppUser> signInManager,
        ILogger<DeletePersonalDataModel> logger,
        IPersonRepository personRepository)
    {
      this.userManager = userManager;
      this.signInManager = signInManager;
      this.logger = logger;
      this.personRepository = personRepository;
    }

    [BindProperty]
    public InputModel Input { get; set; }

    public class InputModel
    {
      [Required]
      [DataType(DataType.Password)]
      [Display(Name = "Пароль")]
      public string Password { get; set; }
    }

    public bool RequirePassword { get; set; }

    public async Task<IActionResult> OnGet()
    {
      var user = await userManager.GetUserAsync(User);
      if (user == null)
      {
        return NotFound($"Unable to load user with ID '{userManager.GetUserId(User)}'.");
      }

      RequirePassword = await userManager.HasPasswordAsync(user);
      return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
      var user = await userManager.GetUserAsync(User);
      if (user == null)
      {
        return NotFound($"Unable to load user with ID '{userManager.GetUserId(User)}'.");
      }

      RequirePassword = await userManager.HasPasswordAsync(user);
      if (RequirePassword)
      {
        if (!await userManager.CheckPasswordAsync(user, Input.Password))
        {
          ModelState.AddModelError(string.Empty, "Password not correct.");
          return Page();
        }
      }

      var result = await userManager.DeleteAsync(user);
      var userId = await userManager.GetUserIdAsync(user);
      if (!result.Succeeded)
      {
        throw new InvalidOperationException($"Unexpected error occurred deleteing user with ID '{userId}'.");
      }

      if (user.PersonId != null)
      {
        var personId = user.PersonId.Value;
        var person = await this.personRepository.GetByIdAsync(personId);
        await this.personRepository.DeleteAsync(person);
        await this.personRepository.SaveAsync();
      }

      await signInManager.SignOutAsync();

      logger.LogInformation("User with ID '{UserId}' deleted themselves.", userId);

      return Redirect("~/");
    }
  }
}