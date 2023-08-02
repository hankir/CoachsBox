using System;
using System.ComponentModel.DataAnnotations;

namespace CoachsBox.WebApp.AppFacade.Documents.DTO
{
  public class MedicalCertificateDTO : BaseStudentDocumentDTO
  {
    /// <summary>
    /// Получить дату выдачи справки.
    /// </summary>
    [Required(ErrorMessage = "Дата выдачи справки обязательна для заполнения")]
    public DateTime? Date { get; set; }

    /// <summary>
    /// Получить дату завершения действия справки.
    /// </summary>
    public DateTime? EndDate { get; set; }

    /// <summary>
    /// Получить признак того, что разрешены тренировки.
    /// </summary>
    public bool AllowTraining { get; set; }

    /// <summary>
    /// Получить признак того, что разрешены соревнования.
    /// </summary>
    public bool AllowCompetition { get; set; }
  }
}
