using System.Collections.Generic;
using System.Threading.Tasks;
using CoachsBox.Coaching.BranchModel;
using CoachsBox.WebApp.Areas.Admin.Facade.DTO;
using CoachsBox.WebApp.Areas.Admin.Facade.Internal;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CoachsBox.WebApp.Areas.Admin.Pages.Branches
{
  public class IndexModel : PageModel
  {
    private readonly IBranchRepository branchRepository;

    public IndexModel(IBranchRepository branchRepository)
    {
      this.branchRepository = branchRepository;
    }

    public IList<BranchDTO> Branches { get; set; }

    public async Task OnGetAsync()
    {
      var branches = await this.branchRepository.ListAllAsync();
      var assembler = new BranchDTOAssembler();
      this.Branches = assembler.ToDTOList(branches);
    }
  }
}
