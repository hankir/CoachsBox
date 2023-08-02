using System;

namespace CoachsBox.WebApp.AppFacade.Accounting.DTO
{
  public class SalaryListItemDTO
  {
    public int Id { get; set; }

    public DateTime PeriodBeginning { get; set; }

    public DateTime PeriodEnding { get; set; }

    public DateTime PaymentsPeriodEnding { get; set; }

    public bool IsPaid { get; set; }

    public string DisplayName { get; set; }

    public decimal TotalAmount { get; set; }

    public decimal TotalAmountToIssued { get; set; }
  }
}
