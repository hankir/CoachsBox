using System;
using System.Collections.Generic;
using System.Text;
using CoachsBox.Accounting;
using CoachsBox.Core;

namespace CoachsBox.Coaching.Accounting.CoachingAccountingEventModel
{
  public class AccountingEventResultingEntry : ValueObject
  {
    public AccountingEventResultingEntry(AccountEntry accountEntry)
    {
      this.AccountEntry = accountEntry;
      this.AccountEntryId = accountEntry.Id;
    }

    public AccountEntry AccountEntry { get; private set; }

    public int AccountEntryId { get; private set; }

    protected override IEnumerable<object> GetAtomicValues()
    {
      yield return this.AccountEntryId;
    }

    private AccountingEventResultingEntry()
    {
      // Требует Entity framework core
    }
  }
}
