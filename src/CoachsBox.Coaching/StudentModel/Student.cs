using System;
using System.Collections.Generic;
using System.Linq;
using CoachsBox.Coaching.PersonModel;
using CoachsBox.Core;

namespace CoachsBox.Coaching.StudentModel
{
  /// <summary>
  /// Модель ученика.
  /// </summary>
  public class Student : BaseEntity
  {
    private readonly List<Relative> relatives = new List<Relative>();

    public Student(int personId, string note)
    {
      if (personId <= 0)
        throw new ArgumentException("Id is transient", nameof(personId));

      this.PersonId = personId;
      this.Note = note;

      this.AddDomainEvent(new CreatedStudentEvent(this));
    }

    public Student(Person person, string note)
    {
      this.Person = person;
      this.Note = note;

      this.AddDomainEvent(new CreatedStudentEvent(this));
    }

    public int PersonId { get; private set; }

    /// <summary>
    /// Получить человека зарегестрированного как студента.
    /// </summary>
    public Person Person { get; private set; }

    /// <summary>
    /// Получить родственников студента.
    /// </summary>
    public IReadOnlyCollection<Relative> Relatives => this.relatives;

    /// <summary>
    /// Получить или установить примечание для ученика.
    /// </summary>
    public string Note { get; private set; }

    /// <summary>
    /// Добавить родственника студента.
    /// </summary>
    /// <param name="relativePerson">Персона связанная с родственником.</param>
    /// <param name="relationship">Родство.</param>
    public void AddRelative(Person relativePerson, Relationship relationship)
    {
      var relative = new Relative(relativePerson, relationship);
      if (this.relatives.Any(r => r.Equals(relative)))
        throw new InvalidOperationException($"Relative already exists. Relative person id: {relativePerson.Id}, relationship: {relationship.Name}");

      var relativeByPersonId = this.relatives.SingleOrDefault(r => r.PersonId == relativePerson.Id);
      if (relativeByPersonId != null)
        throw new InvalidOperationException(
          $"Relative already exists. Relative person id: {relativePerson.Id}, relationship: {relativeByPersonId.Relationship.Name} but added {relationship.Name}");

      if (Relationship.Mother.Equals(relationship) && this.relatives.Any(r => r.Equals(Relationship.Mother)))
        throw new InvalidOperationException($"Not happen two moms");

      if (Relationship.Father.Equals(relationship) && this.relatives.Any(r => r.Equals(Relationship.Father)))
        throw new InvalidOperationException($"Not happen two dads");

      this.relatives.Add(relative);
    }

    public void UpdateRelative(Relative relative)
    {
      var existRelative = this.relatives.SingleOrDefault(r => r.PersonId == relative.PersonId);
      if (existRelative == null)
        throw new InvalidOperationException($"Relative not exists. Relative person id: {relative.PersonId}");

      var index = this.relatives.IndexOf(existRelative);
      this.relatives.Remove(existRelative);
      this.relatives.Insert(index, relative);
    }

    /// <summary>
    /// Изменить примечание студента.
    /// </summary>
    /// <param name="note">Примечание студента.</param>
    public void ChangeNote(string note)
    {
      this.Note = note;
    }

    private Student()
    {
      // Требует Entity framework core
    }
  }
}
