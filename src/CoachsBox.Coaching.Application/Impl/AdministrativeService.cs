using System;
using System.Linq;
using CoachsBox.Coaching.PersonModel;
using CoachsBox.Coaching.StudentModel;
using CoachsBox.Core;
using CoachsBox.Core.Interfaces;
using CoachsBox.Core.Primitives;

namespace CoachsBox.Coaching.Application.Impl
{
  public class AdministrativeService : IAdministrativeService
  {
    private readonly IUnitOfWork unitOfWork;
    private readonly IStudentRepository studentRepository;
    private readonly IPersonRepository personRepository;

    public AdministrativeService(
      IUnitOfWork unitOfWork,
      IStudentRepository studentRepository,
      IPersonRepository personRepository)
    {
      this.unitOfWork = unitOfWork;
      this.studentRepository = studentRepository;
      this.personRepository = personRepository;
    }

    public void AddStudentRelative(int studentId, PersonName relativeName, Date birthday, PersonalInformation relativeInfo, Relationship relationship)
    {
      var student = this.studentRepository.GetByIdAsync(studentId).Result;
      if (student == null)
        throw new InvalidOperationException($"Student (Id: {studentId}) not found");

      var gender = relationship.Equals(Relationship.Mother) || relationship.Equals(Relationship.Grandmother) ?
        Gender.Female :
        Gender.Male;

      var person = new Person(relativeName, gender, birthday);
      person.AssignPersonalInformation(relativeInfo);
      this.personRepository.AddAsync(person).Wait();

      student.AddRelative(person, relationship);
      this.unitOfWork.Save();
    }

    public void UpdateStudentRelative(int studentId, int personId, PersonName relativeName, Date birthday, PersonalInformation relativeInfo, Relationship relationship)
    {
      var person = this.personRepository.GetByIdAsync(personId).Result;
      if (!Equals(person.Name, relativeName))
        person.CorrectName(relativeName);

      var gender = relationship.Equals(Relationship.Mother) || relationship.Equals(Relationship.Grandmother) ?
        Gender.Female :
        Gender.Male;
      if (!Equals(person.Gender, gender))
        person.CorrectGender(gender);

      if (!Equals(person.Birthday, birthday))
        person.CorrectBirthday(birthday);

      if (!Equals(person.PersonalInformation, relativeInfo))
        person.AssignPersonalInformation(relativeInfo);

      var updatedRelative = new Relative(person, relationship);
      var student = this.studentRepository.GetByIdAsync(studentId).Result;
      student.UpdateRelative(updatedRelative);

      this.unitOfWork.Save();
    }
  }
}
