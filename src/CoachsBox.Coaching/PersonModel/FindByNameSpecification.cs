using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using CoachsBox.Core;

namespace CoachsBox.Coaching.PersonModel
{
  public class FindByNameSpecification : BaseSpecification<Person>
  {
    public FindByNameSpecification(PersonName name)
      : base(person =>
          person.Name.Surname == name.Surname &&
          person.Name.Name == name.Name &&
          person.Name.Patronymic == name.Patronymic
      )
    {
    }
  }
}
