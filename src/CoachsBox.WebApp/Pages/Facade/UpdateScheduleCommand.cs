using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using CoachsBox.WebApp.Pages.Facade.DTO;

namespace CoachsBox.WebApp.Pages.Facade
{
  public class UpdateScheduleCommand
  {
    public UpdateScheduleCommand(int scheduleId)
    {
      this.ScheduleId = scheduleId;
    }

    [Display(Name = "Тренер")]
    public int CoachId { get; set; }

    public int GroupId { get; set; }

    public int ScheduleId { get; set; }

    public List<RepeatableTrainingTimeDTO> Schedule { get; private set; } = new List<RepeatableTrainingTimeDTO>();

    public UpdateScheduleCommand()
    {
      // Требует Razor Pages.
    }
  }
}