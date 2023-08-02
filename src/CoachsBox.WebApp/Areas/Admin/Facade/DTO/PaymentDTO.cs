using System;

namespace CoachsBox.WebApp.Areas.Admin.Facade.DTO
{
  public class PaymentDTO
  {
    public int? PaymentId { get; set; }

    public decimal Amount { get; set; }

    public DateTime WhenOccured { get; set; }

    public DateTime WhenNoticed { get; set; }

    public DateTime WhenProcessed { get; set; }

    public bool IsProcessed { get; set; }

    public int StudentId { get; set; }

    public string StudentName { get; internal set; }

    public int GroupId { get; set; }

    public string GroupName { get; internal set; }
  }
}
