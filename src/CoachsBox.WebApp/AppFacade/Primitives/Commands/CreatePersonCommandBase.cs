using System.ComponentModel.DataAnnotations;

namespace CoachsBox.WebApp.AppFacade.Primitives.Commands
{
  public abstract class CreatePersonCommandBase
  {
    [Display(Name = "Фамилия")]
    [Required(ErrorMessage = "Фамилия обязательна для заполнения")]
    public string Surname { get; set; }

    [Display(Name = "Имя")]
    [Required(ErrorMessage = "Имя обязательно для заполнения")]
    public string Name { get; set; }

    [Display(Name = "Отчество")]
    public string Patronymic { get; set; }

    [EmailAddress]
    [Display(Name = "Эл. почта")]
    public virtual string Email { get; set; }

    [Phone]
    [Display(Name = "Номер телефона")]
    public virtual string PhoneNumber { get; set; }
  }
}
