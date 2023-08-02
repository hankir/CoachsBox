using System;
using System.Collections.Generic;
using System.Linq;
using CoachsBox.Coaching.ScheduleModel;
using CoachsBox.WebApp.Pages.Facade.DTO;

namespace CoachsBox.WebApp.Pages.Facade.Internal.Assembler
{
  public class TrainingTimeDTOAssembler
  {
    public IReadOnlyCollection<RepeatableTrainingTimeDTO> ToDTOList(IEnumerable<TrainingTime> trainings)
    {
      return trainings
        .Select(t => new RepeatableTrainingTimeDTO()
        {
          DayOfWeek = t.DayOfWeek,
          Start = new TimeSpan(t.Start.Hours, t.Start.Minutes, 0),
          End = new TimeSpan(t.End.Hours, t.End.Minutes, 0)
        }).ToList();
    }
  }
}
