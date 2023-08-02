using System;
using System.Collections.Generic;
using CoachsBox.Accounting;
using CoachsBox.Coaching.Accounting.StudentAccountingEventModel;
using CoachsBox.Core;

namespace CoachsBox.Coaching.Accounting.CoachingServiceAgreementModel
{
  public class AgreedPostingRule : ValueObject
  {
    public AgreedPostingRule(StudentAccountingEventType eventType, PostingRule postingRule)
    {
      this.EventType = eventType;
      this.PostingRule = postingRule ?? throw new ArgumentNullException(nameof(postingRule), "Posting rule can not be null");
      this.PostingRuleId = postingRule.Id;
    }

    public StudentAccountingEventType EventType { get; set; }

    public PostingRule PostingRule { get; set; }

    public int PostingRuleId { get; set; }

    protected override IEnumerable<object> GetAtomicValues()
    {
      yield return this.EventType;
      yield return this.PostingRule;
      yield return this.PostingRuleId;
    }

    private AgreedPostingRule()
    {
      // Требует Entity framework core
    }
  }
}
