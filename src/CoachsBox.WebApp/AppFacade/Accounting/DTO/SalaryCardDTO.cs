using System;
using System.Collections.Generic;

namespace CoachsBox.WebApp.AppFacade.Accounting.DTO
{
  public class SalaryCardDTO
  {
    public int Id { get; set; }

    public string DisplayName { get; set; }

    public DateTime BeginPeriod { get; set; }

    public DateTime EndPeriod { get; set; }

    public DateTime PaymentsEndPeriod { get; set; }

    public decimal TotalAmount { get; set; }

    public decimal TotalAmountToIssued { get; set; }

    public bool IsPaid { get; set; }

    public List<CoachSalaryCalculationGroupDTO> CoachSalaryCalculationsGroups { get; set; }

    public SalaryCardDTO()
    {
      this.CoachSalaryCalculationsGroups = new List<CoachSalaryCalculationGroupDTO>();
    }
  }
}
