using System.Collections.Generic;
using CoachsBox.Coaching.Application.Data;
using CoachsBox.WebApp.Pages.Facade.DTO;

namespace CoachsBox.WebApp.AppFacade.Attendance.DTO
{
  public class AttendanceLogViewDTO
  {
    public List<TrainingTimeInfo> ColumnsTitle { get; set; }

    public List<AttendanceLogViewRowTitle> RowsTitle { get; set; }

    public AttendanceLogDTO AttendanceLog { get; set; }
  }

  public class AttendanceLogViewRowTitle
  {
    public int StudentId { get; set; }

    public string StudentFullName { get; set; }

    public string StudentShortName { get; set; }

    public bool Excluded { get; set; }
  }
}
