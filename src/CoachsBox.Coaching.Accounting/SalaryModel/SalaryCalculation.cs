using System;
using CoachsBox.Core;
using CoachsBox.Core.Primitives;

namespace CoachsBox.Coaching.Accounting.SalaryModel
{
  /// <summary>
  /// Представление базового расчета зарплаты.
  /// </summary>
  public abstract class SalaryCalculation : BaseEntity
  {
    /// <summary>
    /// Получить расчитаную сумму зарплаты.
    /// </summary>
    public Money Amount { get; protected set; }

    /// <summary>
    /// Получить сумму к выдачи.
    /// </summary>
    public Money AmountToIssued { get; protected set; }

    /// <summary>
    /// Получить описание расчета.
    /// </summary>
    public string Description { get; protected set; }

    /// <summary>
    /// Добавить описание расчета.
    /// </summary>
    /// <param name="description">Описание расчета.</param>
    public void Describe(string description)
    {
      if (string.IsNullOrWhiteSpace(description))
        throw new ArgumentException(nameof(description));

      this.Description = description;
    }

    public bool HasDebt()
    {
      return this.Amount.Quantity > this.AmountToIssued.Quantity;
    }

    /// <summary>
    /// Пересчитать сумму зарплаты исходя из прогнозируемого баланса зарплаты.
    /// </summary>
    /// <param name="balance">Прогнозируемый баланс.</param>
    public abstract SalaryCalculation PropagateSalary(Money balance);

    protected SalaryCalculation()
    {
      this.Amount = Money.CreateRuble(0);
      this.AmountToIssued = Money.CreateRuble(0);
    }
  }
}
