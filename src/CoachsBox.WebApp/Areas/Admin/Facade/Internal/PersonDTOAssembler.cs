using System;
using System.Collections.Generic;
using System.Linq;
using CoachsBox.Coaching.PersonModel;
using CoachsBox.Core;
using CoachsBox.Core.Primitives;
using CoachsBox.WebApp.AppFacade.Primitives.DTO;
using CoachsBox.WebApp.Areas.Admin.Facade.DTO;

namespace CoachsBox.WebApp.Areas.Admin.Facade.Internal
{
  public class PersonDTOAssembler
  {
    public List<PersonDTO> ToDTOList(IEnumerable<Person> persons)
    {
      return persons.Select(this.ToDTO).ToList();
    }

    public PersonDTO ToDTO(Person person)
    {
      return new PersonDTO()
      {
        Id = person.Id,
        FullName = person.Name.FullName(),
        Birthday = person.Birthday.ToDateTime()?.ToLongDateString() ?? string.Empty,
        PhoneNumber = person.PhoneNumber(),
        Address = $"{person.City()}, {person.Street()}".TrimEnd().TrimEnd(','),
        Email = person.Email()
      };
    }
  }
}
