using System;
using CoachsBox.Coaching.Accounting.GroupAccountModel;
using CoachsBox.Coaching.Accounting.SalaryAccountingEventModel;
using CoachsBox.Core;
using CoachsBox.Core.Primitives;

namespace CoachsBox.Coaching.Accounting.SalaryModel
{
  /// <summary>
  /// Расчет зарплаты тренера.
  /// </summary>
  public class CoachSalaryCalculation : SalaryCalculation
  {
    public CoachSalaryCalculation(int coachId, int groupId, int trainingCount, Money trainingCost)
    {
      if (trainingCount < 0)
        throw new ArgumentException($"Training count can not be less than zero", nameof(trainingCount));

      if (trainingCost.IsNegative())
        throw new ArgumentException($"Training cost can not be less than zero", nameof(trainingCost));

      this.CoachId = coachId;
      this.GroupId = groupId;
      this.TrainingCost = Money.CreateRuble(trainingCost.Quantity);
      this.TrainingCount = trainingCount;
      this.Amount = this.CalculateAmount();
    }

    public CoachSalaryCalculation(int coachId, int groupId, int trainingCount, Money trainingCost, Money ammountToIssued)
      : this(coachId, groupId, trainingCount, trainingCost)
    {
      if (ammountToIssued.IsNegative())
        throw new ArgumentException($"Amount to issued can not be less than zero", nameof(ammountToIssued));

      if (ammountToIssued.Quantity > this.Amount.Quantity)
        throw new ArgumentException($"Amount to issued can not be greater than calculated amount", nameof(ammountToIssued));

      this.AmountToIssued = ammountToIssued;
    }

    /// <summary>
    /// Получить идентификатор тренера.
    /// </summary>
    public int CoachId { get; private set; }

    /// <summary>
    /// Получить идентификатор группы, которую вел тренер.
    /// </summary>
    public int GroupId { get; private set; }

    /// <summary>
    /// Получить кол-во проведенных тренером тренировок.
    /// </summary>
    public int TrainingCount { get; private set; }

    /// <summary>
    /// Получить стоимость тренировки.
    /// </summary>
    public Money TrainingCost { get; private set; }

    /// <summary>
    /// Посчитать количество оплаченых тренировок.
    /// </summary>
    /// <returns>Количество оплаченных тренировок.</returns>
    public int CountPaidTrainings()
    {
      return (int)decimal.Floor(decimal.Divide(this.AmountToIssued.Quantity, this.TrainingCost.Quantity));
    }

    /// <summary>
    /// Посчитать количество неоплаченых тренировок
    /// </summary>
    /// <returns>Неоплаченные тренировки.</returns>
    public int CountDebtTrainings()
    {
      var paidTrainingCount = this.CountPaidTrainings();
      return this.TrainingCount - paidTrainingCount;
    }

    public override SalaryCalculation PropagateSalary(Money balance)
    {
      if (balance.IsNegative())
        throw new InvalidOperationException($"Cannot propagate salary for negative balance");

      var estimatedBalance = balance.Substract(this.Amount);
      // На балансе недостаточно денег к выдаче зарплаты.
      if (estimatedBalance.IsNegative())
      {
        var paidTrainingCount = (int)decimal.Floor(decimal.Divide(balance.Quantity, this.TrainingCost.Quantity));
        var estimatedQuantity = decimal.Multiply(paidTrainingCount, this.TrainingCost.Quantity);
        return this.CreatePropagatedCalculation(
          this.CoachId,
          this.GroupId,
          this.TrainingCount,
          Money.CreateRuble(this.TrainingCost.Quantity),
          Money.CreateRuble(estimatedQuantity));
      }

      return this.CreatePropagatedCalculation(
        this.CoachId,
        this.GroupId,
        this.TrainingCount,
        Money.CreateRuble(this.TrainingCost.Quantity),
        Money.CreateRuble(this.Amount.Quantity));
    }

    protected virtual SalaryCalculation CreatePropagatedCalculation(int coachId, int groupId, int trainingCount, Money trainingCost, Money amountToIssued)
    {
      return new CoachSalaryCalculation(
          coachId,
          groupId,
          trainingCount,
          trainingCost,
          amountToIssued);
    }

    public SalaryPaymentAccountingEvent MakeSalaryPaymentEvent(Salary salary, GroupAccount groupAccount, DateTime whenNoticed)
    {
      return new SalaryPaymentAccountingEvent(
        salary,
        groupAccount,
        this.Id,
        salary.PaymentsPeriodEnding.ToDateTime().Value,
        whenNoticed);
    }

    public SalaryDebtAccountingEvent MakeSalaryDebtEvent(Salary salary, DateTime whenNoticed)
    {
      if (!this.HasDebt())
        throw new InvalidOperationException("Calculation has not debt");

      return new SalaryDebtAccountingEvent(
        salary,
        this.CoachId,
        this.GroupId,
        this.CountDebtTrainings(),
        this.TrainingCost,
        salary.PaymentsPeriodEnding.ToDateTime().Value,
        whenNoticed);
    }

    public SalaryDebtAccountingEvent AllocateDebt(Salary salary, int debtTrainingCounts, DateTime whenNoticed)
    {
      if (debtTrainingCounts == 0)
        return null;

      if (debtTrainingCounts < 0)
        throw new InvalidOperationException($"Debt training count should not be negative");

      if (debtTrainingCounts > this.TrainingCount)
        throw new InvalidOperationException($"Can not allocate debt training count greather than training count");

      if (this.AmountToIssued.Quantity > 0)
        throw new InvalidOperationException($"Can not allocate debt training count for propagated calculation");

      this.TrainingCount -= debtTrainingCounts;
      this.Amount = this.CalculateAmount();

      return new SalaryDebtAccountingEvent(
        salary,
        this.CoachId,
        this.GroupId,
        debtTrainingCounts,
        this.TrainingCost,
        salary.PaymentsPeriodEnding.ToDateTime().Value,
        whenNoticed);
    }

    private Money CalculateAmount()
    {
      return Money.CreateRuble(decimal.Multiply(this.TrainingCount, this.TrainingCost.Quantity));
    }

    protected CoachSalaryCalculation()
    {
      // Требует Entity framework core
    }
  }
}