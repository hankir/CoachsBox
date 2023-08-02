using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoachsBox.Coaching.Application.Commands;
using CoachsBox.WebApp.AppFacade.Students.DTO;
using MediatR;
using Microsoft.AspNetCore.Components;

namespace CoachsBox.WebApp.Pages.Students.Components
{
  public partial class CommonInfo : OwningComponentBase<IMediator>
  {
    [Parameter]
    public StudentDetailsDTO Student { get; set; }

    public async Task ChangeNote(string note)
    {
      this.Student.Note = await this.Service.Send(new UpdateStudentNoteCommand(this.Student.StudentId, note));
    }
  }
}
