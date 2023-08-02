using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using CoachsBox.Coaching.Application;
using CoachsBox.Coaching.ScheduleModel;
using CoachsBox.Core;
using CoachsBox.Core.Primitives;
using CoachsBox.WebApp.Pages.Facade.DTO;
using CoachsBox.WebApp.Pages.Facade.Internal.Assembler;

namespace CoachsBox.WebApp.Pages.Facade.Internal
{
  public class SchedulingServiceFacade : ISchedulingServiceFacade
  {
    private readonly List<ScheduleTemplateDTO> scheduleTemplates = new List<ScheduleTemplateDTO>()
    {
      new ScheduleTemplateDTO(1, "1 тренировка", () => new List<RepeatableTrainingTimeDTO>()
      {
        new RepeatableTrainingTimeDTO() { DayOfWeek = DayOfWeek.Monday, Start = TimeSpan.Parse("18:00"), End = TimeSpan.Parse("19:00") }
      }),
      new ScheduleTemplateDTO(2, "2 тренировки", () => new List<RepeatableTrainingTimeDTO>()
      {
        new RepeatableTrainingTimeDTO() { DayOfWeek = DayOfWeek.Tuesday, Start = TimeSpan.Parse("18:00"), End = TimeSpan.Parse("19:00") },
        new RepeatableTrainingTimeDTO() { DayOfWeek = DayOfWeek.Thursday, Start = TimeSpan.Parse("18:00"), End = TimeSpan.Parse("19:00") }
      }),
      new ScheduleTemplateDTO(3, "3 тренировки", () => new List<RepeatableTrainingTimeDTO>()
      {
        new RepeatableTrainingTimeDTO() { DayOfWeek = DayOfWeek.Monday, Start = TimeSpan.Parse("18:00"), End = TimeSpan.Parse("19:00") },
        new RepeatableTrainingTimeDTO() { DayOfWeek = DayOfWeek.Wednesday, Start = TimeSpan.Parse("18:00"), End = TimeSpan.Parse("19:00") },
        new RepeatableTrainingTimeDTO() { DayOfWeek = DayOfWeek.Friday, Start = TimeSpan.Parse("18:00"), End = TimeSpan.Parse("19:00") }
      }),
      new ScheduleTemplateDTO(5, "5 тренировок", () => new List<RepeatableTrainingTimeDTO>()
      {
        new RepeatableTrainingTimeDTO() { DayOfWeek = DayOfWeek.Monday, Start = TimeSpan.Parse("18:00"), End = TimeSpan.Parse("19:00") },
        new RepeatableTrainingTimeDTO() { DayOfWeek = DayOfWeek.Tuesday, Start = TimeSpan.Parse("18:00"), End = TimeSpan.Parse("19:00") },
        new RepeatableTrainingTimeDTO() { DayOfWeek = DayOfWeek.Wednesday, Start = TimeSpan.Parse("18:00"), End = TimeSpan.Parse("19:00") },
        new RepeatableTrainingTimeDTO() { DayOfWeek = DayOfWeek.Thursday, Start = TimeSpan.Parse("18:00"), End = TimeSpan.Parse("19:00") },
        new RepeatableTrainingTimeDTO() { DayOfWeek = DayOfWeek.Friday, Start = TimeSpan.Parse("18:00"), End = TimeSpan.Parse("19:00") }
      }),
    };
    private readonly IScheduleRepository scheduleRepository;
    private readonly ISchedulingService schedulingService;

    public SchedulingServiceFacade(
      IScheduleRepository scheduleRepository,
      ISchedulingService schedulingService)
    {
      this.scheduleRepository = scheduleRepository;
      this.schedulingService = schedulingService;
    }

    public IReadOnlyCollection<ScheduleTemplateDTO> ListScheduleTemplates()
    {
      return this.scheduleTemplates;
    }

