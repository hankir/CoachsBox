using System.Collections.Generic;
using System.Linq;
using CoachsBox.Coaching.PersonModel;
using CoachsBox.Core;

namespace CoachsBox.Coaching.StudentModel
{
  public class FindStudentsByNameSpecification : StudentListSpecification
  {
    public FindStudentsByNameSpecification(PersonName personName)
      : base((student) =>
      (personName.Surname != null && (student.Person.Name.Surname.ToLower().Contains(personName.Surname.ToLower())) || string.IsNullOrWhiteSpace(personName.Surname)) &&
      ((personName.Name != null && student.Person.Name.Name.ToLower().Contains(personName.Name.ToLower())) || string.IsNullOrWhiteSpace(personName.Name)) &&
      ((personName.Patronymic != null && student.Person.Name.Patronymic.ToLower().Contains(personName.Patronymic.ToLower())) || string.IsNullOrWhiteSpace(personName.Patronymic)))
    {
    }

    public FindStudentsByNameSpecification(PersonName personName, IEnumerable<int> studentIds)
      : base((student) =>
      studentIds.Contains(student.Id) &&
      (personName.Surname != null && (student.Person.Name.Surname.ToLower().Contains(personName.Surname.ToLower())) || string.IsNullOrWhiteSpace(personName.Surname)) &&
      ((personName.Name != null && student.Person.Name.Name.ToLower().Contains(personName.Name.ToLower())) || string.IsNullOrWhiteSpace(personName.Name)) &&
      ((personName.Patronymic != null && student.Person.Name.Patronymic.ToLower().Contains(personName.Patronymic.ToLower())) || string.IsNullOrWhiteSpace(personName.Patronymic)))
    {
    }

    public FindStudentsByNameSpecification(string surname, string name, string patronymic)
      : base((student) =>
          (!string.IsNullOrWhiteSpace(surname) && student.Person.Name.Surname.ToLower().Contains(surname.ToLower())) ||
          (!string.IsNullOrWhiteSpace(name) && student.Person.Name.Name.ToLower().Contains(name.ToLower())) ||
          (!string.IsNullOrWhiteSpace(patronymic) && student.Person.Name.Patronymic.ToLower().Contains(patronymic.ToLower())) ||

          (!string.IsNullOrWhiteSpace(patronymic) && student.Person.Name.Surname.ToLower().Contains(patronymic.ToLower())) ||
          (!string.IsNullOrWhiteSpace(surname) && student.Person.Name.Name.ToLower().Contains(surname.ToLower())) ||
          (!string.IsNullOrWhiteSpace(name) && student.Person.Name.Patronymic.ToLower().Contains(name.ToLower())) ||

          (!string.IsNullOrWhiteSpace(name) && student.Person.Name.Surname.ToLower().Contains(name.ToLower())) ||
          (!string.IsNullOrWhiteSpace(patronymic) && student.Person.Name.Name.ToLower().Contains(patronymic.ToLower())) ||
          (!string.IsNullOrWhiteSpace(surname) && student.Person.Name.Patronymic.ToLower().Contains(surname.ToLower()))
      )
    {
      this.AddInclude(student => student.Person);
    }

    public FindStudentsByNameSpecification(string surname, string name, string patronymic, IEnumerable<int> studentIds)
      : base((student) =>
          studentIds.Contains(student.Id) &&
          ((!string.IsNullOrWhiteSpace(surname) && student.Person.Name.Surname.ToLower().Contains(surname.ToLower())) ||
          (!string.IsNullOrWhiteSpace(name) && student.Person.Name.Name.ToLower().Contains(name.ToLower())) ||
          (!string.IsNullOrWhiteSpace(patronymic) && student.Person.Name.Patronymic.ToLower().Contains(patronymic.ToLower())) ||

          (!string.IsNullOrWhiteSpace(patronymic) && student.Person.Name.Surname.ToLower().Contains(patronymic.ToLower())) ||
          (!string.IsNullOrWhiteSpace(surname) && student.Person.Name.Name.ToLower().Contains(surname.ToLower())) ||
          (!string.IsNullOrWhiteSpace(name) && student.Person.Name.Patronymic.ToLower().Contains(name.ToLower())) ||

          (!string.IsNullOrWhiteSpace(name) && student.Person.Name.Surname.ToLower().Contains(name.ToLower())) ||
          (!string.IsNullOrWhiteSpace(patronymic) && student.Person.Name.Name.ToLower().Contains(patronymic.ToLower())) ||
          (!string.IsNullOrWhiteSpace(surname) && student.Person.Name.Patronymic.ToLower().Contains(surname.ToLower())))
      )
    {
      this.AddInclude(student => student.Person);
    }
  }
}
