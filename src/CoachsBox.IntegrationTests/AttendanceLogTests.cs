using System;
using System.Linq;
using CoachsBox.Coaching.AttendanceLogModel;
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
  public class AttendanceLogTests : IntegrationTestsBase
  {
    private const int AttendanceLogYear = 2020;

    [Fact]
    public void CreateAttendanceLog()
    {
      var oneMarkDate = new Date(21, Month.April, AttendanceLogYear);
      var twoMarkDate = new Date(23, Month.April, AttendanceLogYear);

      int groupId;
      int studentId;
      using (var context = this.CreateContext())
      {
        var branchRepository = new BranchRepository(context);
        var studentRepository = new StudentRepository(context);
        var personRepository = new PersonRepository(context);
        var groupRepository = new GroupRepository(context);
        var attendanceLogRepository = new AttendanceLogRepository(context);

        var branch = branchRepository.ListAllAsync().Result.First();
        var group = new Group(branch, "Крутая группа", Sport.Taekwondo, new TrainingProgramSpecification(4, 6));

        var spec = new FindByNameSpecification(PersonConstants.Name);
        var existingPerson = personRepository.ListAsync(spec).Result.Single();
        var student = new Student(existingPerson.Id, string.Empty);

        var attendanceLog = new AttendanceLog(group.Id, AttendanceLogYear);
        attendanceLog.MarkStudent(student.Id, 1, oneMarkDate, new TimeOfDay(19, 0), new TimeOfDay(20, 0));
        attendanceLog.MarkStudent(student.Id, 1, twoMarkDate, new TimeOfDay(19, 0), new TimeOfDay(20, 0), Absence.Sickness);

        attendanceLogRepository.AddAsync(attendanceLog).Wait();
        context.SaveChanges();

        groupId = group.Id;
        studentId = student.Id;
      }

      using (var context = this.CreateContext())
      {
        var attendanceLog = context.AttendanceLogs.Where(a => a.GroupId == groupId && a.Year == AttendanceLogYear).Single();

        Assert.Equal(2, attendanceLog.Entries.Count);
        Assert.Equal(2, attendanceLog.Entries.Where(e => e.StudentId == studentId).Count());
        Assert.Equal(2, attendanceLog.Entries.Where(e => e.Start.Equals(new TimeOfDay(19, 0))).Count());
        Assert.Equal(2, attendanceLog.Entries.Where(e => e.End.Equals(new TimeOfDay(20, 0))).Count());
        Assert.Single(attendanceLog.Entries.Where(e => e.AbsenceReason != null && e.AbsenceReason.Equals(Absence.Sickness)));

        Assert.Throws<InvalidOperationException>(() => attendanceLog.MarkStudent(studentId, 1, oneMarkDate, new TimeOfDay(19, 0), new TimeOfDay(20, 0)));
        Assert.Throws<ArgumentException>(() => attendanceLog.MarkStudent(studentId, 1, new Date(21, Month.April, AttendanceLogYear + 1), new TimeOfDay(19, 0), new TimeOfDay(20, 0)));
      }
    }

    public AttendanceLogTests(ITestOutputHelper output) : base(output)
    {
      using var context = this.CreateContext();
      new TestsCoachsBoxContextSeed().SeedAsync(context).Wait();
    }
  }
}
