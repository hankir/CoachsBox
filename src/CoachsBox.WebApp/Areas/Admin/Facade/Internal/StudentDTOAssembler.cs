using System;
using System.Collections.Generic;
using System.Linq;
using CoachsBox.Coaching.StudentModel;
using CoachsBox.WebApp.Areas.Admin.Facade.DTO;

namespace CoachsBox.WebApp.Areas.Admin.Facade.Internal
{
  public class StudentDTOAssembler
  {
    public List<StudentDTO> ToDTOList(IEnumerable<Student> students)
    {
      return students.Select(this.ToDTO).ToList();
    }

    public StudentDTO ToDTO(Student student)
    {
      var person = student.Person;
      return new StudentDTO()
      {
        Id = student.Id,
        Address = person?.PersonalInformation != null ? $"{person.PersonalInformation.Address.City}, {person.PersonalInformation.Address.Street}".TrimEnd().TrimEnd(',') : null,
        Birthday = person != null ? new DateTime(person.Birthday.Year, person.Birthday.Month.Number, person.Birthday.Day).ToLongDateString() : null,
        Email = person?.PersonalInformation?.Email?.Value,
        FullName = person?.Name.FullName(),
        PhoneNumber = person?.PersonalInformation?.PhoneNumber?.Value
      };
    }
  }
}
