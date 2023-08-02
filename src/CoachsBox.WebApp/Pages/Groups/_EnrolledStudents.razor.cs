using System.Collections.Generic;
using System.Threading.Tasks;
using CoachsBox.Coaching.Application.Commands;
using CoachsBox.WebApp.AppFacade.Groups.DTO;
using MediatR;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;

namespace CoachsBox.WebApp.Pages.Groups
{
  public partial class _EnrolledStudents : OwningComponentBase
  {
    private HashSet<int> processedStudentIds = new HashSet<int>();

    [Parameter]
    public List<GroupDetailsStudentDTO> Students { get; set; }

    [Parameter]
    public int GroupId { get; set; }

    [Parameter]
    public EventCallback<int> StudentExcluded { get; set; }

    public async Task OnEnableTrialPeriodClick(GroupDetailsStudentDTO student)
    {
      if (student == null)
        return;

      this.processedStudentIds.Add(student.StudentId);
      using (var scope = this.ScopedServices.CreateScope())
      {
        var service = scope.ServiceProvider.GetRequiredService<IMediator>();
        await service.Send(new EnableTrialPeriodCommand(this.GroupId, student.StudentId));
        student.TrialTrainingCount = 1;
      }
      this.processedStudentIds.Remove(student.StudentId);
    }

    public async Task OnDisableTrialPeriodClick(GroupDetailsStudentDTO student)
    {
      if (student == null)
        return;

      this.processedStudentIds.Add(student.StudentId);
      using (var scope = this.ScopedServices.CreateScope())
      {
        var service = scope.ServiceProvider.GetRequiredService<IMediator>();
        await service.Send(new DisableTrialPeriodCommand(this.GroupId, student.StudentId));
        student.TrialTrainingCount = 0;
      }
      this.processedStudentIds.Remove(student.StudentId);
    }

    public async Task OnExcludeStudentFromGroupClick(GroupDetailsStudentDTO student)
    {
      if (student == null)
        return;

      this.processedStudentIds.Add(student.StudentId);
      using (var scope = this.ScopedServices.CreateScope())
      {
        var service = scope.ServiceProvider.GetRequiredService<IMediator>();
        await service.Send(new ExcludeStudentCommand(this.GroupId, student.StudentId));
        this.processedStudentIds.Remove(student.StudentId);
        this.Students.Remove(student);
      }
      await this.StudentExcluded.InvokeAsync(student.StudentId);
    }
  }
}
