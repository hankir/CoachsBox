using System;

namespace CoachsBox.WebApp.AppFacade.Groups.DTO
{
  public class GroupDetailsStudentAttendanceDTO
  {
    public DateTime Date { get; set; }

    public TimeSpan Start { get; set; }

    public TimeSpan End { get; set; }

    public string AbsenceReason { get; set; }

    public bool IsTrialTraining { get; set; }
  }
}
