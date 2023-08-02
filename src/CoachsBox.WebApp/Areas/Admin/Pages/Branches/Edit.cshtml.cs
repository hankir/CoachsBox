using CoachsBox.WebApp.Areas.Admin.Facade;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CoachsBox.WebApp.Areas.Admin.Pages.Branches
{
  public class EditModel : PageModel
  {
    private readonly IAdministrationServiceFacade administrationService;

    public EditModel(IAdministrationServiceFacade administrationService)
    {
      this.administrationService = administrationService;
    }

    [BindProperty]
    public CreateBranchCommand EditCommand { get; set; }

    public IActionResult OnGetAsync(int branchId)
    {
      if (branchId <= 0)
      {
        return NotFound();
      }

      this.EditCommand = this.administrationService.EditBranch(branchId);

      return Page();
    }

    public IActionResult OnPost(int branchId)
    {
      if (branchId <= 0)
      {
        return NotFound();
      }

      if (!ModelState.IsValid)
      {
        return Page();
      }

      this.administrationService.UpdateBranch(branchId, this.EditCommand);

      return RedirectToPage("./Index");
    }
  }
}
