using System;
using System.Collections.Generic;
using System.Linq;
using CoachsBox.Accounting;
using CoachsBox.Coaching.Accounting.StudentAccountingEventModel;
using CoachsBox.Coaching.Accounting.StudentAccountPostingRuleModel;
using CoachsBox.Core;
using CoachsBox.Core.Primitives;

namespace CoachsBox.Coaching.Accounting.CoachingServiceAgreementModel
{
  /// <summary>
  /// Соглашение о стоимости услуги за тренировку.
  /// </summary>
  public class CoachingServiceAgreement : BaseEntity
  {
    private List<AgreedPostingRule> postingRules = new List<AgreedPostingRule>();

    public CoachingServiceAgreement(Money rate, StudentAccountingEventType accrualEventType, string description)
    {
      if (rate.IsNegative())
        throw new ArgumentException($"Rate can not be less than zero", nameof(rate));

      this.Rate = rate;
      this.AccrualEventType = accrualEventType;
      this.Description = description;

      var eventType = ValueObject.GetAll<StudentAccountingEventType>().Where(e => e.Equals(accrualEventType)).Single();
      var accrualPostingRule = this.CreatePostingRule(accrualEventType);
      this.AddPostingRule(eventType, accrualPostingRule);

      var paymentPostingRule = this.CreatePostingRule(StudentAccountingEventType.Payment);
      this.AddPostingRule(StudentAccountingEventType.Payment, paymentPostingRule);
    }

    /// <summary>
    /// Получить стоимость услуги.
    /// </summary>
    public Money Rate { get; private set; }

    /// <summary>
    /// Получить тип события начислений по соглашению.
    /// </summary>
    public StudentAccountingEventType AccrualEventType { get; private set; }

    /// <summary>
    /// Получить описание соглашения.
    /// </summary>
    public string Description { get; private set; }

    /// <summary>
    /// Получить правила проводки.
    /// </summary>
    public IReadOnlyCollection<AgreedPostingRule> PostingRules => this.postingRules;

    /// <summary>
    /// Добавить правило проводки.
    /// </summary>
    /// <param name="eventType">Событие.</param>
    /// <param name="postingRule">Правило.</param>
    public void AddPostingRule(StudentAccountingEventType eventType, PostingRule postingRule)
    {
      var establishedPostingRule = new AgreedPostingRule(eventType, postingRule);
      if (!this.postingRules.Any(p => p.EventType.Equals(eventType)))
        this.postingRules.Add(establishedPostingRule);
    }

    /// <summary>
    /// Получить правило проводки.
    /// </summary>
    /// <param name="eventType">Тип события.</param>
    /// <returns>Правило проводки или null, если соответствующего правила на нашлось.</returns>
    public PostingRule GetPostingRule(AccountingEventType eventType)
    {
      return this.postingRules.SingleOrDefault(r => r.EventType.Equals(eventType))?.PostingRule;
    }

    public void ChangeDescription(string description)
    {
      this.Description = description;
    }

    public void ChangeAccrualType(StudentAccountingEventType coachingAccountingEventType)
    {
      AgreedPostingRule existPostingRule = null;
      foreach (var agreedPostingRule in this.PostingRules)
      {
        if (agreedPostingRule.EventType.Equals(this.AccrualEventType))
          existPostingRule = agreedPostingRule;
      }
      if (existPostingRule != null)
        this.postingRules.Remove(existPostingRule);

      this.AccrualEventType = coachingAccountingEventType;
      var accrualPostingRule = this.CreatePostingRule(coachingAccountingEventType);
      var eventType = ValueObject.GetAll<StudentAccountingEventType>().Where(e => e.Equals(coachingAccountingEventType)).Single();

      this.AddPostingRule(eventType, accrualPostingRule);
      this.AddDomainEvent(new CoachingServiceAgreementAccrualTypeChangedEvent(this.Id, existPostingRule));
    }

    public void ChangeRate(Money money)
    {
      this.Rate = money;
    }

    private StudentAccountPostingRule CreatePostingRule(StudentAccountingEventType accrualEventType)
    {
      var accrualPostingRule = StudentAccountPostingRule.CreateFromEventType(accrualEventType);
      return accrualPostingRule ?? throw new InvalidOperationException($"Posting rule for event type ({accrualEventType.Name}) not found");
    }

    private CoachingServiceAgreement()
    {
      // Требует Entity framework core
    }
  }
}
