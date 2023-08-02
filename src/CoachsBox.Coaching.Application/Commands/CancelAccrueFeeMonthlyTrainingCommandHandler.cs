using System.Threading;
using System.Threading.Tasks;
using CoachsBox.Coaching.Accounting.StudentAccountEntryModel;
using CoachsBox.Coaching.Accounting.StudentAccountingEventModel;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CoachsBox.Coaching.Application.Commands
{
  public class CancelAccrueFeeMonthlyTrainingCommandHandler : IRequestHandler<CancelAccrueFeeMonthlyTrainingCommand, bool>
  {
    private readonly ILogger<CancelAccrueFeeMonthlyTrainingCommandHandler> logger;
    private readonly IStudentAccountingEventRepository eventRepository;
    private readonly IStudentAccountEntryRepository studentAccountEntryRepository;

    public CancelAccrueFeeMonthlyTrainingCommandHandler(
      ILogger<CancelAccrueFeeMonthlyTrainingCommandHandler> logger,
      IStudentAccountingEventRepository eventRepository,
      IStudentAccountEntryRepository studentAccountEntryRepository)
    {
      this.logger = logger;
      this.eventRepository = eventRepository;
      this.studentAccountEntryRepository = studentAccountEntryRepository;
    }

    public async Task<bool> Handle(CancelAccrueFeeMonthlyTrainingCommand request, CancellationToken cancellationToken)
    {
      if (request == null)
        return false;

      var groupId = request.GroupId;
      var studentId = request.StudentId;
      var agreementId = request.AgreementId;
      var month = request.Month;
      var year = request.Year;

      var monthlyAccrualSpecification = new AccrualForMonthSpecification(studentId, month, year).WithResultingAccountEntries();
      var monthlyAccrualEvent = await this.eventRepository.GetBySpecAsync(monthlyAccrualSpecification);
      if (monthlyAccrualEvent == null)
      {
        this.logger.LogWarning("Monthly accrual accounting event not exists. GroupId: {GroupId}, AgreementId: {AgreementId}, StudentId: {StudentId}, Period:{Month}/{Year}",
          groupId, agreementId, studentId, month, year);
        return false;
      }

      await this.eventRepository.DeleteAsync(monthlyAccrualEvent);
      await this.eventRepository.SaveAsync();

      if (monthlyAccrualEvent.ProcessingState.IsProcessed)
      {
        this.logger.LogWarning("Monthly accrual accounting event already processed. Account entries will be deleted. GroupId: {GroupId}, AgreementId: {AgreementId}, StudentId: {StudentId}, Period:{Month}/{Year}",
          groupId, agreementId, studentId, month, year);

        foreach (var resultingEntry in monthlyAccrualEvent.ProcessingState.ResultingEntries)
        {
          var studentAccountEntry = resultingEntry.AccountEntry as StudentAccountEntry;
          if (studentAccountEntry != null)
            await this.studentAccountEntryRepository.DeleteAsync(studentAccountEntry);
        }
        await this.studentAccountEntryRepository.SaveAsync();
      }

      logger.LogInformation("Cancel accrual [GroupId:{GroupId};AgreementId:{AgreementId};Period:{Month}/{Year};StudentId:{StudentId}].",
        groupId, agreementId, month, year, studentId);

      return true;
    }
  }
}
