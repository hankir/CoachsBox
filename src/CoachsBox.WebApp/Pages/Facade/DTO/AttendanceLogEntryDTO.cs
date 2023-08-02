using System;

namespace CoachsBox.WebApp.Pages.Facade.DTO
{
  public class AttendanceLogEntryDTO
  {
    public int GroupId { get; set; }

    public int StudentId { get; set; }

    public int? CoachId { get; set; }

    public string StudentFullName { get; set; }

    public decimal? GroupBalance { get; set; }

    public DateTime Date { get; set; }

    public TimeSpan Start { get; set; }

    public TimeSpan End { get; set; }

    public string AbsenceReason { get; set; }

    public bool IsExists { get; set; }

    public bool IsTrial { get; set; }

    public bool Excluded { get; set; }
  }
}
