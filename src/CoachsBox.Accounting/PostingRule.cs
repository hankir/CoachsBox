using System.Collections.Generic;
using CoachsBox.Core;
using CoachsBox.Core.Primitives;

namespace CoachsBox.Accounting
{
  public abstract class PostingRule : BaseEntity
  {
    public AccountEntryType EntryType { get; private set; }

    public PostingRule(AccountEntryType entryType)
    {
      this.EntryType = entryType;
    }

    public IEnumerable<AccountEntry> Process(AccountingEvent accountingEvent)
    {
      var amount = this.CalculateAmout(accountingEvent);
      return this.MakeEntries(accountingEvent, amount);
    }

    protected abstract Money CalculateAmout(AccountingEvent accountingEvent);

    protected abstract IEnumerable<AccountEntry> MakeEntries(AccountingEvent accountingEvent, Money amount);

    protected PostingRule()
    {
      // Требует Entity framework core
    }
  }
}
