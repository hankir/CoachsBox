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
  public class StudentMarkedEventHandler : INotificationHandler<StudentMarkedEvent>
  {
    private readonly ILogger<StudentMarkedEventHandler> logger;
    private readonly IMediator mediator;
    private readonly IAgreementRegistryEntryRepository agreementRegistry;
    private readonly IAccrualService accrualService;

    public StudentMarkedEventHandler(
      ILogger<StudentMarkedEventHandler> logger,
      IMediator mediator,
      IAgreementRegistryEntryRepository agreementRegistry,
      IAccrualService accrualService)
    {
      this.logger = logger;
      this.mediator = mediator;
      this.agreementRegistry = agreementRegistry;
      this.accrualService = accrualService;
    }

    public async Task Handle(StudentMarkedEvent notification, CancellationToken cancellationToken)
    {
      if (notification == null)
        return;

      this.logger.LogInformation("Student marked [GroupId:{GroupId};StudentId:{StudentId};CoachId:{CoachId};Training:{TrainingStart}-{TrainingEnd};IsTrial:{IsTrialTraining};AbscenseReason:{Abscence}]",
        notification.GroupId, notification.StudentId, notification.CoachId, notification.TrainingStart, notification.TrainingEnd, notification.IsTrialTraining, notification.AbsenceReason);

      if (notification.IsTrialTraining)
      {
        await this.mediator.Send(new UseTrialTrainingCommand(notification.GroupId, notification.StudentId));
        return;
      }

      var absence = Absence.GetAbsence(notification.AbsenceReason);
      // Начисляем только для подотчетной отметки.
      if (this.accrualService.IsAttendanceLogEntryAccountable(notification.IsTrialTraining, absence))
      {
        var groupAgreementSpecification = new AgreementRegistryEntryByGroupSpecification(notification.GroupId);
        var agreementRegistryEntry = await this.agreementRegistry.GetBySpecAsync(groupAgreementSpecification);
        if (agreementRegistryEntry != null)
        {
          var agreement = agreementRegistryEntry.Agreement;
          if (agreement.AccrualEventType.Equals(StudentAccountingEventType.PersonalTrainingAccrual))
          {
            await this.mediator.Send(new AccrueFeePersonalTrainingCommand(
              notification.GroupId,
              agreement.Id,
              notification.StudentId,
              notification.TrainingStart,
              notification.TrainingEnd));
          }
          else if (agreement.AccrualEventType.Equals(StudentAccountingEventType.Accrual))
          {
            await this.mediator.Send(new AccrueFeeMonthlyTrainingCommand(
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
