using System;
using System.Collections.Generic;
using System.Text;
using MediatR;

namespace CoachsBox.Coaching.Application.Commands
{
  public class ExcludeStudentCommand : IRequest<bool>
  {
    public ExcludeStudentCommand(int groupId, int studentId)
    {
      this.GroupId = groupId;
      this.StudentId = studentId;
    }

    public int GroupId { get; }

    public int StudentId { get; }
  }
}
