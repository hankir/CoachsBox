using System.Linq;
using CoachsBox.Coaching.PersonModel;
using CoachsBox.WebApp.AppFacade.Coaches.Commands;
using CoachsBox.WebApp.Areas.Admin.Facade;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CoachsBox.WebApp.Areas.Admin.Pages.Coaches
{
  public class CreateModel : PageModel
  {
    private readonly IAdministrationServiceFacade administrativeServiceFacade;

    public CreateModel(
      IAdministrationServiceFacade administrativeServiceFacade)
    {
      this.administrativeServiceFacade = administrativeServiceFacade;
    }

    [BindProperty]
    public CreateCoachCommand CreateCommand { get; set; }

    public IActionResult OnGet(int? branchId)
    {
      this.CreateCommand = new CreateCoachCommand()
      {
        Gender = Gender.Male.Value
      };

      if (branchId != null)
        this.CreateCommand.BranchId = branchId.Value;

      return Page();
    }

    public IActionResult OnPost()
    {
      if (!ModelState.IsValid)
        return Page();

      var existingPersons = this.administrativeServiceFacade.FindExistCoach(this.CreateCommand);
      if (existingPersons.Any())
      {
        // TODO: Список похожих персон.
        this.ModelState.AddModelError("CreateCommand.Name", "Тренер(ы) с похожим именем уже существует(ют)");
        return this.Page();
      }

      var coachId = this.administrativeServiceFacade.CreateCoach(this.CreateCommand);
      return RedirectToPage("./Details", new { coachId });
    }
  }
}