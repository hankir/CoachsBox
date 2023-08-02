using System.Linq;
using CoachsBox.Coaching.Accounting.StudentAccountModel;
using CoachsBox.Coaching.BranchModel;
using CoachsBox.Coaching.CoachModel;
using CoachsBox.Coaching.GroupModel;
using CoachsBox.Coaching.Infrastructure;
using CoachsBox.Coaching.PersonModel;
using CoachsBox.Coaching.StudentModel;
using CoachsBox.Core.Primitives;
using Xunit;
using Xunit.Abstractions;
using static CoachsBox.IntegrationTests.TestsCoachsBoxContextSeed;

namespace CoachsBox.IntegrationTests
{
  public class AddNew : IntegrationTestsBase
  {
    [Fact]
    public void PersonForStudentExists()
    {
      var existingPerson = this.FindByName(PersonConstants.Name);

      Assert.NotNull(existingPerson);
      Assert.Equal(Gender.Male, existingPerson.Gender);
      Assert.Equal(PersonConstants.Name, existingPerson.Name);
      Assert.Equal(new Date(7, Month.July, 2011), existingPerson.Birthday);
    }

    [Fact]
    public void PersonForCoachExists()
    {
      var existingPerson = this.FindByName(CoachConstants.Name);

      Assert.NotNull(existingPerson);
      Assert.Equal(Gender.Male, existingPerson.Gender);
      Assert.Equal(CoachConstants.Name, existingPerson.Name);
      Assert.Equal(new Date(21, Month.July, 1974), existingPerson.Birthday);
    }

    [Fact]
    public void CreateStudentGroup()
    {
      int groupId;
      int studentId;
      int branchId;
      Person existingPerson;
      using (var context = this.CreateContext())
      {
        var personRepository = new PersonRepository(context);
        var groupRepository = new GroupRepository(context);
        var studentRepository = new StudentRepository(context);
        var branchRepository = new BranchRepository(context);

        var spec = new Coaching.PersonModel.FindByNameSpecification(PersonConstants.Name);
        existingPerson = personRepository.ListAsync(spec).Result.Single();

        var branch = branchRepository.ListAllAsync().Result.Single();

        var group = new Group(branch.Id, "Nameless", Sport.Taekwondo, new TrainingProgramSpecification(4, 6));
        groupRepository.AddAsync(group).Wait();

        var student = new Student(existingPerson.Id, "Перешел из карате.");
        studentRepository.AddAsync(student).Wait();

        var student2 = new Student(existingPerson.Id, "Перешел из afr.");
        studentRepository.AddAsync(student).Wait();

        group.EnrollStudent(student2);
        group.EnrollStudent(student);

        context.SaveChanges();

        studentId = student.Id;
        branchId = branch.Id;
        groupId = group.Id;
      }

      var existingGroup = this.GetExistingGroup(groupId);
      Assert.NotNull(existingGroup);

      var existingStudent = this.GetExistingStudent(studentId);
      Assert.NotNull(existingStudent);

      var existingStudentAccount = this.GetExistingStudentAccount(studentId);
      Assert.NotNull(existingStudentAccount);

      Assert.True(existingGroup.IsEnrolled(studentId));
      Assert.Equal(existingPerson, existingStudent.Person);

      Assert.Equal(branchId, existingGroup.BranchId);
    }

    [Fact]
    public void CreateNewBranch()
    {
      int branchId;
      int coachId;
      using (var context = this.CreateContext())
      {
        var personRepository = new PersonRepository(context);
        var coachRepository = new CoachRepository(context);
        var branchRepository = new BranchRepository(context);

        var existingPerson = this.FindByName(CoachConstants.Name);
        var coach = new Coach(existingPerson.Id);
        coachRepository.AddAsync(coach).Wait();

        var branch = branchRepository.ListAllAsync().Result.Single();
        branch.IncludeCoach(coach);

        context.SaveChanges();

        coachId = coach.Id;
        branchId = branch.Id;
      }

      var existingBranch = this.GetExistingBranch(branchId);
      Assert.Equal(1, existingBranch.CoachingStaff.Count);

      var existingCoach = this.GetExistingCoach(coachId);
      Assert.Contains(existingBranch.CoachingStaff, m => m.Coach == existingCoach);
    }

    private Coach GetExistingCoach(int coachId)
    {
      using var context = this.CreateContext();
      var coachRepository = new CoachRepository(context);
      return coachRepository.GetByIdAsync(coachId).Result;
    }

    private Branch GetExistingBranch(int branchId)
    {
      using var context = this.CreateContext();
      var branchRepository = new BranchRepository(context);
      return branchRepository.GetByIdAsync(branchId).Result;
    }

    private Student GetExistingStudent(int studentId)
    {
      using var context = this.CreateContext();
      var studentRepository = new StudentRepository(context);
      return studentRepository.GetByIdAsync(studentId).Result;
    }

    private StudentAccount GetExistingStudentAccount(int studentId)
    {
      using var context = this.CreateContext();
      var studentRepository = new StudentAccountRepository(context);
      var specification = new StudentAccountSpecification(studentId);
      return studentRepository.GetBySpecAsync(specification).Result;
    }

    private Person FindByName(PersonName name)
    {
      var spec = new Coaching.PersonModel.FindByNameSpecification(name);
      using var context = this.CreateContext();
      var studentRepository = new PersonRepository(context);
      return studentRepository.ListAsync(spec).Result.SingleOrDefault();
    }

    private Group GetExistingGroup(int id)
    {
      using var context = this.CreateContext();
      var groupRepository = new GroupRepository(context);
      return groupRepository.GetByIdAsync(id).Result;
    }

    public AddNew(ITestOutputHelper output) : base(output)
    {
      using var context = this.CreateContext();
      new TestsCoachsBoxContextSeed().SeedAsync(context).Wait();
    }
  }
}
