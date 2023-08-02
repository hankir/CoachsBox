using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using CoachsBox.WebApp.AppFacade;
using CoachsBox.WebApp.Areas.Identity.Data;
using CoachsBox.WebApp.Extensions;
using CoachsBox.WebApp.Pages.Facade;
using CoachsBox.WebApp.Pages.Facade.DTO;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace CoachsBox.WebApp.Pages.Attendance
{
  public partial class AttendanceLogEditor : OwningComponentBase<IAttendanceLogServiceFacade>
  {
    private HashSet<int> processedStudentId = new HashSet<int>();

    private DateTime endOfTraining;

    private bool canChangeAttendanceLog = true;

    private AttendanceLogDTO groupAttendance;

    // TODO: После обновления Blazor прикрутить TypeConverter
    // https://github.com/dotnet/aspnetcore/issues/8493
    // Вынести вместо Command параметры Date, Start, End и навесить [JsonConverter(typeof(TimeSpanConverter))].
    [Parameter]
    public Parameters TrainingInfo { get; set; }

    [Parameter]
    public bool HideHeader { get; set; }

    [Parameter]
    public bool ShowAllStudents { get; set; }

    protected override async Task OnInitializedAsync()
    {
      var attendanceLogRestriction = this.ScopedServices.GetRequiredService<IAttendanceLogRestriction>();
      var httpContextAccessor = this.ScopedServices.GetRequiredService<IHttpContextAccessor>();
      var currentUser = httpContextAccessor.HttpContext.User;
      this.endOfTraining = this.TrainingInfo.Date.Add(this.TrainingInfo.End);
      this.canChangeAttendanceLog = attendanceLogRestriction.CanChangeAttendanceLogOnDate(this.endOfTraining) ||
        currentUser.IsInRole(CoachsBoxWebAppRole.Administrator);

      var cacheKey = $"{nameof(AttendanceLogEditor)}-{this.TrainingInfo.GroupId}-{this.TrainingInfo.Date}-{this.TrainingInfo.Start}-{this.TrainingInfo.End}";
      if (!this.ScopedServices.TryGetCachedData(cacheKey, out this.groupAttendance))
      {
        this.groupAttendance = await this.Service.ViewTrainingAttendance(
          this.TrainingInfo.GroupId,
          this.TrainingInfo.Date,
          this.TrainingInfo.Start,
          this.TrainingInfo.End,
          this.ShowAllStudents);
        this.ScopedServices.SetCacheData(cacheKey, this.groupAttendance, TimeSpan.FromSeconds(3));
      }
    }

    public async Task OnMarkStudentClick(AttendanceLogEntryDTO studentAttendance, string absenceReason, bool isTrialTraining)
    {
      if (studentAttendance == null)
        return;

      this.processedStudentId.Add(studentAttendance.StudentId);
      var command = this.CreateMarkCommand(absenceReason, isTrialTraining);
      await this.Service.MarkStudent(studentAttendance.StudentId, command);
      UpdateStudentMark(studentAttendance, absenceReason, isTrialTraining, false);
      this.processedStudentId.Remove(studentAttendance.StudentId);
    }

    public async Task OnClearMarkStudent(AttendanceLogEntryDTO studentAttendance)
    {
      if (studentAttendance == null)
        return;

      this.processedStudentId.Add(studentAttendance.StudentId);
      var command = this.CreateMarkCommand(null, false);
      await this.Service.ClearMarkStudent(studentAttendance.StudentId, command);
      UpdateStudentMark(studentAttendance, null, studentAttendance.IsTrial, true);
      this.processedStudentId.Remove(studentAttendance.StudentId);
    }

    public async Task OnMarkAll(string absenceReason = null)
    {
      foreach (var studentAttendance in this.groupAttendance.Entries)
      {
        if (!studentAttendance.IsExists)
          this.processedStudentId.Add(studentAttendance.StudentId);
      }

      var commad = this.CreateMarkCommand(absenceReason, false);
      await this.Service.MarkAllStudents(this.groupAttendance.GroupId, commad);

      foreach (var studentAttendance in this.groupAttendance.Entries)
      {
        if (!studentAttendance.IsExists)
        {
          UpdateStudentMark(studentAttendance, commad.AbsenceReason, studentAttendance.IsTrial, false);
          this.processedStudentId.Remove(studentAttendance.StudentId);
        }
      }
    }

    public async Task OnClearAllMark()
    {
      foreach (var studentAttendance in this.groupAttendance.Entries)
      {
        if (studentAttendance.IsExists)
          this.processedStudentId.Add(studentAttendance.StudentId);
      }

      var commad = this.CreateMarkCommand(null, false);
      await this.Service.ClearAllMark(this.groupAttendance.GroupId, commad);

      foreach (var studentAttendance in this.groupAttendance.Entries)
      {
        if (studentAttendance.IsExists)
        {
          UpdateStudentMark(studentAttendance, null, studentAttendance.IsTrial, true);
          this.processedStudentId.Remove(studentAttendance.StudentId);
        }
      }
    }

    private MarkAttendanceCommand CreateMarkCommand(string absenceReason, bool isTrialTraining)
    {
      return new MarkAttendanceCommand()
      {
        CoachId = this.TrainingInfo.CoachId,
        Start = this.TrainingInfo.Start,
        End = this.TrainingInfo.End,
        Date = this.TrainingInfo.Date,
        AttendanceId = this.groupAttendance.AttendanceId,
        AbsenceReason = absenceReason,
        IsTrialTraining = isTrialTraining
      };
    }

    private static void UpdateStudentMark(AttendanceLogEntryDTO studentAttendance, string absenceReason, bool isTrial, bool clearMark)
    {
      studentAttendance.AbsenceReason = absenceReason;
      studentAttendance.IsTrial = isTrial;
      studentAttendance.IsExists = !clearMark;
    }

#pragma warning disable CA1034 // Nested types should not be visible
    public class Parameters
#pragma warning restore CA1034 // Nested types should not be visible
    {
      public int CoachId { get; set; }

      public int GroupId { get; set; }

      public DateTime Date { get; set; }

      [JsonConverter(typeof(TimeSpanConverter))]
      public TimeSpan Start { get; set; }

      [JsonConverter(typeof(TimeSpanConverter))]
      public TimeSpan End { get; set; }
    }
  }
}
