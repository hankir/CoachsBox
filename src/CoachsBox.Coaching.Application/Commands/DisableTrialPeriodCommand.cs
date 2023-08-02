using System;
using System.Collections.Generic;
using System.Text;
using MediatR;

namespace CoachsBox.Coaching.Application.Commands
{
  public class DisableTrialPeriodCommand : IRequest<bool>
  {
    public DisableTrialPeriodCommand(int groupId, int studentId)
    {
      this.GroupId = groupId;
      this.StudentId = studentId;
    }

    public int GroupId { get; }

    public int StudentId { get; }
  }
}
