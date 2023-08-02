using System;
using System.Collections.Generic;
using System.Linq;
using CoachsBox.Core;

namespace CoachsBox.Coaching.PersonModel
{
  public class PersonByIdSpecification : BaseSpecification<Person>
  {
    public PersonByIdSpecification(int personId)
      : base(person => person.Id == personId)
    {
    }

    public PersonByIdSpecification(IEnumerable<int> personIds)
      : base(person => personIds.Contains(person.Id))
    {
    }
  }
}
