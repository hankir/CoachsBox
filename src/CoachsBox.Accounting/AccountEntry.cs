using System;
using CoachsBox.Core;
using CoachsBox.Core.Primitives;

namespace CoachsBox.Accounting
{
  public abstract class AccountEntry : BaseEntity
  {
    public AccountEntry(AccountEntryType entryType, Money amount, DateTime date)
    {
      this.EntryType = entryType ?? throw new ArgumentNullException(nameof(entryType));
      this.Amount = amount ?? throw new ArgumentNullException(nameof(amount));
      this.Date = date;
    }

    public AccountEntryType EntryType { get; private set; }

    public Money Amount { get; private set; }

    public DateTime Date { get; private set; }

    protected AccountEntry()
    {
      // Требует Entity framework core
    }
  }
}
