using System;
using MediatR;

namespace CoachsBox.Coaching.Application.Commands
{
  public class RegisterPaymentCommand : IRequest<int>
  {
    public RegisterPaymentCommand(decimal amount, DateTime whenOccured, int studentId, int groupId)
    {
      this.Amount = amount;
      this.WhenOccured = whenOccured;
      this.StudentId = studentId;
      this.GroupId = groupId;
    }

    public decimal Amount { get; }

    public DateTime WhenOccured { get; }

    public int StudentId { get; }

    public int GroupId { get; }
  }
}
