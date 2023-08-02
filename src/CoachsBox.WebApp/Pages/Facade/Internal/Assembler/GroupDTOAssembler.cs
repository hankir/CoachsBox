using System.Collections.Generic;
using System.Linq;
using CoachsBox.Coaching.GroupModel;
using CoachsBox.WebApp.Areas.Admin.Facade.DTO;
using CoachsBox.WebApp.Pages.Facade.DTO;

namespace CoachsBox.WebApp.Pages.Facade.Internal.Assembler
{
  public class GroupDTOAssembler
  {
    public List<GroupDTO> ToDTOList(IEnumerable<Group> groups)
    {
      return groups.Select(this.ToDTO).ToList();
    }

    public GroupDTO ToDTO(Group group)
    {
      return this.ToDTO(group, null, null);
    }

    public GroupDTO ToDTO(Group group, CoachDTO coachDTO)
    {
      return this.ToDTO(group, null, coachDTO);
    }

    public GroupDTO ToDTO(Group group, BranchRefDTO branchRef)
    {
      return this.ToDTO(group, branchRef, null);
    }

    public GroupDTO ToDTO(Group group, BranchRefDTO branchRef, CoachDTO coachDTO)
    {
      var result = new GroupDTO(
        id: group.Id,
        branch: branchRef,
        name: group.Name,
        sport: group.Sport.Name,
        minAge: group.TrainingProgramm.MinimumAge,
        maxAge: group.TrainingProgramm.MaximumAge)
      {
        Coach = coachDTO
      };

      foreach (var student in group.EnrolledStudents)
      {
        result.Students.Add(new GroupStudentDTO()
        {
          StudentId = student.StudentId
        });
      }

      return result;
    }
  }
}
