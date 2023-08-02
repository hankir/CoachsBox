using System.Threading;
using System.Threading.Tasks;
using CoachsBox.Coaching.Accounting.CoachingServiceAgreementModel;
using CoachsBox.Coaching.Accounting.StudentAccountingEventModel;
using CoachsBox.Coaching.Accounting.StudentAccountModel;
using CoachsBox.Core;
using CoachsBox.Core.Primitives;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CoachsBox.Coaching.Application.Commands
{
  public class AccrueFeePersonalTrainingCommandHandler : IRequestHandler<AccrueFeePersonalTrainingCommand, bool>
  {
    private readonly ILogger<AccrueFeePersonalTrainingCommandHandler> logger;
    private readonly IStudentAccountingEventRepository eventRepository;
    private readonly ICoachingServiceAgreementRepository agreementRepository;
    private readonly IStudentAccountRepository studentAccountRepository;

    public AccrueFeePersonalTrainingCommandHandler(
      ILogger<AccrueFeePersonalTrainingCommandHandler> logger,
      IStudentAccountingEventRepository eventRepository,
      ICoachingServiceAgreementRepository agreementRepository,
      IStudentAccountRepository studentAccountRepository)
    {
      this.logger = logger;
      this.eventRepository = eventRepository;
      this.agreementRepository = agreementRepository;
      this.studentAccountRepository = studentAccountRepository;
    }

    public async Task<bool> Handle(AccrueFeePersonalTrainingCommand request, CancellationToken cancellationToken)
    {
      if (request == null)
        return false;

      var agreementId = request.AgreementId;
      var agreement = await this.agreementRepository.GetByIdAsync(agreementId);
      if (agreement == null)
        return false;

      var groupId = request.GroupId;
      var studentId = request.StudentId;
      var whenOccured = request.TrainingStart;

      var studentAccountSpecification = new StudentAccountSpecification(studentId);
      var account = await this.studentAccountRepository.GetBySpecAsync(studentAccountSpecification);
      if (account == null)
      {
        logger.LogInformation("Skip accrual [GroupId:{GroupId};AgreementId:{AgreementId};Period:{Date};StudentId:{StudentId}]. Account not found.",
          groupId, agreementId, whenOccured.ToShortDateString(), studentId);
        return false;
      }

      var trainingDate = Date.Create(whenOccured);
      var trainingStart = TimeOfDay.Create(request.TrainingStart.TimeOfDay);
      var trainingEnd = TimeOfDay.Create(request.TrainingEnd.TimeOfDay);
      var whenNoticed = Watch.Now.DateTime;

      var personalTrainingAccrualEventSpecification = new PersonalTrainingAccrualEventSpecification(
        groupId,
        agreementId,
        studentId,
        request.TrainingStart,
        request.TrainingEnd);

      if (await this.eventRepository.CountAsync(personalTrainingAccrualEventSpecification) > 0)
      {
        logger.LogWarning("Skip accrual [GroupId:{GroupId};AgreementId:{AgreementId};Period:{Date} {Start}-{End};StudentId:{StudentId}]. Accrual already exist.",
          groupId, agreementId, whenOccured.ToShortDateString(), request.TrainingStart.TimeOfDay, request.TrainingEnd.TimeOfDay, studentId);
        return false;
      }

      var accruralEvent = new PersonalTrainingAccrualAccountingEvent(
        trainingDate,
        trainingStart,
        trainingEnd,
        whenOccured,
        whenNoticed,
        groupId,
        account,
        agreement);

      accruralEvent.Process();

      logger.LogInformation("Process accrual event. Account entries will be created. [GroupId:{GroupId};AgreementId:{AgreementId};Period:{Date} {Start}-{End};StudentId:{StudentId}].",
        groupId, agreementId, whenOccured.ToShortDateString(), request.TrainingStart.TimeOfDay, request.TrainingEnd.TimeOfDay, studentId);

      await this.eventRepository.AddAsync(accruralEvent);
      await this.eventRepository.SaveAsync();

      logger.LogInformation("Added accrual [GroupId:{GroupId};AgreementId:{AgreementId};Period:{Date} {Start}-{End};StudentId:{StudentId}].",
        groupId, agreementId, whenOccured.ToShortDateString(), request.TrainingStart.TimeOfDay, request.TrainingEnd.TimeOfDay, studentId);
      return true;
    }
  }
}
