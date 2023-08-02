using System.ComponentModel.DataAnnotations;
using CoachsBox.WebApp.AppFacade.Primitives.DTO;
using CoachsBox.WebApp.Areas.Admin.Facade.DTO;

namespace CoachsBox.WebApp.Pages.Facade.DTO
{
  public class GroupStudentDTO
  {
    public PersonDTO Person { get; set; }

    public int StudentId { get; set; }

    public bool IsTrialTraining { get; set; }
  }
}
