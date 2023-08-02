using System;
using System.Text.Json.Serialization;

namespace CoachsBox.WebApp.Pages.Facade
{
  public class MarkAttendanceCommand
  {
    public int AttendanceId { get; set; }

    public int CoachId { get; set; }

    public DateTime Date { get; set; }

    [JsonConverter(typeof(TimeSpanConverter))]
    public TimeSpan Start { get; set; }

    [JsonConverter(typeof(TimeSpanConverter))]
    public TimeSpan End { get; set; }

    public string AbsenceReason { get; set; }

    public bool IsTrialTraining { get; set; }
  }
}
