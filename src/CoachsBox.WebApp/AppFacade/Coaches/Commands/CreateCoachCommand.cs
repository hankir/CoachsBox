using System.ComponentModel.DataAnnotations;
using CoachsBox.WebApp.AppFacade.Primitives.Commands;

namespace CoachsBox.WebApp.AppFacade.Coaches.Commands
{
  public class CreateCoachCommand : CreatePersonCommand
  {
    [Display(Name = "Филиал")]
    public int BranchId { get; set; }

    [Required(ErrorMessage = "Номер телефона обязателен для заполнения")]
    public override string PhoneNumber { get; set; }

    [Required(ErrorMessage = "Эл. почта обязательна для заполнения")]
    public override string Email { get; set; }
  }
}
