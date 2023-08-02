using System;
using System.Threading.Tasks;
using CoachsBox.WebApp.AppFacade.Groups.DTO;
using CoachsBox.WebApp.Extensions;
using CoachsBox.WebApp.Pages.Facade;
using Microsoft.AspNetCore.Components;

namespace CoachsBox.WebApp.Pages.Groups.Components
{
  public partial class GroupCard : OwningComponentBase<IGroupManagmentServiceFacade>
  {
    private GroupDetailsDTO group;

    private Attendance.AttendanceLogView attendanceLogView;

    [Parameter]
    public int GroupId { get; set; }

    [Parameter]
    public int Year { get; set; }

    [Parameter]
    public int Month { get; set; }

    [Parameter]
    public bool ShowAllStudents { get; set; }

    [Parameter]
    public DateTime? PaymentsEndPeriod { get; set; }

    protected override async Task OnInitializedAsync()
    {
      var cacheKey = $"Group{this.GroupId}-Attendance-{this.Year}-{this.Month}-{this.PaymentsEndPeriod}";
      if (!this.ScopedServices.TryGetCachedData(cacheKey, out this.group))
      {
        this.group = await this.LoadGroup();
        this.ScopedServices.SetCacheData(cacheKey, this.group, TimeSpan.FromSeconds(3));
      }
    }

    private async Task<GroupDetailsDTO> LoadGroup()
    {
      return this.PaymentsEndPeriod.HasValue ?
        await this.Service.ViewGroup(this.GroupId, this.Year, this.Month, this.PaymentsEndPeriod.Value) :
        await this.Service.ViewGroup(this.GroupId, this.Year, this.Month);
    }

    private async Task StudentEnrolled()
    {
      try
      {
        this.group = await this.LoadGroup();
        await this.attendanceLogView.Refresh();
      }
      finally
      {
        this.StateHasChanged();
      }
    }

    private async Task StudentExcluded()
    {
      try
      {
        await this.attendanceLogView.Refresh();
      }
      finally
      {
        this.StateHasChanged();
      }
    }
  }
}
