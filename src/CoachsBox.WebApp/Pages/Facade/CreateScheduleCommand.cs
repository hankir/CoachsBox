using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using CoachsBox.WebApp.Pages.Facade.DTO;

namespace CoachsBox.WebApp.Pages.Facade
{
  public class CreateScheduleCommand
  {
    public CreateScheduleCommand(int groupId, int branchId)
    {
      this.GroupId = groupId;
      this.BranchId = branchId;
    }

    public int GroupId { get; set; }

    [Display(Name = "Тренер")]
    public int CoachId { get; set; }

    public int BranchId { get; set; }

    public CreateScheduleCommand()
    {
      // Требует Razor Pages
    }
  }
}