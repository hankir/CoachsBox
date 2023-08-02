using System.Collections.Generic;
using System.Threading.Tasks;
using CoachsBox.WebApp.AppFacade.Students.DTO;
using CoachsBox.WebApp.Pages.Facade;
using CoachsBox.WebApp.Pages.Facade.DTO;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;

namespace CoachsBox.WebApp.Pages.Students.Components
{
  public partial class StudentGroups : OwningComponentBase
  {
    private bool isGroupListLoading = true;
    private IReadOnlyCollection<GroupDTO> studentGroups = new List<GroupDTO>();

    [Parameter]
    public StudentDetailsDTO Student { get; set; }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
      if (firstRender)
      {
        await this.LoadGroups();
        this.StateHasChanged();
      }

      await base.OnAfterRenderAsync(firstRender);
    }

    private async Task LoadGroups()
    {
      this.isGroupListLoading = true;
      var groupManagmentServiceFacade = this.ScopedServices.GetRequiredService<IGroupManagmentServiceFacade>();
      this.studentGroups = await groupManagmentServiceFacade.ListGroupsByStudent(this.Student.StudentId);
      this.isGroupListLoading = false;
    }
  }
}
