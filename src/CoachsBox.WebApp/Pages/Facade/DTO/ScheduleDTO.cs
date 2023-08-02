using System.Collections.Generic;
using CoachsBox.Coaching.ScheduleModel;

namespace CoachsBox.WebApp.Pages.Facade.DTO
{
  public class ScheduleDTO
  {
    public ScheduleDTO(int scheduleId, ScheduleSpecification specification, IReadOnlyCollection<RepeatableTrainingTimeDTO> trainings)
    {
      this.ScheduleId = scheduleId;
      this.GroupId = specification.GroupId;
      this.CoachId = specification.CoachId;
      this.BranchId = specification.BranchId;
      this.Trainings = trainings;
    }

    public int ScheduleId { get; }

    public int GroupId { get; }

    public int CoachId { get; }

    public int BranchId { get; }

    public IReadOnlyCollection<RepeatableTrainingTimeDTO> Trainings { get; }
  }
}
