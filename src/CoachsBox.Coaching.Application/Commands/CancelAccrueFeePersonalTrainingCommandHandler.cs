using System.Threading;
using System.Threading.Tasks;
using CoachsBox.Coaching.Accounting.StudentAccountEntryModel;
using CoachsBox.Coaching.Accounting.StudentAccountingEventModel;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CoachsBox.Coaching.Application.Commands
{
  public class CancelAccrueFeePersonalTrainingCommandHandler : IRequestHandler<CancelAccrueFeePersonalTrainingCommand, bool>
  {
    private readonly ILogger<CancelAccrueFeePersonalTrainingCommandHandler> logger;
    private readonly IStudentAccountingEventRepository eventRepository;
    private readonly IStudentAccountEntryRepository studentAccountEntryRepository;

    public CancelAccrueFeePersonalTrainingCommandHandler(
      ILogger<CancelAccrueFeePersonalTrainingCommandHandler> logger,
      IStudentAccountingEventRepository eventRepository,
      IStudentAccountEntryRepository studentAccountEntryRepository)
    {
      this.logger = logger;
      this.eventRepository = eventRepository;
      this.studentAccountEntryRepository = studentAccountEntryRepository;
    }

    public async Task<bool> Handle(CancelAccrueFeePersonalTrainingCommand request, CancellationToken cancellationToken)
    {
      if (request == null)
        return false;

      var personalTrainingAccrualEventSpecification = new PersonalTrainingAccrualEventSpecification(
        request.GroupId,
        request.AgreementId,
        request.StudentId,
        request.TrainingStart,
        request.TrainingEnd)
        .WithResultingAccountEntries();
      var personalTrainingAccrualEvent = await this.eventRepository.GetBySpecAsync(personalTrainingAccrualEventSpecification);
      if (personalTrainingAccrualEvent == null)
      {
        this.logger.LogWarning("Personal accrual accounting event not exists. GroupId: {GroupId}, AgreementId: {AgreementId}, StudentId: {StudentId}, Training: {TrainingStart}-{TrainingEnd}",
          request.GroupId, request.AgreementId, request.StudentId, request.TrainingStart, request.TrainingEnd);
        return false;
      }

      await this.eventRepository.DeleteAsync(personalTrainingAccrualEvent);
      await this.eventRepository.SaveAsync();

      if (personalTrainingAccrualEvent.ProcessingState.IsProcessed)
      {
        this.logger.LogWarning("Personal accrual accounting event already processed. Account entries will be deleted. GroupId: {GroupId}, AgreementId: {AgreementId}, StudentId: {StudentId}, Training: {TrainingStart}-{TrainingEnd}",
          request.GroupId, request.AgreementId, request.StudentId, request.TrainingStart, request.TrainingEnd);
        foreach (var resultingEntry in personalTrainingAccrualEvent.ProcessingState.ResultingEntries)
        {
          var studentAccountEntry = resultingEntry.AccountEntry as StudentAccountEntry;
          if (studentAccountEntry != null)
            await this.studentAccountEntryRepository.DeleteAsync(studentAccountEntry);
        }
        await this.studentAccountEntryRepository.SaveAsync();
      }

      logger.LogInformation("Cancel accrual [GroupId:{GroupId};AgreementId:{AgreementId};Period:{Date} {Start}-{End};StudentId:{StudentId}].",
        request.GroupId, request.AgreementId, request.TrainingStart.ToShortDateString(), request.TrainingStart.TimeOfDay, request.TrainingEnd.TimeOfDay, request.StudentId);

      return true;
    }
  }
}
