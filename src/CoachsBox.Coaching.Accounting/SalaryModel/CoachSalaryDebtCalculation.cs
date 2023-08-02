using CoachsBox.Coaching.Accounting.SalaryAccountingEventModel;
using CoachsBox.Core.Primitives;

namespace CoachsBox.Coaching.Accounting.SalaryModel
{
  public class CoachSalaryDebtCalculation : CoachSalaryCalculation
  {
    public CoachSalaryDebtCalculation(SalaryDebtAccountingEvent salaryDebtEvent)
      : base(salaryDebtEvent.CoachId, salaryDebtEvent.GroupId, salaryDebtEvent.TrainingCount, salaryDebtEvent.TrainingCost)
    {
      this.SalaryDebtAccountingEventId = salaryDebtEvent.Id;
    }

    public CoachSalaryDebtCalculation(int salaryDebtEventId, int coachId, int groupId, int trainingCount, Money trainingCost, Money amountToIssued)
      : base(coachId, groupId, trainingCount, trainingCost, amountToIssued)
    {
      this.SalaryDebtAccountingEventId = salaryDebtEventId;
    }

    /// <summary>
    /// Получить идентификатор события долга по зарплате.
    /// </summary>
    public int SalaryDebtAccountingEventId { get; private set; }

    protected override SalaryCalculation CreatePropagatedCalculation(int coachId, int groupId, int trainingCount, Money trainingCost, Money amountToIssued)
    {
      return new CoachSalaryDebtCalculation(
        this.SalaryDebtAccountingEventId,
        this.CoachId,
        this.GroupId,
        trainingCount,
        trainingCost,
        amountToIssued);
    }

    private CoachSalaryDebtCalculation()
    {
      // Требует Entity framework core
    }
  }
}
