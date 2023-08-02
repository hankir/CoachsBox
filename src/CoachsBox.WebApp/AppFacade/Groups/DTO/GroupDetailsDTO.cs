using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using CoachsBox.Coaching.Application.Data;
using CoachsBox.Core.Primitives;
using CoachsBox.WebApp.Pages.Facade.DTO;

namespace CoachsBox.WebApp.AppFacade.Groups.DTO
{
  public class GroupDetailsDTO
  {
    public GroupDetailsDTO()
    {
      this.Students = new List<GroupDetailsStudentDTO>();
      this.AttendanceReportCardTitle = new List<TrainingTimeInfo>();
      this.Schedule = new List<RepeatableTrainingTimeDTO>();
    }

    public decimal CurrentPayments { get; set; }

    public int GroupId { get; set; }

    public string GroupName { get; set; }


    public int? ScheduleId { get; set; }

    public int? CoachId { get; set; }

    [Display(Name = "Тренер")]
    public string CoachFullName { get; set; }

    [Display(Name = "Ученики")]
    public List<GroupDetailsStudentDTO> Students { get; set; }

    public List<TrainingTimeInfo> AttendanceReportCardTitle { get; set; }

    public IReadOnlyCollection<RepeatableTrainingTimeDTO> Schedule { get; set; }

    public Money Tariff { get; set; }
  }
}
