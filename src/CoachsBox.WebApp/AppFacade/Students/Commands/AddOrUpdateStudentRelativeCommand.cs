using System;
using System.ComponentModel.DataAnnotations;
using CoachsBox.WebApp.AppFacade.Primitives.Commands;

namespace CoachsBox.WebApp.AppFacade.Students.Commands
{
  public class AddOrUpdateStudentRelativeCommand : CreatePersonCommand
  {
    public int StudentId { get; set; }

    [Display(Name = "Родство")]
    [Required(ErrorMessage = "Родство является обязательным для заполнения")]
    public string Relationship { get; set; }

    [Display(Name = "Дата рождения")]
    [DisplayFormat(DataFormatString = "{0:dd-MMMM-yyyy}")]
    new public DateTime? Birthdate { get; set; }

    [Display(Name = "Пол")]
    new public string Gender { get; set; }
  }
}
