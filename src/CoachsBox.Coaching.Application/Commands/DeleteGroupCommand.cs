using System;
using System.Collections.Generic;
using System.Text;
using MediatR;

namespace CoachsBox.Coaching.Application.Commands
{
  public class DeleteGroupCommand : IRequest<bool>
  {
    public DeleteGroupCommand(int groupId)
    {
      this.GroupId = groupId;
    }

    public int GroupId { get; }
  }
}
