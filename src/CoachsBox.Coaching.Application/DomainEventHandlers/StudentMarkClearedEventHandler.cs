using System.Threading;
using System.Threading.Tasks;
using CoachsBox.Coaching.Accounting.AgreementRegistryEntryModel;
using CoachsBox.Coaching.Accounting.StudentAccountingEventModel;
using CoachsBox.Coaching.Application.Commands;
using CoachsBox.Coaching.AttendanceLogModel;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CoachsBox.Coaching.Application.DomainEventHandlers
{
  public class StudentMarkClearedEventHandler : INotificationHandler<StudentMarkClearedEvent>
  {
    private readonly ILogger<StudentMarkClearedEventHandler> logger;
    private readonly IMediator mediator;
    private readonly IAgreementRegistryEntryRepository agreementRegistry;

    public StudentMarkClearedEventHandler(
      ILogger<StudentMarkClearedEventHandler> logger,
      IMediator mediator,
      IAgreementRegistryEntryRepository agreementRegistry)
    {
      this.logger = logger;
      this.mediator = mediator;
      this.agreementRegistry = agreementRegistry;
    }

    public async Task Handle(StudentMarkClearedEvent notification, CancellationToken cancellationToken)
    {
      if (notification == null)
        return;

      this.logger.LogInformation("Student mark cleared [GroupId:{GroupId};StudentId:{StudentId};Training:{TrainingStart}-{TrainingEnd};IsTrial:{IsTrialTraining};AbscenseReason:{Abscence}]",
        notification.GroupId, notification.StudentId, notification.TrainingStart, notification.TrainingEnd, notification.IsTrialTraining, notification.Absence);

      if (notification.IsTrialTraining)
      {
        await this.mediator.Send(new ReuseTrialTrainingCommand(notification.GroupId, notification.StudentId));
        return;
      }

      var groupAgreementSpecification = new AgreementRegistryEntryByGroupSpecification(notification.GroupId);
      var agreementRegistryEntry = await this.agreementRegistry.GetBySpecAsync(groupAgreementSpecification);
      if (agreementRegistryEntry != null)
      {
        var agreement = agreementRegistryEntry.Agreement;
        if (agreement.AccrualEventType.Equals(StudentAccountingEventType.PersonalTrainingAccrual))
        {
          await this.mediator.Send(new CancelAccrueFeePersonalTrainingCommand(
            notification.GroupId,
            agreement.Id,
            notification.StudentId,
            notification.TrainingStart,
            notification.TrainingEnd));
        }
        else if (agreement.AccrualEventType.Equals(StudentAccountingEventType.Accrual))
        {
          if (!notification.HaveAttendanceInMonth)
          {
            await this.mediator.Send(new CancelAccrueFeeMonthlyTrainingCommand(
              notification.GroupId,
              agreement.Id,
              notification.StudentId,
              notification.TrainingStart.Month,
              notification.TrainingStart.Year));
          }
        }
      }
    }
  }
}
