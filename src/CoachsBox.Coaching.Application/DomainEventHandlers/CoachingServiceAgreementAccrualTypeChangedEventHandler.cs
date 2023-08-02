using System.Threading;
using System.Threading.Tasks;
using CoachsBox.Coaching.Accounting.CoachingServiceAgreementModel;
using CoachsBox.Coaching.Accounting.StudentAccountPostingRuleModel;
using MediatR;

namespace CoachsBox.Coaching.Application.DomainEventHandlers
{
  public class CoachingServiceAgreementAccrualTypeChangedEventHandler : INotificationHandler<CoachingServiceAgreementAccrualTypeChangedEvent>
  {
    private readonly IStudentAccountPostingRuleRepository studentAccountPostingRuleRepository;

    public CoachingServiceAgreementAccrualTypeChangedEventHandler(IStudentAccountPostingRuleRepository studentAccountPostingRuleRepository)
    {
      this.studentAccountPostingRuleRepository = studentAccountPostingRuleRepository;
    }

    public async Task Handle(CoachingServiceAgreementAccrualTypeChangedEvent notification, CancellationToken cancellationToken)
    {
      if (notification == null)
        return;

      // После смены типа начисления удалим более не используемое правило проводки.
      if (notification.PreviousAgreedPostingRule?.PostingRule is StudentAccountPostingRule studentAccountPostingRule)
      {
        await this.studentAccountPostingRuleRepository.DeleteAsync(studentAccountPostingRule);
        await this.studentAccountPostingRuleRepository.SaveAsync();
      }
    }
  }
}
