using System.Collections.Generic;
using System.Linq;
using CoachsBox.Core;
using CoachsBox.Core.Primitives;

namespace CoachsBox.Accounting
{
  public abstract class Account : BaseEntity
  {
    private readonly List<AccountEntry> entries = new List<AccountEntry>();

    /// <summary>
    /// Получить баланс счета.
    /// </summary>
    public Money GetBalance()
    {
      var amount = this.entries.Sum(e => e.Amount.Quantity);
      return Money.CreateRuble(amount);
    }

    // TODO: Можно добавить метод для получения баланса на определенную дату.
    // public Money GetBalance(Date date) { }

    public IReadOnlyCollection<AccountEntry> Entries => this.entries;

    public void AddEntry(AccountEntry entry)
    {
      this.entries.Add(entry);
    }

    protected Account()
    {
      // Требует Entity framework core
    }
  }
}
