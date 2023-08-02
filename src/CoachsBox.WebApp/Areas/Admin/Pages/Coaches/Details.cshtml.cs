using System;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using CoachsBox.Coaching.CoachModel;
using CoachsBox.WebApp.Areas.Admin.Facade.DTO;
using CoachsBox.WebApp.Areas.Admin.Facade.Internal;
using CoachsBox.WebApp.Areas.Identity.Commands;
using CoachsBox.WebApp.Areas.Identity.Data;
using CoachsBox.WebApp.Resources;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Localization;

namespace CoachsBox.WebApp.Areas.Admin.Pages.Coaches
{
  public class DetailsModel : PageModel
  {
    private readonly ICoachRepository coachRepository;
    private readonly UserManager<CoachsBoxWebAppUser> userManager;
    private readonly IMediator mediator;
    private readonly IStringLocalizer<IdentityResource> stringLocalizer;

    public DetailsModel(
      ICoachRepository coachRepository,
      UserManager<CoachsBoxWebAppUser> userManager,
      IMediator mediator,
      IStringLocalizer<IdentityResource> stringLocalizer)
    {
      this.coachRepository = coachRepository;
      this.userManager = userManager;
      this.mediator = mediator;
      this.stringLocalizer = stringLocalizer;
    }

    public CoachDTO Coach { get; set; }

    public int PersonId { get; set; }

    public string AccountStatusStyle { get; set; }

    public string AccountStatus { get; set; }

    public string AccountStatusDetails { get; set; }

    public string AccountActionTitle { get; set; }

    public string AccountActionHandler { get; set; }

    public string AccountEmail { get; set; }

    public bool AccountEmailChangedFromCoach { get; set; }

    public async Task<IActionResult> OnGetAsync(int? coachId)
    {
      if (coachId == null)
        return NotFound();

      var coach = await this.coachRepository.GetByIdAsync(coachId.Value);

      if (coach == null)
        return NotFound();

      this.FillProperties(coach);

      return Page();
    }

    public async Task<IActionResult> OnPostInvite(int coachId, int personId)
    {
      var user = this.userManager.Users.SingleOrDefault(user => user.PersonId == personId);
      if (user == null)
      {
        var coach = await this.coachRepository.GetByIdAsync(coachId);
        if (coach == null)
          return this.NotFound();

        var addUserResult = await this.mediator.Send(new CreateCoachUserCommand(coach.Person.Email(), coach.Person.PhoneNumber(), coach.PersonId));
        if (addUserResult.IsSucceeded())
          await this.mediator.Send(new SendInvitationCommand(addUserResult.UserId));
        else
        {
          foreach (var addUserError in addUserResult.IdentityResult.Errors)
            this.ModelState.TryAddModelError(addUserError.Code, this.stringLocalizer.GetString(addUserError.Code));

          this.FillProperties(coach);
          return this.Page();
        }
      }
      else if (!user.EmailConfirmed)
        await this.mediator.Send(new SendInvitationCommand(user.Id));

      return this.RedirectToPage("Details", new { coachId });
    }

    public async Task<IActionResult> OnPostUpdateUserName(int coachId, string userName)
    {
      var coach = await this.coachRepository.GetByIdAsync(coachId);
      if (coach == null)
        return this.NotFound();

      var newEmail = coach.Person.Email();
      var updateResult = await this.mediator.Send(new UpdateUserNameAndEmailCommand(userName, newEmail, newEmail));
      if (updateResult.Succeeded)
        return this.RedirectToPage("Details", new { coachId });

      foreach (var updateError in updateResult.Errors)
        this.ModelState.TryAddModelError(updateError.Code, this.stringLocalizer.GetString(updateError.Code) ?? updateError.Description);

      this.FillProperties(coach);
      return this.Page();
    }

    public void FillProperties(Coach coach)
    {
      var assembler = new CoachDTOAssembler();
      this.Coach = assembler.ToDTO(coach);
      this.PersonId = coach.PersonId;

      var user = this.userManager.Users.SingleOrDefault(user => user.PersonId == coach.PersonId);
      if (user == null)
      {
        this.AccountStatus = "Приглашение не отправлено";
        this.AccountStatusStyle = "primary";
        this.AccountStatusDetails = "Тренер не получал приглашение на эл. почту.";
        this.AccountActionTitle = "Пригласить";
        this.AccountActionHandler = "Invite";
      }
      else
      {
        this.AccountEmail = user.Email.ToLower(CultureInfo.CurrentUICulture);
        this.AccountEmailChangedFromCoach = !string.Equals(user.UserName, coach.Person.Email(), StringComparison.OrdinalIgnoreCase);

        if (!user.EmailConfirmed)
        {
          this.AccountStatus = "Ожидание подтверждения";
          this.AccountStatusStyle = "warning";
          this.AccountStatusDetails = $"Тренер не подтвердил эл. почту '{this.AccountEmail}'.";
          this.AccountActionTitle = "Отправить приглашение";
          this.AccountActionHandler = "Invite";
        }
        else
        {
          this.AccountStatus = "Приглашён";
          this.AccountStatusStyle = "success";
          this.AccountStatusDetails = $"Тренер подтвердил эл. почту '{this.AccountEmail}' и может пользоваться системой.";
        }
      }
    }
  }
}
