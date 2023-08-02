using System;
using CoachsBox.Accounting;

namespace CoachsBox.Coaching.Accounting.CoachingAccountingEventModel
{
  public abstract class CoachingAccountingEvent : AccountingEvent
  {
    public AccountingEventProcessingState ProcessingState { get; private set; }

    public override void Process()
    {
      var resultingEntries = this.FindRule().Process(this);
      var newProcessingState = this.ProcessingState.SetProcessed(resultingEntries);
      this.Process(newProcessingState);
    }

    protected void Process(AccountingEventProcessingState newProcessingState)
    {
      this.ProcessingState = newProcessingState;
    }

    public CoachingAccountingEvent(
      AccountingEventType eventType,
      DateTime whenOccured,
      DateTime whenNoticed)
      : base(eventType, whenOccured, whenNoticed)
    {
      this.ProcessingState = new AccountingEventProcessingState(this);
    }

    protected CoachingAccountingEvent()
    {
      // Требует Entity framework core
    }
  }
}
