using System;
using System.Collections.Generic;
using System.Linq;
using CoachsBox.Coaching.Application.Impl;
using CoachsBox.Coaching.BranchModel;
using CoachsBox.Coaching.CoachModel;
using CoachsBox.Coaching.GroupModel;
using CoachsBox.Coaching.Infrastructure;
using CoachsBox.Coaching.PersonModel;
using CoachsBox.Coaching.ScheduleModel;
using CoachsBox.Core.Primitives;
using Xunit;
using Xunit.Abstractions;

namespace CoachsBox.IntegrationTests
{
  public class ScheduleTests : IntegrationTestsBase
  {
    [Fact]
    public void ListScheduleByDay()
    {
      var currentDay = new Date((byte)DateTime.Now.Day, Month.GetAll<Month>().Where(m => m.Number == DateTime.Now.Month).Single(), DateTime.Now.Year);
      var coachId = this.GenerateTwoTrainigsByWeek(currentDay);
      var byDaySpec = new ScheduleOnDayByCoachSpecification(coachId, currentDay);

      using var context = this.CreateContext();
      var scheduleRepository = new ScheduleRepository(context);
      var currentDaySchedule = scheduleRepository.ListAsync(byDaySpec).Result;
      Assert.Equal(1, currentDaySchedule.Count);
    }

    [Fact]
    public void ListScheduleByDayOfWeek()
    {
      var currentDay = Date.Create(DateTime.Now);
      var coachId = this.GenerateTwoTrainigsByWeek(currentDay);
      var byDayOfWeekSpec = new ScheduleOnDayOfWeekSpecification(coachId, TrainingTime.CalculateDayOfWeek(currentDay));

      using var context = this.CreateContext();
      var scheduleRepository = new ScheduleRepository(context);
      var currentDaySchedule = scheduleRepository.ListAsync(byDayOfWeekSpec).Result;
      Assert.Equal(1, currentDaySchedule.Count);
    }

    [Fact]
    public void ListScheduleOnDay()
    {
      var currentDay = new Date((byte)DateTime.Now.Day, Month.GetAll<Month>().Where(m => m.Number == DateTime.Now.Month).Single(), DateTime.Now.Year);
      var coachId = this.GenerateTwoTrainigsByWeek(currentDay);

      using var context = this.CreateContext();
      var scheduleRepository = new ScheduleRepository(context);
      var schedulingService = new SchedulingService(scheduleRepository);

      var currentDaySchedule = schedulingService.ListScheduleOnDate(coachId, currentDay);
      Assert.Equal(1, currentDaySchedule.Count);
    }

    private int GenerateTwoTrainigsByWeek(Date additionalTrainingDay)
    {
      var personName = new PersonName("surname", "name");
      var person = new Person(personName);
      var coach = new Coach(person);
      var branch = new Branch(Address.Empty, person);
      var group = new Group(branch, "Черепашки ниндзя", new Sport("Тхэквондо"), new TrainingProgramSpecification(4, 6));

      using var context = this.CreateContext();

      var personRepository = new PersonRepository(context);
      var coachRepository = new CoachRepository(context);
      var branchRepository = new BranchRepository(context);
      var groupRepository = new GroupRepository(context);

      personRepository.AddAsync(person).Wait();
      coachRepository.AddAsync(coach).Wait();
      branchRepository.AddAsync(branch).Wait();
      groupRepository.AddAsync(group).Wait();

      context.SaveChanges();

      var trainings = new List<TrainingTime>();
      trainings.Add(new TrainingTime(DayOfWeek.Tuesday, new TimeOfDay(19, 00), new TimeOfDay(20, 00)));
      trainings.Add(new TrainingTime(TrainingTime.CalculateDayOfWeek(additionalTrainingDay), new TimeOfDay(19, 00), new TimeOfDay(20, 00)));
      trainings.Add(new TrainingTime(additionalTrainingDay, new TimeOfDay(19, 00), new TimeOfDay(20, 00)));

      var scheduleSpec = new ScheduleSpecification(coach.Id, group.Id, branch.Id);
      var schedule = new Schedule(scheduleSpec, trainings);
      var scheduleRepository = new ScheduleRepository(context);
      scheduleRepository.AddAsync(schedule).Wait();

      context.SaveChanges();
      return coach.Id;
    }

    public ScheduleTests(ITestOutputHelper output) : base(output) { }
  }
}
