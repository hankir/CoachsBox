using System;
using System.ComponentModel.DataAnnotations;
using CoachsBox.WebApp.AppFacade.Primitives.DTO;

namespace CoachsBox.WebApp.AppFacade.Primitives.Commands
{
  public abstract class CreatePersonCommand : CreatePersonCommandBase
  {
    [Display(Name = "Адрес")]
    public AddressDTO Address { get; set; }

    [Display(Name = "Дата рождения")]
    [DisplayFormat(DataFormatString = "{0:dd-MMMM-yyyy}")]
    [Required(ErrorMessage = "Дата рождения обязателена для заполнения")]
    public DateTime? Birthdate { get; set; }

    [Display(Name = "Пол")]
    [Required(ErrorMessage = "Пол обязателен для заполнения")]
    public string Gender { get; set; }
  }
}
