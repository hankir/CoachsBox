using System.Threading.Tasks;
using CoachsBox.Coaching.BranchModel;
using CoachsBox.WebApp.Areas.Admin.Facade.DTO;
using CoachsBox.WebApp.Areas.Admin.Facade.Internal;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CoachsBox.WebApp.Areas.Admin.Pages.Branches
{
  public class DetailsModel : PageModel
  {
    private readonly IBranchRepository branchRepository;

    public DetailsModel(IBranchRepository branchRepository)
    {
      this.branchRepository = branchRepository;
    }

    public BranchDTO Branch { get; set; }

    public async Task<IActionResult> OnGetAsync(int? branchId)
    {
      if (branchId == null)
        return NotFound();

      var assembler = new BranchDTOAssembler();
      var branch = await this.branchRepository.GetByIdAsync(branchId.Value);
      this.Branch = assembler.ToDTO(branch);

      if (Branch == null)
        return NotFound();

      return Page();
    }
  }
}
