using System.Collections.Generic;
using System.Linq;
using CoachsBox.Coaching.BranchModel;
using CoachsBox.WebApp.Areas.Admin.Facade.DTO;

namespace CoachsBox.WebApp.Areas.Admin.Facade.Internal
{
  public class BranchDTOAssembler
  {
    public BranchDTO ToDTO(Branch branch)
    {
      return new BranchDTO(
        id: branch.Id,
        state: branch.Address?.State,
        city: $"{branch.Address?.City}",
        street: branch.Address?.Street,
        contactPersonId: branch.ContactPersonId,
        contactPersonFullName: branch.ContactPerson?.Name.FullName(),
        address: $"{branch.Address?.City}, {branch.Address?.Street}".TrimEnd().TrimEnd(','),
        phoneNumber: branch.ContactPerson.PersonalInformation?.PhoneNumber?.Value);
    }

    public List<BranchDTO> ToDTOList(IEnumerable<Branch> branches)
    {
      return branches.Select(this.ToDTO).ToList();
    }
  }
}
