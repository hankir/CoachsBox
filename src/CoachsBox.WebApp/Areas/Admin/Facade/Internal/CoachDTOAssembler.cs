using System;
using System.Collections.Generic;
using System.Linq;
using CoachsBox.Coaching.CoachModel;
using CoachsBox.Core.Primitives;
using CoachsBox.WebApp.Areas.Admin.Facade.DTO;
using CoachsBox.WebApp.Extensions;

namespace CoachsBox.WebApp.Areas.Admin.Facade.Internal
{
  public class CoachDTOAssembler
  {
    public CoachDTO ToDTO(Coach coach)
    {
      var address = coach.Person.PersonalInformation != null ?
        $"{coach.Person.PersonalInformation.Address.City}, {coach.Person.PersonalInformation.Address.Street}".TrimEnd().TrimEnd(',') :
        null;
      var birthday = !coach.Person.Birthday.Equals(Date.Empty) ?
        new DateTime(coach.Person.Birthday.Year, coach.Person.Birthday.Month.Number, coach.Person.Birthday.Day).ToLongDateString() :
        null;

      return new CoachDTO()
      {
        Id = coach.Id,
        FullName = coach.Person.Name.FullName(),
        ShortName = coach.Person.Name.ShortName(),
        Birthday = birthday,
        PhoneNumber = coach.Person.PersonalInformation?.PhoneNumber.Value,
        Address = address,
        Email = coach.Person.PersonalInformation?.Email.Value
      };
    }

    public List<CoachDTO> ToDTOList(IEnumerable<Coach> coaches)
    {
      return coaches.Select(this.ToDTO).ToList();
    }
  }
}
