using System;
using System.Collections.Generic;
using System.Text;
using MediatR;

namespace CoachsBox.Coaching.Application.Commands
{
  public class UpdateStudentNoteCommand : IRequest<string>
  {
    public UpdateStudentNoteCommand(int studentId, string note)
    {
      this.StudentId = studentId;
      this.Note = note;
    }

    public int StudentId { get; }

    public string Note { get; }
  }
}
