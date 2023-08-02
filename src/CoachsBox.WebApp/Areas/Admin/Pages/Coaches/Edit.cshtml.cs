using System.Threading.Tasks;
using CoachsBox.Coaching.CoachModel;
using CoachsBox.WebApp.AppFacade.Coaches.Commands;
using CoachsBox.WebApp.Areas.Admin.Facade;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CoachsBox.WebApp.Areas.Admin.Pages.Coaches
{
  public class EditModel : PageModel
  {
    private readonly IAdministrationServiceFacade administrationService;
    private readonly ICoachRepository coachRepository;

    public EditModel(
      IAdministrationServiceFacade administrationService,
      ICoachRepository coachRepository)
    {
      this.administrationService = administrationService;
      this.coachRepository = coachRepository;
    }

    [BindProperty]
    public InputCoachCommand InputCommand { get; set; }

    public async Task<IActionResult> OnGetAsync(int? coachId)
    {
      if (coachId == null)
        return this.NotFound();

      var coach = await this.coachRepository.GetByIdAsync(coachId.Value);
      if (coach == null)
        return this.NotFound();

      this.InputCommand = new InputCoachCommand(coach);

      return this.Page();
    }

    public IActionResult OnPost()
    {
      if (!ModelState.IsValid)
        return this.Page();

      this.administrationService.UpdateCoach(this.InputCommand);

      return this.RedirectToPage("./Index");
    }
  }
}
