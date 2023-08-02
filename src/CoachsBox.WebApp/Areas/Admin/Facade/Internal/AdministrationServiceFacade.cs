using System;
using System.Collections.Generic;
using System.Linq;
using CoachsBox.Coaching.Application;
using CoachsBox.Coaching.BranchModel;
using CoachsBox.Coaching.CoachModel;
using CoachsBox.Coaching.PersonModel;
using CoachsBox.Coaching.StudentModel;
using CoachsBox.Core;
using CoachsBox.Core.Interfaces;
using CoachsBox.Core.Primitives;
using CoachsBox.WebApp.AppFacade.Coaches.Commands;
using CoachsBox.WebApp.AppFacade.Primitives.Commands;
using CoachsBox.WebApp.AppFacade.Primitives.DTO;
using CoachsBox.WebApp.AppFacade.Students.Commands;
using CoachsBox.WebApp.AppFacade.Students.DTO;
using CoachsBox.WebApp.Areas.Admin.Facade.DTO;

namespace CoachsBox.WebApp.Areas.Admin.Facade.Internal
{
  public class AdministrationServiceFacade : IAdministrationServiceFacade
  {
    private readonly IUnitOfWork unitOfWork;
    private readonly IStudentRepository studentRepository;
    private readonly ICoachRepository coachRepository;
    private readonly IPersonRepository personRepository;
    private readonly IBranchRepository branchRepository;
    private readonly IAdministrativeService administrativeService;

    public AdministrationServiceFacade(
      IUnitOfWork unitOfWork,
      IStudentRepository studentRepository,
      ICoachRepository coachRepository,
      IPersonRepository personRepository,
      IBranchRepository branchRepository,
      IAdministrativeService administrativeService)
    {
      this.unitOfWork = unitOfWork;
      this.studentRepository = studentRepository;
      this.coachRepository = coachRepository;
      this.personRepository = personRepository;
      this.branchRepository = branchRepository;
      this.administrativeService = administrativeService;
    }

    public IReadOnlyCollection<CoachDTO> ListCoachesForBranch(int branchId)
    {
      // TODO: Сделать фильтр по тренерскому составу филиала.
      var coaches = this.coachRepository.ListAllAsync().Result;
      var assembler = new CoachDTOAssembler();
      return assembler.ToDTOList(coaches);
    }

    public int CreateCoach(CreateCoachCommand command)
    {
      var personName = this.GetNameFromCommand(command);
      var birthDate = command.Birthdate != null ? Date.Create(command.Birthdate.Value) : Date.Empty;

      var gender = Gender.GetAll<Gender>().Where(g => g.Value == command.Gender).Single();
      var person = new Person(personName, gender, birthDate);

      var address = new Address(
        command.Address.Street,
        command.Address.City,
        command.Address.State,
        command.Address.Country,
        command.Address.ZipCode);

      var personalInformation = new PersonalInformation(new PhoneNumber(command.PhoneNumber), new EmailAddress(command.Email), address);
      person.AssignPersonalInformation(personalInformation);

      var coach = new Coach(person);

      this.personRepository.AddAsync(person).Wait();
      this.coachRepository.AddAsync(coach).Wait();

      this.unitOfWork.Save();

      return coach.Id;
    }

    public int CreateStudent(CreateStudentCommand command)
    {
      var personName = this.GetNameFromCommand(command);
      var birthDate = command.Birthdate != null ? Date.Create(command.Birthdate.Value) : Date.Empty;

      var gender = Gender.GetAll<Gender>().Where(g => g.Value == command.Gender).Single();
      var person = new Person(personName, gender, birthDate);

      var address = new Address(
        command.Address.Street,
        command.Address.City,
        command.Address.State,
        command.Address.Country,
        command.Address.ZipCode);

      var personalInformation = new PersonalInformation(new PhoneNumber(command.PhoneNumber), new EmailAddress(command.Email), address);
      person.AssignPersonalInformation(personalInformation);

      var student = new Student(person, string.Empty);

      this.personRepository.AddAsync(person).Wait();
      this.studentRepository.AddAsync(student).Wait();

      this.unitOfWork.Save();
      return student.Id;
    }

    public IReadOnlyCollection<CoachDTO> FindExistCoach(CreatePersonCommandBase command)
    {
      var findByNameSpecification = new Coaching.CoachModel.FindByNameSpecification(command.Surname, command.Surname, command.Patronymic);
      var existingCoaches = this.coachRepository.ListAsync(findByNameSpecification).Result;
      var assembler = new CoachDTOAssembler();
      return assembler.ToDTOList(existingCoaches);
    }

