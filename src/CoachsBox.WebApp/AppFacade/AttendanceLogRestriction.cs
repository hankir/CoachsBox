using System;
using CoachsBox.Core;
using CoachsBox.WebApp.Jobs;
using CoachsBox.WebApp.Jobs.Data;

namespace CoachsBox.WebApp.AppFacade
{
  public class AttendanceLogRestriction : IAttendanceLogRestriction
  {
    private static readonly string AccrualProcessorServiceId = typeof(AccrualProcessorWorker).FullName;

    private readonly BackgroundServiceDbContext serviceDbContext;

    public AttendanceLogRestriction(BackgroundServiceDbContext serviceDbContext)
    {
      if (serviceDbContext == null)
        throw new ArgumentNullException(nameof(serviceDbContext));

      this.serviceDbContext = serviceDbContext;
      var serviceInfo = this.serviceDbContext.Find<ServiceInfo>(AccrualProcessorServiceId);
      if (serviceInfo != null)
      {
        var lastRun = this.serviceDbContext.GetServiceUtcLastRun(AccrualProcessorServiceId);
        this.LastCloseDate = lastRun == default ? DateTime.MinValue : Watch.ConvertFrom(lastRun, DateTimeOffset.UtcNow.Offset);
        this.NextCloseDate = Watch.ConvertFrom(serviceInfo.NextStart, DateTimeOffset.UtcNow.Offset);
      }
      else
      {
        this.LastCloseDate = DateTime.MinValue;
        this.NextCloseDate = DateTime.MaxValue;
      }
    }

    public bool CanChangeAttendanceLogOnDate(DateTime dateTime)
    {
      // Когда для переданной даты не указано время, то сравниваем по концу дня переданной даты.
      return dateTime.TimeOfDay == TimeSpan.Zero ?
        (dateTime.AddDays(1) - TimeSpan.FromSeconds(1)) >= this.LastCloseDate :

        // Иначе сравниваем как есть.
        dateTime >= this.LastCloseDate;
    }

    public DateTime NextCloseDate { get; }

    public DateTime LastCloseDate { get; }
  }
}
