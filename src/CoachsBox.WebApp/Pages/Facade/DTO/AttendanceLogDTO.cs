using System.Collections.Generic;

namespace CoachsBox.WebApp.Pages.Facade.DTO
{
  public class AttendanceLogDTO
  {
    public int AttendanceId { get; set; }

    public int GroupId { get; set; }

    public string GroupName { get; set; }

    public List<AttendanceLogEntryDTO> Entries { get; set; }
  }
}
