using System;
using CoachsBox.Coaching.StudentModel;
using CoachsBox.Core.Primitives;
using CoachsBox.WebApp.AppFacade.Primitives.DTO;

namespace CoachsBox.WebApp.AppFacade.Students.Commands
{
  public class UpdateStudentCommand : CreateStudentCommand
  {
    public UpdateStudentCommand(Student student)
    {
      this.Id = student.Id;

      var personName = student.Person.Name;
      this.Surname = personName.Surname;
      this.Name = personName.Name;
      this.Patronymic = personName.Patronymic;

      this.Gender = student.Person.Gender.Value;
      this.Birthdate = !student.Person.Birthday.Equals(Date.Empty) ?
        (DateTime?)new DateTime(student.Person.Birthday.Year, student.Person.Birthday.Month.Number, student.Person.Birthday.Day) :
        null;

      var address = new AddressDTO();
      var personalInfo = student.Person.PersonalInformation;
      if (personalInfo != null)
      {
        this.PhoneNumber = personalInfo.PhoneNumber?.Value;
        this.Email = personalInfo.Email?.Value;

        address.City = personalInfo.Address?.City;
        address.Country = personalInfo.Address?.Country;
        address.State = personalInfo.Address?.State;
        address.Street = personalInfo.Address?.Street;
        address.ZipCode = personalInfo.Address?.ZipCode;
      }

      this.Address = address;
    }

    public int Id { get; set; }

    public UpdateStudentCommand()
    {
      // Требует Asp.Net Razor pages.
    }
  }
}