    public int CreateSchedule(CreateScheduleCommand createCommand, ScheduleTemplateDTO scheduleTemplate)
    {
      var groupId = createCommand.GroupId;
      var coachId = createCommand.CoachId;
      var branchId = createCommand.BranchId;

      var trainings = scheduleTemplate.CreateTrainings().Select(t => new TrainingTime(
          dayOfWeek: t.DayOfWeek,
          start: new TimeOfDay((byte)t.Start.Hours, (byte)t.Start.Minutes),
          end: new TimeOfDay((byte)t.End.Hours, (byte)t.End.Minutes))
        )
        .ToList();

      var scheduleSpecification = new ScheduleSpecification(coachId, groupId, branchId);
      return this.schedulingService.CreateSchedule(scheduleSpecification, trainings);
    }

    public void UpdateSchedule(UpdateScheduleCommand updateCommand)
    {
      var scheduleId = updateCommand.ScheduleId;
      var coachId = updateCommand.CoachId;

      var trainings = updateCommand.Schedule.Select(t =>
        new TrainingTime(
          dayOfWeek: t.DayOfWeek,
          start: new TimeOfDay((byte)t.Start.Hours, (byte)t.Start.Minutes),
          end: new TimeOfDay((byte)t.End.Hours, (byte)t.End.Minutes))
        )
        .ToList();

      this.schedulingService.UpdateTraining(scheduleId, trainings, coachId);
    }

    public ScheduleDTO LoadSchedule(int scheduleId)
    {
      var schedule = this.scheduleRepository.GetByIdAsync(scheduleId).Result;

      var assembler = new TrainingTimeDTOAssembler();
      var onlyScheduledTime = schedule.TrainingList.Where(t => t.Date.Equals(Date.Empty));
      var trainigs = assembler.ToDTOList(onlyScheduledTime);

      var coachId = schedule.CoachId;
      var groupId = schedule.GroupId;
      var branchId = schedule.BranchId;
      var scheduleSpecification = new ScheduleSpecification(coachId, groupId, branchId);

      return new ScheduleDTO(scheduleId, scheduleSpecification, trainigs);
    }

    public IReadOnlyCollection<DayOfWeekDTO> ListDayOfWeek()
    {
      return Enum.GetValues(typeof(DayOfWeek))
        .Cast<DayOfWeek>()
        // Сделаем понедельник первым днем недели.
        .OrderBy(d => d == DayOfWeek.Sunday ? 7 : (int)d)
        .Select(d => new DayOfWeekDTO(d, DateTimeFormatInfo.CurrentInfo.GetDayName(d)))
        .ToList();
    }

    public IReadOnlyCollection<DayScheduleDTO> ListScheduleOnDateByCoach(int coachId, DateTime weekDate)
    {
      var currentDay = Date.Create(weekDate);
      return this.ListScheduleOnDateInternal(weekDate, () => this.schedulingService.ListScheduleOnDate(coachId, currentDay));
    }
    public IReadOnlyCollection<DayScheduleDTO> ListScheduleOnDate(DateTime weekDate)
    {
      var currentDay = Date.Create(weekDate);
      return this.ListScheduleOnDateInternal(weekDate, () => this.schedulingService.ListScheduleOnDate(currentDay));
    }

    private IReadOnlyCollection<DayScheduleDTO> ListScheduleOnDateInternal(DateTime weekDate, Func<IReadOnlyList<Schedule>> listScheduleOnDate)
    {
      var currentDay = Date.Create(weekDate);
      var scheduleList = listScheduleOnDate();
      var dayScheduleDTOAssembler = new DayScheduleDTOAssembler();
      var result = new List<DayScheduleDTO>();
      foreach (var schedule in scheduleList)
      {
        foreach (var trainingTime in schedule.TrainingList)
        {
          if (trainingTime.DayOfWeek == weekDate.DayOfWeek)
            result.Add(dayScheduleDTOAssembler.ToDto(schedule.Group, trainingTime));
          else if (trainingTime.Date == currentDay)
            result.Add(dayScheduleDTOAssembler.ToDto(schedule.Group, trainingTime));
        }
      }
      return result.OrderBy(s => s.StartTraining).ToList();
    }
  }
}
