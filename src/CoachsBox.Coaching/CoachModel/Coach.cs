using System;
using CoachsBox.Coaching.PersonModel;
using CoachsBox.Core;

namespace CoachsBox.Coaching.CoachModel
{
  /// <summary>
  /// Модель тренера.
  /// </summary>
  public class Coach : BaseEntity
  {
    public Coach(int personId)
    {
      if (personId <= 0)
        throw new ArgumentException("Id is transient", nameof(personId));

      this.PersonId = personId;
      this.AddDomainEvent(new CreatedCoachEvent(this));
    }

    public Coach(Person person)
    {
      this.Person = person;
      this.PersonId = person.Id;
      this.AddDomainEvent(new CreatedCoachEvent(this));
    }

    public int PersonId { get; private set; }

    /// <summary>
    /// Получить персону тренера.
    /// </summary>
    public Person Person { get; private set; }

    private Coach()
    {
      // Требует Entity framework core
    }
  }
}
