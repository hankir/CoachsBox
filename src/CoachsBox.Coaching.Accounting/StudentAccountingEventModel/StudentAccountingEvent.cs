using System;
using CoachsBox.Accounting;
using CoachsBox.Coaching.Accounting.CoachingAccountingEventModel;
using CoachsBox.Coaching.Accounting.CoachingServiceAgreementModel;
using CoachsBox.Coaching.Accounting.StudentAccountModel;
using CoachsBox.Core.Primitives;

namespace CoachsBox.Coaching.Accounting.StudentAccountingEventModel
{
  public abstract class StudentAccountingEvent : CoachingAccountingEvent
  {
    public int GroupId { get; private set; }

    public int AccountId { get; private set; }

    public int ServiceAgreementId { get; private set; }

    public StudentAccount Account { get; private set; }

    public CoachingServiceAgreement ServiceAgreement { get; private set; }

    public abstract string BuildAccountEntryDescription();

    protected override PostingRule FindRule()
    {
      var postingRule = this.ServiceAgreement.GetPostingRule(this.EventType);
      return postingRule ?? throw new InvalidOperationException($"Posting rule not found by event type {this.EventType}");
    }

    protected StudentAccountingEvent(
      AccountingEventType eventType,
      DateTime whenOccured,
      DateTime whenNoticed,
      int groupId,
      StudentAccount account,
      CoachingServiceAgreement serviceAgreement)
      : base(eventType, whenOccured, whenNoticed)
    {
      this.GroupId = groupId;
      this.Account = account;
      this.ServiceAgreement = serviceAgreement;
    }

    protected StudentAccountingEvent(
      AccountingEventType eventType,
      DateTime whenOccured,
      DateTime whenNoticed,
      int groupId,
      int accountId,
      int serviceAgreementId)
      : base(eventType, whenOccured, whenNoticed)
    {
      this.GroupId = groupId;
      this.AccountId = accountId;
      this.ServiceAgreementId = serviceAgreementId;
    }

    protected StudentAccountingEvent()
    {
      // Требует Entity framework core
    }
  }
}
