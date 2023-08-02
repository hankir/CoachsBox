using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CoachsBox.WebApp.AppFacade.Groups.DTO
{
  public class GroupDetailsStudentDTO
  {
    public GroupDetailsStudentDTO()
    {
      this.AttendanceLog = new List<GroupDetailsStudentAttendanceDTO>();
    }

    public int StudentId { get; set; }

    public string StudentFullName { get; set; }

    public string StudentShortName { get; set; }

    public string Email { get; set; }

    public string Phone { get; set; }

    [Display(Name = "Счет группы")]
    public decimal? GroupBalance { get; set; }

    public List<GroupDetailsStudentAttendanceDTO> AttendanceLog { get; set; }

    public byte TrialTrainingCount { get; set; }

    public bool CanEnableTrialTraining { get; set; }
  }
}
