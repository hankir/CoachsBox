using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoachsBox.Coaching.BranchModel;
using CoachsBox.WebApp.Pages.Facade.DTO;

namespace CoachsBox.WebApp.Pages.Facade.Internal.Assembler
{
  public class BranchRefDTOAssembler
  {
    public List<BranchRefDTO> ToDTOList(IEnumerable<Branch> branches)
    {
      return branches.Select(this.ToDTO).ToList();
    }

    public BranchRefDTO ToDTO(Branch branch)
    {
      return new BranchRefDTO(
          id: branch.Id,
          name: $"{branch.Address.State}, {branch.Address.City}".TrimEnd().TrimEnd(',')
          );
    }
  }
}
