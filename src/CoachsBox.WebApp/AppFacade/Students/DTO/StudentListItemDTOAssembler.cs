using System.Collections.Generic;
using System.Linq;
using CoachsBox.Coaching.StudentModel;
using CoachsBox.Core.Primitives;
using CoachsBox.WebApp.Areas.Admin.Facade.Internal;

namespace CoachsBox.WebApp.AppFacade.Students.DTO
{
  public class StudentListItemDTOAssembler
  {
    public IReadOnlyCollection<StudentListItemDTO> ToDTOList(
      IEnumerable<Student> students,
      PersonDTOAssembler personAssembler,
      IReadOnlyDictionary<int, Money> studentsBalances)
    {
      return students.Select(s =>
        new StudentListItemDTO()
        {
          Person = personAssembler.ToDTO(s.Person),
          StudentId = s.Id,
          IsTrialTraining = false,
          Balance = studentsBalances.TryGetValue(s.Id, out var balance) ? (decimal?)balance.Quantity : null
        })
        .ToList();
    }
  }
}
