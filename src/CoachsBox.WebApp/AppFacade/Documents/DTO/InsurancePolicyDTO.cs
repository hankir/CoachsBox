using System;
using System.ComponentModel.DataAnnotations;

namespace CoachsBox.WebApp.AppFacade.Documents.DTO
{
  public class InsurancePolicyDTO : BaseStudentDocumentDTO
  {
    /// <summary>
    /// Получить номер страхового полиса.
    /// </summary>
    [Required(AllowEmptyStrings = false, ErrorMessage = "Номер полиса обязателен для заполнения")]
    public string Number { get; set; }

    /// <summary>
    /// Получить дату начала действия полиса.
    /// </summary>
    [Required(ErrorMessage = "Дата начала действия полиса обязательна для заполнения.")]
    public DateTime? Date { get; set; }

    /// <summary>
    /// Получить дату завершения действия полиса.
    /// </summary>
    [Required(ErrorMessage = "Дата окончания действия полиса обязательна для заполнения.")]
    public DateTime? EndDate { get; set; }
  }
}
