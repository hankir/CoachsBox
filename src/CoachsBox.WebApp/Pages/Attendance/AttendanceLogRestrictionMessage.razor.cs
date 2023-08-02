using System;
using System.Globalization;
using CoachsBox.WebApp.AppFacade;
using CoachsBox.WebApp.Resources;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;

namespace CoachsBox.WebApp.Pages.Attendance
{
  public partial class AttendanceLogRestrictionMessage : OwningComponentBase
  {
    private bool canChangeAttendanceLog = true;

    private string attendanceLogRestrictMessage;

    [Parameter]
    public string ClassName { get; set; }

    [Parameter]
    public DateTime CheckedDate { get; set; }

    [Parameter]
    public int DaysBeforeShowRestrictMessage { get; set; } = 4;

    protected override void OnInitialized()
    {
      var attendanceLogRestriction = this.ScopedServices.GetRequiredService<IAttendanceLogRestriction>();
      var localizer = this.ScopedServices.GetRequiredService<IStringLocalizer<SharedResource>>();
      var httpContextAccessor = this.ScopedServices.GetRequiredService<IHttpContextAccessor>();
      var currentUser = httpContextAccessor.HttpContext.User;

      this.canChangeAttendanceLog = attendanceLogRestriction.CanChangeAttendanceLogOnDate(this.CheckedDate);
      if (this.canChangeAttendanceLog)
      {
        var nextStart = currentUser != null ? currentUser.ToUserTime(attendanceLogRestriction.NextCloseDate) : attendanceLogRestriction.NextCloseDate;
        if ((nextStart.Date - this.CheckedDate.Date).Days <= this.DaysBeforeShowRestrictMessage || this.DaysBeforeShowRestrictMessage == default)
        {
          if (nextStart.Date > this.CheckedDate.Date)
          {
            this.attendanceLogRestrictMessage = localizer.GetString(
              SharedResource.AttendanceLogWillBeClosedRestrictMessageFormat,
              nextStart.ToLongDateString());
          }
        }
      }
      else
      {
        var lastRun = currentUser != null ? currentUser.ToUserTime(attendanceLogRestriction.LastCloseDate) : attendanceLogRestriction.LastCloseDate;
        this.attendanceLogRestrictMessage = localizer.GetString(
          SharedResource.AttendanceLogClosedRestrictMessageFormat,
          lastRun.ToString("f", CultureInfo.CurrentCulture));
      }
    }
  }
}
