using System;
using System.Collections.Generic;
using CoachsBox.Accounting;
using CoachsBox.Core;

namespace CoachsBox.Coaching.Accounting.CoachingAccountingEventModel
{
  public class AccountingEventProcessingState : ValueObject
  {
    private readonly List<AccountingEventResultingEntry> resultingEntries = new List<AccountingEventResultingEntry>();

    public AccountingEventProcessingState(CoachingAccountingEvent accountingEvent)
    {
      this.AccountingEvent = accountingEvent;
      this.AccountingEventId = accountingEvent.Id;
    }

    public int AccountingEventId { get; private set; }

    public CoachingAccountingEvent AccountingEvent { get; private set; }

    public bool IsProcessed { get; private set; }

    public DateTime WhenProcessed { get; private set; }

    public IReadOnlyCollection<AccountingEventResultingEntry> ResultingEntries => this.resultingEntries;

    public AccountingEventProcessingState SetProcessed(IEnumerable<AccountEntry> resultingAccountEntries)
    {
      var processedAccountingEventState = new AccountingEventProcessingState(this.AccountingEvent)
      {
        IsProcessed = true,
        WhenProcessed = Watch.Now.DateTime
      };
      foreach (var resultingEntry in resultingAccountEntries)
      {
        processedAccountingEventState.resultingEntries.Add(new AccountingEventResultingEntry(resultingEntry));
      }
      return processedAccountingEventState;
    }

    protected override IEnumerable<object> GetAtomicValues()
    {
      yield return this.WhenProcessed;
      foreach (var entry in this.ResultingEntries)
        yield return entry;
    }

    protected AccountingEventProcessingState()
    {
      // Требует Entity framework core.
    }
  }
}
