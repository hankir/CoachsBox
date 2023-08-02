using System;
using CoachsBox.Core;

namespace CoachsBox.Accounting
{
  public abstract class AccountingEvent : BaseEntity
  {
    public AccountingEvent(AccountingEventType eventType, DateTime whenOccured, DateTime whenNoticed)
    {
      this.EventType = eventType;
      this.WhenOccured = whenOccured;
      this.WhenNoticed = whenNoticed;
    }

    public AccountingEventType EventType { get; private set; }

    public DateTime WhenOccured { get; private set; }

    public DateTime WhenNoticed { get; private set; }

    public abstract void Process();

    protected abstract PostingRule FindRule();

    protected AccountingEvent()
    {
      // Требует Entity framework core
    }
  }
}
