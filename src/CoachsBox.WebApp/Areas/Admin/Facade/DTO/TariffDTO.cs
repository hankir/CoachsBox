using System.ComponentModel.DataAnnotations;

namespace CoachsBox.WebApp.Areas.Admin.Facade.DTO
{
  public class TariffDTO
  {
    public static readonly TariffDTO Free = new TariffDTO()
    {
      Description = "Бесплатно",
      TrainingRate = 0,
      AgreementId = -1
    };

    public int AgreementId { get; set; }

    [Display(Name = "Описание")]
    public string Description { get; set; }

    [Display(Name = "Стоимость тренировки")]
    public decimal TrainingRate { get; set; }

    [Display(Name = "Способ начислений")]
    public AccrualTypeDTO AccrualType { get; set; }
  }
}
