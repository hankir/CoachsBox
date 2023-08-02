using MediatR;

namespace CoachsBox.Coaching.Accounting.CoachingServiceAgreementModel
{
  /// <summary>
  /// Событие о смене типа начисления для соглашения о стоимости услуг за тренировки.
  /// </summary>
  public class CoachingServiceAgreementAccrualTypeChangedEvent : INotification
  {
    public CoachingServiceAgreementAccrualTypeChangedEvent(int coachingServiceAgreementId, AgreedPostingRule previousAgreedPostingRule)
    {
      this.CoachingServiceAgreementId = coachingServiceAgreementId;
      this.PreviousAgreedPostingRule = previousAgreedPostingRule;
    }

    /// <summary>
    /// Получить идентификатор соглашения.
    /// </summary>
    public int CoachingServiceAgreementId { get; }

    /// <summary>
    /// Получить предыдущее правило проводки для начисления, связанное с соглашением.
    /// </summary>
    public AgreedPostingRule PreviousAgreedPostingRule { get; }
  }
}
