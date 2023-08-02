namespace CoachsBox.WebApp.AppFacade.Accounting.DTO
{
  public class CoachSalaryCalculationDTO
  {
    /// <summary>
    /// Получить или установить идентификатор расчета.
    /// </summary>
    public string Id { get; set; }

    /// <summary>
    /// Получить идентификатор тренера.
    /// </summary>
    public int CoachId { get; set; }

    /// <summary>
    /// Получить идентификатор группы, которую вел тренер.
    /// </summary>
    public int GroupId { get; set; }

    /// <summary>
    /// Получить имя группы.
    /// </summary>
    public string GroupName { get; set; }

    /// <summary>
    /// Получить расчитаную сумму зарплаты.
    /// </summary>
    public decimal Amount { get; set; }

    /// <summary>
    /// Получить сумму к выдачи.
    /// </summary>
    public decimal AmountToIssued { get; set; }

    /// <summary>
    /// Получить описание расчета.
    /// </summary>
    public string Description { get; set; }

    /// <summary>
    /// Получить кол-во проведенных тренером тренировок.
    /// </summary>
    public int TrainingCount { get; set; }

    /// <summary>
    /// Получить стоимость тренировки.
    /// </summary>
    public decimal TrainingCost { get; set; }

    /// <summary>
    /// Получить количество тренировок, зарплата за которые не будет выплачена.
    /// </summary>
    public int DebtTrainingCount { get; set; }

    /// <summary>
    /// Получить или установить признак того, что это расчет долга.
    /// </summary>
    public bool IsDebt { get; set; }
  }
}
