using System;
using MediatR;

namespace CoachsBox.Coaching.Application.Commands
{
  public class CancelAccrueFeePersonalTrainingCommand : IRequest<bool>
  {
    public CancelAccrueFeePersonalTrainingCommand(int groupId, int agreementId, int studentId, DateTime trainingStart, DateTime trainingEnd)
    {
      this.GroupId = groupId;
      this.AgreementId = agreementId;
      this.StudentId = studentId;
      this.TrainingStart = trainingStart;
      this.TrainingEnd = trainingEnd;
    }

    public int GroupId { get; }

    public int AgreementId { get; }

    public int StudentId { get; }

    public DateTime TrainingStart { get; }

    public DateTime TrainingEnd { get; }
  }
}
