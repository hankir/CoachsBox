using System;
using CoachsBox.Accounting;
using CoachsBox.Coaching.Accounting.GroupAccountEntryModel;
using CoachsBox.Core.Primitives;

namespace CoachsBox.Coaching.Accounting.GroupAccountModel
{
  public class GroupAccount : Account
  {
    public GroupAccount(int groupId)
    {
      if (groupId <= 0)
        throw new ArgumentException("Student id should be greater than zero", nameof(groupId));

      this.GroupId = groupId;
    }

    /// <summary>
    /// Получить идентификатор группы.
    /// </summary>
    public int GroupId { get; private set; }

    /// <summary>
    /// Добавить запись счета о депозите.
    /// </summary>
    /// <param name="amount">Сумма депозита.</param>
    /// <param name="whenOccured">Дата поступления депозита.</param>
    /// <returns>Созданная запись счета.</returns>
    public AccountEntry Deposit(Money amount, DateTime whenOccured)
    {
      var groupAccountEntry = new GroupAccountEntry(GroupAccountEntryType.Deposit, Money.CreateRuble(amount.Quantity), whenOccured);
      this.AddEntry(groupAccountEntry);
      return groupAccountEntry;
    }

    /// <summary>
    /// Добавить запись счета о списании.
    /// </summary>
    /// <param name="amount">Сумма списания.</param>
    /// <param name="whenOccured">Дата списания.</param>
    /// <returns>Созданная запись счета.</returns>
    public AccountEntry Withdraw(Money amount, DateTime whenOccured)
    {
      var groupAccountEntry = new GroupAccountEntry(GroupAccountEntryType.Withdraw, Money.CreateRuble(amount.Quantity), whenOccured);
      this.AddEntry(groupAccountEntry);
      return groupAccountEntry;
    }

    private GroupAccount()
    {
      // Требует Entity framework core
    }
  }
}
