using System;
using CoachsBox.Core.Primitives;

namespace CoachsBox.Coaching.Accounting.SalaryModel
{
  public class CoachSalaryAllocatedDebtCalculation : CoachSalaryCalculation
  {
    public CoachSalaryAllocatedDebtCalculation(int coachId, int groupId, int debtTrainingCount, Money trainingCost)
      : base(coachId, groupId, debtTrainingCount, trainingCost)
    {
    }

    public CoachSalaryAllocatedDebtCalculation(int coachId, int groupId, int debtTrainingCount, Money trainingCost, Money amountToIssued)
      : base(coachId, groupId, debtTrainingCount, trainingCost, amountToIssued)
    {
    }

    protected override SalaryCalculation CreatePropagatedCalculation(int coachId, int groupId, int debtTrainingCount, Money trainingCost, Money amountToIssued)
    {
      throw new InvalidOperationException($"Can not support propagate allocated debt calculation.");
    }

    private CoachSalaryAllocatedDebtCalculation()
    {
      // Требует Entity framework core
    }
  }
}
