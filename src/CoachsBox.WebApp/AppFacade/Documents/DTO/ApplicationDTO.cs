using System;
using System.ComponentModel.DataAnnotations;

namespace CoachsBox.WebApp.AppFacade.Documents.DTO
{
  public class ApplicationDTO : BaseStudentDocumentDTO
  {
    [Required(ErrorMessage = "Дата обязательна для заполнения")]
    public DateTime? Date { get; set; }
  }
}
