using System;
using System.ComponentModel.DataAnnotations;

namespace CoachsBox.WebApp.AppFacade.Documents.DTO
{
  public class ContractDTO : BaseStudentDocumentDTO
  {
    [Required(AllowEmptyStrings = false, ErrorMessage = "Номер обязателен для заполнения")]
    public string Number { get; set; }

    [Required(ErrorMessage = "Дата обязательна для заполнения")]
    public DateTime? Date { get; set; }
  }
}
