using System;
using System.Collections.Generic;
using CoachsBox.Coaching.PersonModel;
using CoachsBox.Core;

namespace CoachsBox.Coaching.StudentModel
{
  /// <summary>
  /// Родственник.
  /// </summary>
  public class Relative : ValueObject
  {
    public Relative(int personId, Relationship relationship)
    {
      this.PersonId = personId;
      this.Relationship = relationship;
    }

    public Relative(Person person, Relationship relationship)
    {
      if (person == null)
        throw new ArgumentNullException(nameof(person));

      this.PersonId = person.Id;
      this.Relationship = relationship;
      this.Person = person;
    }

    /// <summary>
    /// Получить идентификатор персоны родственника.
    /// </summary>
    public int PersonId { get; private set; }

    /// <summary>
    /// Получить информацию о персоне родственника.
    /// </summary>
    public Person Person { get; private set; }

    /// <summary>
    /// Получить признак родства.
    /// </summary>
    public Relationship Relationship { get; private set; }

    protected override IEnumerable<object> GetAtomicValues()
    {
      yield return this.PersonId;
      yield return this.Relationship;
    }

    private Relative()
    {
      // Требует Entity framework core
    }
  }
}
