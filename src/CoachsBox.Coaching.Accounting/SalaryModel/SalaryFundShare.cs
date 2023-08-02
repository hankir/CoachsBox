using System.Collections.Generic;
using CoachsBox.Core;
using CoachsBox.Core.Primitives;

namespace CoachsBox.Coaching.Accounting.SalaryModel
{
  /// <summary>
  /// Доля фонда зарплаты.
  /// </summary>
  public class SalaryFundShare : ValueObject
  {
    /// <summary>
    /// Создать долю фонда зарплаты.
    /// </summary>
    /// <param name="groupId">Идентификатор группы.</param>
    /// <param name="balance">Баланс группы.</param>
    public SalaryFundShare(int groupId, Money balance)
    {
      this.GroupId = groupId;
      this.Balance = balance;
    }

    /// <summary>
    /// Получить идентификатор группы.
    /// </summary>
    public int GroupId { get; private set; }

    /// <summary>
    /// Получить баланс группы.
    /// </summary>
    public Money Balance { get; private set; }

    protected override IEnumerable<object> GetAtomicValues()
    {
      yield return this.GroupId;
      yield return this.Balance;
    }
  }
}