    public IReadOnlyCollection<StudentDTO> FindExistStudent(CreatePersonCommandBase command)
    {
      var personName = this.GetNameFromCommand(command);
      var findByNameSpecification = new FindStudentsByNameSpecification(personName);
      var existingStudents = this.studentRepository.ListAsync(findByNameSpecification).Result;
      var assembler = new StudentDTOAssembler();
      return assembler.ToDTOList(existingStudents);
    }

    private PersonName GetNameFromCommand(CreatePersonCommandBase command)
    {
      var surname = command.Surname;
      var name = command.Name;
      var patronymic = command.Patronymic;
      return new PersonName(surname, name, patronymic);
    }

    public IReadOnlyCollection<PersonDTO> ListContactPersonsForBranch()
    {
      var persons = this.branchRepository.ListPersonForContact().Result;
      var assembler = new PersonDTOAssembler();
      return assembler.ToDTOList(persons);
    }

    public int CreateBranch(CreateBranchCommand createCommand)
    {
      var address = new Address(
        createCommand.Street,
        createCommand.City,
        createCommand.State,
        createCommand.Country,
        createCommand.ZipCode);

      var branch = new Branch(address, createCommand.ContactPersonId);
      this.branchRepository.AddAsync(branch).Wait();

      this.unitOfWork.Save();
      return branch.Id;
    }

    public int CreatePerson(CreatePersonCommandBase command)
    {
      var personName = this.GetNameFromCommand(command);
      var person = new Person(personName);

      var phone = new PhoneNumber(command.PhoneNumber);
      var email = new EmailAddress(command.Email);
      var personalInfo = new PersonalInformation(phone, email);
      person.AssignPersonalInformation(personalInfo);

      this.personRepository.AddAsync(person).Wait();
      this.unitOfWork.Save();
      return person.Id;
    }

    public void UpdateCoach(InputCoachCommand command)
    {
      var coach = this.coachRepository.GetByIdAsync(command.Id).Result;
      var person = this.personRepository.GetByIdAsync(coach.PersonId).Result;

      var personName = this.GetNameFromCommand(command);
      if (!person.Name.Equals(personName))
        person.CorrectName(personName);

      var gender = Gender.Create(command.Gender);
      if (person.Gender != gender)
        person.CorrectGender(gender);

      var birthday = command.Birthdate != null ? Date.Create(command.Birthdate.Value) : Date.Empty;
      if (person.Birthday != birthday)
        person.CorrectBirthday(birthday);

      var address = new Address(
        command.Address.Street,
        command.Address.City,
        command.Address.State,
        command.Address.Country,
        command.Address.ZipCode);

      var phone = new PhoneNumber(command.PhoneNumber);
      var email = new EmailAddress(command.Email);
      var personalInfo = new PersonalInformation(phone, email, address);

      if (person.PersonalInformation == null || !person.PersonalInformation.Equals(personalInfo))
        person.AssignPersonalInformation(personalInfo);

      // Пока нет свойств в самом Coach.
      this.coachRepository.UpdateAsync(coach).Wait();
      this.personRepository.UpdateAsync(person).Wait();

      this.unitOfWork.Save();
    }

    public void UpdateStudent(UpdateStudentCommand command)
    {
      var student = this.studentRepository.GetByIdAsync(command.Id).Result;
      var person = this.personRepository.GetByIdAsync(student.PersonId).Result;

      var personName = this.GetNameFromCommand(command);
      if (!person.Name.Equals(personName))
        person.CorrectName(personName);

      var gender = Gender.Create(command.Gender);
      if (person.Gender != gender)
        person.CorrectGender(gender);

      var birthday = command.Birthdate != null ? Date.Create(command.Birthdate.Value) : Date.Empty;
      if (person.Birthday != birthday)
        person.CorrectBirthday(birthday);

      var address = new Address(
        command.Address.Street,
        command.Address.City,
        command.Address.State,
        command.Address.Country,
        command.Address.ZipCode);

      var phone = new PhoneNumber(command.PhoneNumber);
      var email = new EmailAddress(command.Email);
      var personalInfo = new PersonalInformation(phone, email, address);

      if (person.PersonalInformation == null || !person.PersonalInformation.Equals(personalInfo))
        person.AssignPersonalInformation(personalInfo);

      // Пока нет свойств в самом Student.
      this.studentRepository.UpdateAsync(student).Wait();
      this.personRepository.UpdateAsync(person).Wait();

      this.unitOfWork.Save();
    }

