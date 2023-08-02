using System.Collections.Generic;
using System.Linq;
using CoachsBox.Coaching.StudentModel;
using CoachsBox.WebApp.Areas.Admin.Facade.Internal;
using CoachsBox.WebApp.Pages.Facade.DTO;

namespace CoachsBox.WebApp.Pages.Facade.Internal.Assembler
{
  public class GroupStudentDTOAssembler
  {
    public IReadOnlyCollection<GroupStudentDTO> ToDTOList(IEnumerable<Student> students, PersonDTOAssembler personAssembler)
    {
      return students.Select(s =>
        new GroupStudentDTO()
        {
          Person = personAssembler.ToDTO(s.Person),
          StudentId = s.Id,
          IsTrialTraining = false
        })
        .ToList();
    }
  }
}
