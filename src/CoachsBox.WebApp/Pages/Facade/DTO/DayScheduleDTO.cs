using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoachsBox.WebApp.Pages.Facade.DTO
{
  public class DayScheduleDTO
  {
    public DayScheduleDTO(int groupId, string groupName, int studentsCount, TimeSpan startTraining, TimeSpan endTraining)
    {
      this.GroupId = groupId;
      this.GroupName = groupName;
      this.StudentsCount = studentsCount;
      this.StartTraining = startTraining;
      this.EndTraining = endTraining;
    }

    public int GroupId { get; set; }

    public string GroupName { get; set; }

    public int StudentsCount { get; set; }

    public TimeSpan StartTraining { get; set; }

    public TimeSpan EndTraining { get; set; }

    public DayScheduleDTO()
    {
      // Требует Razor Pages.
    }
  }
}
