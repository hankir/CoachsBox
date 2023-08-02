using System;
using System.Collections.Generic;
using System.Linq;
using CoachsBox.Core.Primitives;

namespace CoachsBox.Coaching.Accounting.SalaryModel
{
  /// <summary>
  /// Фонд зарплаты, состоящий из балансов счетов групп.
  /// </summary>
  public class SalaryFund
  {
    private readonly Dictionary<int, SalaryFundShare> salaryFundShares = new Dictionary<int, SalaryFundShare>();

    /// <summary>
    /// Создать экземпляр фонд зарплаты на основе долей групп.
    /// </summary>
    /// <param name="salaryFundShares">Доли групп, из которых состоит фонд зарплаты.</param>
    public SalaryFund(HashSet<SalaryFundShare> salaryFundShares)
    {
      foreach (var share in salaryFundShares)
        this.Add(share.GroupId, share.Balance);
    }

    /// <summary>
    /// Получить доли фонда зарплаты по группам.
    /// </summary>
    public IReadOnlyCollection<SalaryFundShare> Shares => this.salaryFundShares.Values;

    /// <summary>
    /// Расчитать разницу фондов зарплат.
    /// </summary>
    /// <param name="salaryFund">Вычитаемый фонд зарплаты.</param>
    /// <returns>Разница фондов зарплат.</returns>
    public SalaryFund Minus(SalaryFund salaryFund)
    {
      var result = new SalaryFund();
      foreach (var share in this.salaryFundShares.Values)
      {
        var otherShare = salaryFund.GetShare(share.GroupId);
        if (otherShare != null)
        {
          var balance = share.Balance.Substract(otherShare.Balance);
          result.Add(share.GroupId, balance);
        }
      }
      return result;
    }

    /// <summary>
    /// Вычесть долю зарплаты.
    /// </summary>
    /// <param name="groupId">Идентификатор группы.</param>
    /// <param name="balance">Сумма вычитания.</param>
    public void Minus(int groupId, Money balance)
    {
      if (this.salaryFundShares.TryGetValue(groupId, out SalaryFundShare salaryFundShare))
        this.salaryFundShares[groupId] = new SalaryFundShare(groupId, salaryFundShare.Balance.Substract(balance));
      else
        this.salaryFundShares.Add(groupId, new SalaryFundShare(groupId, balance.Negate()));
    }

    /// <summary>
    /// Добавить долю зарплаты.
    /// </summary>
    /// <param name="groupId">Идентификатор группы.</param>
    /// <param name="balance">Добавляемая сумма.</param>
    public void Add(int groupId, Money balance)
    {
      var balanceQuantity = Math.Max(balance.Quantity, 0);
      if (this.salaryFundShares.TryGetValue(groupId, out SalaryFundShare salaryFundShare))
        this.salaryFundShares[groupId] = new SalaryFundShare(groupId, salaryFundShare.Balance.Add(Money.CreateRuble(balanceQuantity)));
      else
        this.salaryFundShares.Add(groupId, new SalaryFundShare(groupId, Money.CreateRuble(balanceQuantity)));
    }

    /// <summary>
    /// Получить долю по группе.
    /// </summary>
    /// <param name="groupId">Идентификатор группы.</param>
    /// <returns>Доля группы.</returns>
    public SalaryFundShare GetShare(int groupId)
    {
      SalaryFundShare result;
      this.salaryFundShares.TryGetValue(groupId, out result);
      return result;
    }

    /// <summary>
    /// Проверить есть ли доли с отрицательным балансом.
    /// </summary>
    /// <returns>True, если есть доли с отрицательным балансом, иначе - false.</returns>
    public bool HasNegativeSharesBalance()
    {
      return this.Shares.Any(share => share.Balance.IsNegative());
    }

    /// <summary>
    /// Получить общую сумму фонда зарплаты.
    /// </summary>
    /// <returns>Общая сумма фонда.</returns>
    public Money Total()
    {
      var result = Money.CreateRuble(0);
      foreach (var share in this.Shares)
        result = result.Add(Money.CreateRuble(share.Balance.Quantity));

      return result;
    }

    public SalaryFund()
    {
    }
  }
}
