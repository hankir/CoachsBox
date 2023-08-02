using System.ComponentModel.DataAnnotations;
using CoachsBox.WebApp.Pages.Facade.DTO;

namespace CoachsBox.WebApp.Pages.Facade
{
  public class CreateGroupCommand
  {
    [Display(Name = "Филиал")]
    public int BranchId { get; set; }

    [Display(Name = "Имя группы")]
    [Required(ErrorMessage = "Имя группы обязательно для заполнения")]
    public string Name { get; set; }

    [Display(Name = "Программа тренировок")]
    public TrainingProgramDTO Program { get; set; }


    [Display(Name = "Стоимость тренировки")]
    public int TariffId { get; set; }
  }
}