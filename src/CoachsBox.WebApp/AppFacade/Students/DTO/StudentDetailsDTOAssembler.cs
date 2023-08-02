using System;
using CoachsBox.Coaching.StudentModel;
using CoachsBox.Core.Primitives;

namespace CoachsBox.WebApp.AppFacade.Students.DTO
{
  public class StudentDetailsDTOAssembler
  {
    public StudentDetailsDTO ToDTO(Student student)
    {
      var person = student.Person;
      var result = new StudentDetailsDTO()
      {
        StudentId = student.Id,
        FullName = person?.Name.FullName(),
        PhoneNumber = person?.PersonalInformation?.PhoneNumber?.Value,
        Email = person?.PersonalInformation?.Email?.Value,
        Address = person?.PersonalInformation?.Address != null ? $"{person.PersonalInformation.Address.City}, {person.PersonalInformation.Address.Street}".TrimEnd().TrimEnd(',') : null,
        Birthday = person != null && !person.Birthday.Equals(Date.Empty) ? new DateTime(person.Birthday.Year, person.Birthday.Month.Number, person.Birthday.Day).ToLongDateString() : null,
        Note = student.Note
      };
      foreach (var relative in student.Relatives)
      {
        result.Relatives.Add(new StudentDetailsRelativeDTO()
        {
          PersonId = relative.PersonId,
          RelationshipName = relative.Relationship.GetLocalization(),
          Phone = relative.Person?.PersonalInformation?.PhoneNumber?.Value,
          PersonFullName = relative.Person?.Name?.FullName(),
          Email = relative.Person?.PersonalInformation?.Email?.Value
        });
      }
      return result;
    }
  }
}
