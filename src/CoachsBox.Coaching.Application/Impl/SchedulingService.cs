using System;
using System.Collections.Generic;
using System.Linq;
using CoachsBox.Coaching.ScheduleModel;
using CoachsBox.Core.Primitives;

namespace CoachsBox.Coaching.Application.Impl
{
  public class SchedulingService : ISchedulingService
  {
    private readonly IScheduleRepository scheduleRepository;

    public SchedulingService(IScheduleRepository scheduleRepository)
    {
      this.scheduleRepository = scheduleRepository;
    }

    public int CreateSchedule(ScheduleSpecification scheduleSpecification, IReadOnlyList<TrainingTime> trainingList)
    {
      var existSchedule = this.scheduleRepository.ListAsync(scheduleSpecification).Result.SingleOrDefault();
      if (existSchedule == null)
      {
        var schedule = new Schedule(scheduleSpecification, trainingList);
        this.scheduleRepository.AddAsync(schedule).Wait();
        this.scheduleRepository.SaveAsync().Wait();
        return schedule.Id;
      }

      throw new InvalidOperationException("Schedule already exists by specification");
    }

    public IReadOnlyList<Schedule> ListScheduleOnDate(int coachId, Date day)
    {
      var dayOfWeek = TrainingTime.CalculateDayOfWeek(day);
      var byDayOfWeekSpec = new ScheduleOnDayOfWeekSpecification(coachId, dayOfWeek);
      var scheduleByDayOfWeek = this.scheduleRepository.ListAsync(byDayOfWeekSpec).Result;
      return new List<Schedule>(scheduleByDayOfWeek);
    }

    public IReadOnlyList<Schedule> ListScheduleOnDate(Date day)
    {
      var dayOfWeek = TrainingTime.CalculateDayOfWeek(day);
      var byDayOfWeekSpec = new ScheduleOnDayOfWeekSpecification(dayOfWeek);
      var scheduleByDayOfWeek = this.scheduleRepository.ListAsync(byDayOfWeekSpec).Result;
      return new List<Schedule>(scheduleByDayOfWeek);
    }

    public void UpdateTraining(int scheduleId, IReadOnlyList<TrainingTime> trainingList, int coachId)
    {
      var existSchedule = this.scheduleRepository.GetByIdAsync(scheduleId).Result;
      if (existSchedule != null)
      {
        existSchedule.UpdateTrainings(trainingList);

        if (existSchedule.CoachId != coachId)
          existSchedule.ReplaceCoachTo(coachId);

        this.scheduleRepository.UpdateAsync(existSchedule).Wait();
        this.scheduleRepository.SaveAsync().Wait();
      }
      else
        throw new InvalidOperationException($"Schedule [Id: {scheduleId}] not found");
    }
  }
}
