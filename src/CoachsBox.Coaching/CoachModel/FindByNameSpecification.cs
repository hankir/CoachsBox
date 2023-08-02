using CoachsBox.Core;

namespace CoachsBox.Coaching.CoachModel
{
  public class FindByNameSpecification : BaseSpecification<Coach>
  {
    public FindByNameSpecification(string surname, string name, string patronymic)
      : base(coach =>
          coach.Person.Name.Surname == surname &&
          coach.Person.Name.Name == name &&
          coach.Person.Name.Patronymic == patronymic
      )
    {
    }
  }
}
