using System;
using MediatR;

namespace CoachsBox.Coaching.Application.Commands
{
  public class AccrueFeeMonthlyTrainingCommand : IRequest<bool>
  {
    public AccrueFeeMonthlyTrainingCommand(int groupId, int agreementId, int studentId, int month, int year)
    {
      this.GroupId = groupId;
      this.AgreementId = agreementId;
      this.StudentId = studentId;
      this.Month = month;
      this.Year = year;
    }

    public int GroupId { get; }

    public int AgreementId { get; }

    public int StudentId { get; }

    public int Month { get; }

    public int Year { get; }
  }
}
