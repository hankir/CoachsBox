using System.Threading.Tasks;
using CoachsBox.WebApp.Areas.Admin.Facade;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CoachsBox.WebApp.Areas.Admin.Pages.Branches
{
  public class CreateModel : PageModel
  {
    private readonly IAdministrationServiceFacade administrationService;

    public CreateModel(
      IAdministrationServiceFacade administrationService)
    {
      this.administrationService = administrationService;
    }

    public async Task<IActionResult> OnGet()
    {
      this.CreateCommand = new CreateBranchCommand();
      return Page();
    }

    [BindProperty]
    public CreateBranchCommand CreateCommand { get; set; }

    public IActionResult OnPost()
    {
      if (!this.ModelState.IsValid)
      {
        return this.Page();
      }

      var branchId = this.administrationService.CreateBranch(this.CreateCommand);
      return RedirectToPage("./Details", new { branchId });
    }
  }
}