    public void AddStudentRelative(AddOrUpdateStudentRelativeCommand command)
    {
      var surname = command.Surname;
      var name = command.Name;
      var patronymic = command.Patronymic;
      var relativeName = new PersonName(surname, name, patronymic);
      var birthday = command.Birthdate != null ? Date.Create(command.Birthdate.Value) : Date.Empty;

      var address = new Address(
        command.Address.Street,
        command.Address.City,
        command.Address.State,
        command.Address.Country,
        command.Address.ZipCode);

      var phone = new PhoneNumber(command.PhoneNumber);
      var email = new EmailAddress(command.Email);
      var personalInfo = new PersonalInformation(phone, email, address);

      var relationship = ValueObject.GetAll<Relationship>().Where(r => r.Name == command.Relationship).Single();

      this.administrativeService.AddStudentRelative(command.StudentId, relativeName, birthday, personalInfo, relationship);
    }

    public void UpdateStudentRelative(int personId, AddOrUpdateStudentRelativeCommand command)
    {
      var surname = command.Surname;
      var name = command.Name;
      var patronymic = command.Patronymic;
      var relativeName = new PersonName(surname, name, patronymic);
      var birthday = command.Birthdate != null ? Date.Create(command.Birthdate.Value) : Date.Empty;

      var address = new Address(
        command.Address.Street,
        command.Address.City,
        command.Address.State,
        command.Address.Country,
        command.Address.ZipCode);

      var phone = new PhoneNumber(command.PhoneNumber);
      var email = new EmailAddress(command.Email);
      var personalInfo = new PersonalInformation(phone, email, address);

      var relationship = ValueObject.GetAll<Relationship>().Where(r => r.Name == command.Relationship).Single();

      this.administrativeService.UpdateStudentRelative(command.StudentId, personId, relativeName, birthday, personalInfo, relationship);
    }

    public StudentDetailsDTO StudentDetails(int studentId)
    {
      var studentSpecification = new StudentByIdSpecification(studentId).WithRelatives();
      var student = this.studentRepository.ListAsync(studentSpecification).Result.SingleOrDefault();
      if (student == null)
        throw new InvalidOperationException($"Student not found. Id: {studentId}");

      var assembler = new StudentDetailsDTOAssembler();
      var result = assembler.ToDTO(student);
      return result;
    }

    public AddOrUpdateStudentRelativeCommand EditStudentRelative(int studentId, int personId)
    {
      var studentSpecification = new StudentByIdSpecification(studentId).WithRelatives();
      var student = this.studentRepository.GetBySpecAsync(studentSpecification).Result;
      if (student == null)
        throw new InvalidOperationException($"Student not found. Id: {studentId}");

      var relative = student.Relatives.Where(r => r.PersonId == personId).SingleOrDefault();
      if (relative == null)
        throw new InvalidOperationException($"Student relative not found. PersonId: {personId}");

      var command = new AddOrUpdateStudentRelativeCommand()
      {
        Name = relative.Person.Name.Name,
        Surname = relative.Person.Name.Surname,
        Patronymic = relative.Person.Name.Patronymic,
        Relationship = relative.Relationship.Name,
        Birthdate = relative.Person.Birthday?.ToDateTime(),
        PhoneNumber = relative.Person.PhoneNumber(),
        Email = relative.Person.Email(),
        Address = new AddressDTO()
        {
          City = relative.Person.City(),
          Country = relative.Person.Country(),
          State = relative.Person.State(),
          Street = relative.Person.Street(),
          ZipCode = relative.Person.ZipCode()
        },
        StudentId = studentId
      };

      return command;
    }

    public CreateBranchCommand EditBranch(int branchId)
    {
      var branch = this.branchRepository.GetByIdAsync(branchId).Result;
      if (branch == null)
        throw new InvalidOperationException($"Branch not found. Id: {branchId}");

      return new CreateBranchCommand()
      {
        ContactPersonId = branch.ContactPersonId,
        City = branch.Address?.City,
        Country = branch.Address?.Country,
        State = branch.Address?.State,
        Street = branch.Address?.Street,
        ZipCode = branch.Address?.ZipCode
      };
    }

    public void UpdateBranch(int branchId, CreateBranchCommand command)
    {
      var branch = this.branchRepository.GetByIdAsync(branchId).Result;
      if (branch == null)
        throw new InvalidOperationException($"Branch not found. Id: {branchId}");

      if (branch.ContactPersonId != command.ContactPersonId)
      {
        var contactPerson = this.personRepository.GetByIdAsync(command.ContactPersonId).Result;
        if (contactPerson == null)
          throw new InvalidOperationException($"Branch contact person not found. Id: {command.ContactPersonId}");

        branch.AssignContactPerson(contactPerson);
      }

      var address = new Address(
        command.Street,
        command.City,
        command.State,
        command.Country,
        command.ZipCode);

      if (!Equals(branch.Address, address))
      {
        branch.CorrectAddress(address);
      }

      this.unitOfWork.Save();
    }
  }
}
