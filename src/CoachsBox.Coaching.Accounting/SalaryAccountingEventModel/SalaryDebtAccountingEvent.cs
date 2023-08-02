using System;
using System.Linq;
using CoachsBox.Accounting;
using CoachsBox.Coaching.Accounting.SalaryModel;
using CoachsBox.Core.Primitives;

namespace CoachsBox.Coaching.Accounting.SalaryAccountingEventModel
{
  public class SalaryDebtAccountingEvent : SalaryAccountingEvent
  {
    public SalaryDebtAccountingEvent(Salary salary, int coachId, int groupId, int trainingCount, Money trainingCost, DateTime whenOccured, DateTime whenNoticed)
      : base(SalaryAccountingEventType.CoachSalaryDebt, salary, whenOccured, whenNoticed)
    {
      this.CoachId = coachId;
      this.GroupId = groupId;
      this.TrainingCount = trainingCount;
      this.TrainingCost = Money.CreateRuble(trainingCost.Quantity);
    }

    /// <summary>
    /// Получить идентификатор тренера, для которого случилось событие долга.
    /// </summary>
    public int CoachId { get; private set; }

    /// <summary>
    /// Получить идентификатор группы, для которой случилось событие долга.
    /// </summary>
    public int GroupId { get; private set; }

    /// <summary>
    /// Получить кол-во тренировок, зарплата по которым не выплачена.
    /// </summary>
    public Money TrainingCost { get; private set; }

    /// <summary>
    /// Получить стоимость тренировки.
    /// </summary>
    public int TrainingCount { get; private set; }

    /// <summary>
    /// Получить идентификатор зарплаты, в которой обработан долг.
    /// </summary>
    public int? ProcessedInSalaryId { get; private set; }

    /// <summary>
    /// Обработать событие.
    /// </summary>
    /// <param name="salaryId">Зарплата, в которой обработан долг.</param>
    public void Process(int salaryId)
    {
      this.ProcessedInSalaryId = salaryId;
      var newProcessingState = this.ProcessingState.SetProcessed(Enumerable.Empty<AccountEntry>());
      this.Process(newProcessingState);
    }

    public override void Process()
    {
      throw new NotSupportedException($"Process event with type {SalaryAccountingEventType.CoachSalaryDebt} not supported");
    }

    private SalaryDebtAccountingEvent()
    {
      // Требует Entity framework core
    }
  }
}
