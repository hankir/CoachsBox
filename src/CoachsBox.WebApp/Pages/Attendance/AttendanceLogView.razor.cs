using System;
using System.Linq;
using System.Threading.Tasks;
using CoachsBox.Coaching.Application;
using CoachsBox.Coaching.Application.Data;
using CoachsBox.Coaching.AttendanceLogModel;
using CoachsBox.WebApp.AppFacade.Attendance.DTO;
using CoachsBox.WebApp.Extensions;
using CoachsBox.WebApp.Pages.Facade;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;

namespace CoachsBox.WebApp.Pages.Attendance
{
  public partial class AttendanceLogView : OwningComponentBase<IAttendanceLogServiceFacade>
  {
    private AttendanceLogViewDTO attendanceLog;

    private int selectedMonth;

    private int selectedYear;

    private bool showMarkedStudents;

    private AttendanceLogEditor.Parameters showedTrainingTimeEditor;

    [Parameter]
    public int GroupId { get; set; }

    [Parameter]
    public int Month { get; set; }

    [Parameter]
    public int Year { get; set; }

    [Parameter]
    public bool ShowAllStudents { get; set; }

    public async Task Refresh()
    {
      this.attendanceLog = await this.LoadAttendanceLog();
      this.StateHasChanged();
    }

    protected override async Task OnInitializedAsync()
    {
      this.selectedMonth = this.Month;
      this.selectedYear = this.Year;
      this.showMarkedStudents = this.ShowAllStudents;

      var cacheKey = $"{nameof(AttendanceLogView)}-{this.GroupId}-{this.selectedMonth}-{this.selectedYear}-{this.showMarkedStudents}";
      if (!this.ScopedServices.TryGetCachedData(cacheKey, out this.attendanceLog))
      {
        this.attendanceLog = await this.LoadAttendanceLog();
        this.ScopedServices.SetCacheData(cacheKey, this.attendanceLog, TimeSpan.FromSeconds(3));
      }
    }

    protected async Task SelectMonthAsync(int month)
    {
      this.selectedMonth = month;
      this.attendanceLog = await this.LoadAttendanceLog();
    }

    protected async Task SelectYearAsync(int year)
    {
      this.selectedYear = year;
      this.attendanceLog = await this.LoadAttendanceLog();
    }

    protected async Task OnShowMarkedStudents(ChangeEventArgs e)
    {
      this.showMarkedStudents = !this.showMarkedStudents;
      this.attendanceLog = await this.LoadAttendanceLog();
    }

    private Task<AttendanceLogViewDTO> LoadAttendanceLog()
    {
      return this.Service.ViewAttendanceLog(this.GroupId, this.selectedMonth, this.selectedYear, this.showMarkedStudents);
    }

    protected void ShowTrainingEditorModal(TrainingTimeInfo trainingTime)
    {
      this.showedTrainingTimeEditor = new AttendanceLogEditor.Parameters()
      {
        GroupId = this.GroupId,
        Date = trainingTime.Date,
        Start = trainingTime.Start,
        End = trainingTime.End
      };
    }

    protected async Task CloseTrainingEditorModalAsync()
    {
      this.showedTrainingTimeEditor = null;
      this.attendanceLog = await this.LoadAttendanceLog();
    }

    private int CountStudentAccountableMarks(int studentId)
    {
      var accrualService = this.ScopedServices.GetRequiredService<IAccrualService>();
      return this.attendanceLog.AttendanceLog.Entries
        .Where(entry => entry.StudentId == studentId && accrualService.IsAttendanceLogEntryAccountable(entry.IsTrial, Absence.GetAbsence(entry.AbsenceReason)))
        .Count();
    }

    private int CountAccountableMarks()
    {
      var accrualService = this.ScopedServices.GetRequiredService<IAccrualService>();
      return this.attendanceLog.AttendanceLog.Entries
        .Where(entry => accrualService.IsAttendanceLogEntryAccountable(entry.IsTrial, Absence.GetAbsence(entry.AbsenceReason)))
        .Count();
    }
  }
}
