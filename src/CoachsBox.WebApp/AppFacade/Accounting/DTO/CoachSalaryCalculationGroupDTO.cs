namespace CoachsBox.WebApp.AppFacade.Accounting.DTO
{
  public class CoachSalaryCalculationGroupDTO
  {
    /// <summary>
    /// Получить идентификатора тренера.
    /// </summary>
    public int CoachId { get; set; }

    /// <summary>
    /// Получить или установить полное имя тренера.
    /// </summary>
    public string CoachFullName { get; set; }

    /// <summary>
    /// Получить или установить начисленную сумму зарплаты тренера.
    /// </summary>
    public decimal TotalAmount { get; set; }

    /// <summary>
    /// Получить или установить полную сумму зарплаты к выдаче.
    /// </summary>
    public decimal TotalToIssued { get; set; }
  }
}
