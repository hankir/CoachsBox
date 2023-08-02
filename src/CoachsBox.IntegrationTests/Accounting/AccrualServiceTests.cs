using System;
using CoachsBox.Coaching.Accounting.AgreementRegistryEntryModel;
using CoachsBox.Coaching.Accounting.CoachingServiceAgreementModel;
using CoachsBox.Coaching.Accounting.GroupAccountModel;
using CoachsBox.Coaching.Accounting.StudentAccountingEventModel;
using CoachsBox.Coaching.Application.Impl;
using CoachsBox.Coaching.AttendanceLogModel;
using CoachsBox.Coaching.BranchModel;
using CoachsBox.Coaching.GroupModel;
using CoachsBox.Coaching.Infrastructure;
using CoachsBox.Coaching.PersonModel;
using CoachsBox.Coaching.ScheduleModel;
using CoachsBox.Coaching.StudentModel;
using CoachsBox.Core;
using CoachsBox.Core.Primitives;
using Xunit;
using Xunit.Abstractions;

namespace CoachsBox.IntegrationTests.Accounting
{
  public class AccrualServiceTests : IntegrationTestsBase
  {
    [Fact(DisplayName = "bug#92")]
    public void AccrualForOneNewStudentInTwoGroups()
    {
      using (var context = this.CreateContext())
      {
        var logger = this.CreateLogger<AccrualService>();
        using var unitOfWork = new CoachsBoxUnitOfWork(context);
        var accountingEventRepository = new StudentAccountingEventRepository(context);
        var aggrementRegistryRepository = new AgreementRegistryEntryRepository(context);
        var attendanceLogRepository = new AttendanceLogRepository(context);
        var scheduleRepository = new ScheduleRepository(context);
        var studentAccountRepository = new TestStudentAccountRepository(new StudentAccountRepository(context));
        var groupRepository = new GroupRepository(context);
        var groupAccountRepository = new GroupAccountRepository(context);
        var branchRepository = new BranchRepository(context);
        var agreementRepository = new CoachingServiceAgreementRepository(context);
        var studentRepository = new StudentRepository(context);

        var studentPerson = new Person(new PersonName("Пупкин", "Иван"));
        var student = new Student(studentPerson, "Новичок");
        studentRepository.AddAsync(student).Wait();

        var branchContactPerson = new Person(new PersonName("Пупкин", "Иван"));
        var branch = new Branch(new Address("Street", "City", "State", "Country", "Zipcode"), branchContactPerson);
        branchRepository.AddAsync(branch).Wait();

        var group = new Group(branch, "Черепашки-ниндзя", Sport.Taekwondo, new TrainingProgramSpecification(4, 6));
        group.EnrollStudent(student);

        var group2 = new Group(branch, "Самураи", Sport.Taekwondo, new TrainingProgramSpecification(4, 6));
        group2.EnrollStudent(student);

        groupRepository.AddAsync(group).Wait();
        groupRepository.AddAsync(group2).Wait();

        var schedule = new Schedule(new ScheduleSpecification(group.Id, group.BranchId), new[]
        {
          new TrainingTime(DayOfWeek.Tuesday, TimeOfDay.Create(new TimeSpan(19,0,0)), TimeOfDay.Create(new TimeSpan(20,0,0))),
          new TrainingTime(DayOfWeek.Thursday, TimeOfDay.Create(new TimeSpan(19,0,0)), TimeOfDay.Create(new TimeSpan(20,0,0)))
        });

        var schedule2 = new Schedule(new ScheduleSpecification(group2.Id, group.BranchId), new[]
        {
          new TrainingTime(DayOfWeek.Tuesday, TimeOfDay.Create(new TimeSpan(19,0,0)), TimeOfDay.Create(new TimeSpan(20,0,0))),
          new TrainingTime(DayOfWeek.Thursday, TimeOfDay.Create(new TimeSpan(19,0,0)), TimeOfDay.Create(new TimeSpan(20,0,0)))
        });

        scheduleRepository.AddAsync(schedule).Wait();
        scheduleRepository.AddAsync(schedule2).Wait();

        var attendanceLog = new AttendanceLog(group.Id, 2020);
        var attendanceLog2 = new AttendanceLog(group2.Id, 2020);
        attendanceLogRepository.AddAsync(attendanceLog).Wait();
        attendanceLogRepository.AddAsync(attendanceLog2).Wait();

        var tariff = new CoachingServiceAgreement(Money.CreateRuble(150), StudentAccountingEventType.Accrual, "Общие тренировки");
        var tariff2 = new CoachingServiceAgreement(Money.CreateRuble(500), StudentAccountingEventType.Accrual, "Персональные тренировки");
        agreementRepository.AddAsync(tariff).Wait();
        agreementRepository.AddAsync(tariff2).Wait();

        var groupAccount = new GroupAccount(group.Id);
        groupAccountRepository.AddAsync(groupAccount).Wait();

        var groupAccount2 = new GroupAccount(group2.Id);
        groupAccountRepository.AddAsync(groupAccount2).Wait();

        aggrementRegistryRepository.AddAsync(new AgreementRegistryEntry(tariff.Id, group.Id, groupAccount.Id)).Wait();
        aggrementRegistryRepository.AddAsync(new AgreementRegistryEntry(tariff2.Id, group2.Id, groupAccount2.Id)).Wait();

        unitOfWork.Save();

        var accrualService = new AccrualService(
          logger,
          unitOfWork,
          accountingEventRepository,
          aggrementRegistryRepository,
          attendanceLogRepository,
          scheduleRepository,
          studentAccountRepository);

        accrualService.CalculateAccrualForDate(Date.Create(Watch.Now.DateTime));

        Assert.Equal(0, studentAccountRepository.AddedCount);
      }
    }

    public AccrualServiceTests(ITestOutputHelper output)
      : base(output)
    {
    }
  }
}
