using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using CoachsBox.WebApp.Areas.Admin.Facade.DTO;

namespace CoachsBox.WebApp.Pages.Facade.DTO
{
  public class GroupDTO
  {
    public GroupDTO(int id, BranchRefDTO branch, string name, string sport, int minAge, int maxAge)
    {
      this.Id = id;
      this.Branch = branch;
      this.Name = name;
      this.Sport = sport;
      this.MinAge = minAge;
      this.MaxAge = maxAge;
      this.Students = new List<GroupStudentDTO>();
    }

    public int Id { get; }

    [Display(Name = "Филиал")]
    public BranchRefDTO Branch { get; set; }

    [Display(Name = "Тренер")]
    public CoachDTO Coach { get; set; }

    public string Name { get; set; }

    public string Sport { get; set; }

    [Display(Name = "Возраст от")]
    public int MinAge { get; set; }

    [Display(Name = "до")]
    public int MaxAge { get; set; }

    public int? ScheduleId { get; set; }

    [Display(Name = "Ученики")]
    public List<GroupStudentDTO> Students { get; set; }
  }
}
