using System;
using System.Collections.Generic;
using CoachsBox.Coaching.CoachModel;
using CoachsBox.Coaching.PersonModel;
using CoachsBox.Core;
using CoachsBox.Core.Primitives;

namespace CoachsBox.Coaching.BranchModel
{
  /// <summary>
  /// Филиал школы.
  /// </summary>
  public class Branch : BaseEntity
  {
    private readonly List<CoachingStaffMember> coachingStaff = new List<CoachingStaffMember>();

    public Branch(Address adress, int contactPersonId)
    {
      if (contactPersonId <= 0)
        throw new ArgumentException("Id is transient", nameof(contactPersonId));

      this.Address = adress;
      this.ContactPersonId = contactPersonId;
    }

    public Branch(Address adress, Person contactPerson)
    {
      this.Address = adress;
      this.ContactPerson = contactPerson;
    }

    /// <summary>
    /// Получить адрес филиала.
    /// </summary>
    public Address Address { get; private set; }

    /// <summary>
    /// Получить идентификатор контактного лица.
    /// </summary>
    public int ContactPersonId { get; private set; }

    /// <summary>
    /// Получить контактное лицо филиала.
    /// </summary>
    public Person ContactPerson { get; private set; }

    /// <summary>
    /// Получить тренерский состав филиала.
    /// </summary>
    public IReadOnlyCollection<CoachingStaffMember> CoachingStaff => this.coachingStaff;

    /// <summary>
    /// Включить тренера в штаб филиала.
    /// </summary>
    /// <param name="coachId">Идентификатор тренера.</param>
    public void IncludeCoach(int coachId)
    {
      var coachingStaffMember = new CoachingStaffMember(coachId);
      if (!this.coachingStaff.Contains(coachingStaffMember))
        this.coachingStaff.Add(coachingStaffMember);
    }

    public void IncludeCoach(Coach coach)
    {
      var coachingStaffMember = new CoachingStaffMember(coach);
      if (coach.IsTransient() || !this.coachingStaff.Contains(coachingStaffMember))
        this.coachingStaff.Add(coachingStaffMember);
    }

    /// <summary>
    /// Исключить тренера из штаба филиала.
    /// </summary>
    /// <param name="coachId">Идентификатор тренера.</param>
    public void ExcludeCoach(int coachId)
    {
      var coachingStaffMember = new CoachingStaffMember(coachId);
      if (!this.coachingStaff.Contains(coachingStaffMember))
        throw new System.ArgumentException($"Coach (id: {coachId}) not a member of a branch coaching staff");
      this.coachingStaff.Remove(coachingStaffMember);
    }

    /// <summary>
    /// Назначить контактное лицо.
    /// </summary>
    /// <param name="contactPerson">Контактное лицо.</param>
    public void AssignContactPerson(Person contactPerson)
    {
      this.ContactPersonId = contactPerson.Id;
      this.ContactPerson = contactPerson;
    }

    public void CorrectAddress(Address address)
    {
      this.Address = address;
    }

    private Branch()
    {
      // Требует Entity framework core
    }
  }
}
