using System;
using CoachsBox.Coaching.CoachModel;
using CoachsBox.Core.Primitives;
using CoachsBox.WebApp.AppFacade.Primitives.DTO;

namespace CoachsBox.WebApp.AppFacade.Coaches.Commands
{
  public class InputCoachCommand : CreateCoachCommand
  {
    public InputCoachCommand(Coach coach)
    {
      this.Id = coach.Id;

      var personName = coach.Person.Name;
      this.Surname = personName.Surname;
      this.Name = personName.Name;
      this.Patronymic = personName.Patronymic;

      this.Gender = coach.Person.Gender.Value;
      this.Birthdate = !coach.Person.Birthday.Equals(Date.Empty) ?
        (DateTime?)new DateTime(coach.Person.Birthday.Year, coach.Person.Birthday.Month.Number, coach.Person.Birthday.Day) :
        null;

      var address = new AddressDTO();
      var personalInfo = coach.Person.PersonalInformation;
      if (personalInfo != null)
      {
        this.PhoneNumber = personalInfo.PhoneNumber.Value;
        this.Email = personalInfo.Email.Value;

        address.City = personalInfo.Address.City;
        address.Country = personalInfo.Address.Country;
        address.State = personalInfo.Address.State;
        address.Street = personalInfo.Address.Street;
        address.ZipCode = personalInfo.Address.ZipCode;
      }

      this.Address = address;
    }

    public int Id { get; set; }

    public InputCoachCommand()
    {
      // Требует Asp.Net Razor pages.
    }
  }
}
