using System;
using CoachsBox.WebApp.AppFacade;
using CoachsBox.WebApp.Jobs;
using CoachsBox.WebApp.Jobs.Data;
using CoachsBox.WebApp.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Xunit;

namespace CoachsBox.WebAppTests
{
  public class AttendanceLogRestrictionTests : IDisposable
  {
    private BackgroundServiceDbContext context;

    [Fact]
    public void CanChangeNowWhenNoServiceInfo()
    {
      var restriction = new AttendanceLogRestriction(this.context);
      Assert.True(restriction.CanChangeAttendanceLogOnDate(DateTime.Now));
    }

    [Fact]
    public void CanChangeNowWhenClosedTodayBefore()
    {
      var utcNow = new DateTime(2020, 9, 22, 9, 30, 0, DateTimeKind.Utc);
      var utcLastRun = utcNow.AddHours(-1);

      var serviceInfo = this.AddServiceInfoToContext(typeof(AccrualProcessorWorker).FullName);
      AddServiceEventToContext(serviceInfo, utcLastRun);
      this.context.SaveChanges();

      var restriction = new AttendanceLogRestriction(this.context);

      Assert.True(restriction.CanChangeAttendanceLogOnDate(utcNow.ToLocalTime()));
      Assert.Equal(utcLastRun.ToLocalTime(), restriction.LastCloseDate);
      Assert.Equal(serviceInfo.NextStart.ToLocalTime(), restriction.NextCloseDate);
    }

    [Fact]
    public void CanChangeNowDateWhenClosedTodayBefore()
    {
      var utcNow = new DateTime(2020, 9, 22, 9, 30, 0, DateTimeKind.Utc);
      var utcLastRun = utcNow.AddHours(-1);

      var serviceInfo = this.AddServiceInfoToContext(typeof(AccrualProcessorWorker).FullName);
      AddServiceEventToContext(serviceInfo, utcLastRun);
      this.context.SaveChanges();

      var restriction = new AttendanceLogRestriction(this.context);

      Assert.True(restriction.CanChangeAttendanceLogOnDate(utcNow.ToLocalTime().Date));
      Assert.Equal(utcLastRun.ToLocalTime(), restriction.LastCloseDate);
      Assert.Equal(serviceInfo.NextStart.ToLocalTime(), restriction.NextCloseDate);
    }

    [Fact]
    public void CanNotChangeNowWhenClosedTodayAfter()
    {
      var utcNow = new DateTime(2020, 9, 22, 9, 30, 0, DateTimeKind.Utc);
      var utcLastRun = utcNow.AddHours(1);

      var serviceInfo = this.AddServiceInfoToContext(typeof(AccrualProcessorWorker).FullName);
      AddServiceEventToContext(serviceInfo, utcLastRun);
      this.context.SaveChanges();

      var restriction = new AttendanceLogRestriction(this.context);

      Assert.False(restriction.CanChangeAttendanceLogOnDate(utcNow.ToLocalTime()));
      Assert.Equal(utcLastRun.ToLocalTime(), restriction.LastCloseDate);
      Assert.Equal(serviceInfo.NextStart.ToLocalTime(), restriction.NextCloseDate);
    }

    [Fact]
    public void CanChangeNowDateWhenClosedTodayAfter()
    {
      var utcNow = new DateTime(2020, 9, 22, 9, 30, 0, DateTimeKind.Utc);
      var utcLastRun = utcNow.AddHours(1);

      var serviceInfo = this.AddServiceInfoToContext(typeof(AccrualProcessorWorker).FullName);
      AddServiceEventToContext(serviceInfo, utcLastRun);
      this.context.SaveChanges();

      var restriction = new AttendanceLogRestriction(this.context);

      Assert.True(restriction.CanChangeAttendanceLogOnDate(utcNow.ToLocalTime().Date));
      Assert.Equal(utcLastRun.ToLocalTime(), restriction.LastCloseDate);
      Assert.Equal(serviceInfo.NextStart.ToLocalTime(), restriction.NextCloseDate);
    }

    [Fact]
    public void ExpectedScheduledNextRun()
    {
      var serviceInfo = this.AddServiceInfoToContext(typeof(AccrualProcessorWorker).FullName);

      var utcNow = new DateTime(2020, 9, 22, 9, 30, 0, DateTimeKind.Utc);
      var nextStart = utcNow.AddDays(1);
      serviceInfo.ScheduleNextStart(nextStart);
      this.context.SaveChanges();

      var restriction = new AttendanceLogRestriction(this.context);

      Assert.True(restriction.CanChangeAttendanceLogOnDate(utcNow.ToLocalTime()));
      Assert.Equal(nextStart.ToLocalTime(), restriction.NextCloseDate);
    }

    private ServiceInfo AddServiceInfoToContext(string serviceId)
    {
      var serviceInfo = new ServiceInfo(serviceId);
      this.context.Add(serviceInfo);
      return serviceInfo;
    }

    private ServiceEvent AddServiceEventToContext(ServiceInfo serviceInfo, DateTime lastRun)
    {
      var serviceEvent = new ServiceEvent(serviceInfo, lastRun);
      this.context.Add(serviceEvent);
      return serviceEvent;
    }

    public void Dispose()
    {
      this.context?.Dispose();
    }

    public AttendanceLogRestrictionTests()
    {
      var dbOptions = new DbContextOptionsBuilder<BackgroundServiceDbContext>()
        .UseInMemoryDatabase(this.GetType().Name + Guid.NewGuid().ToString())
        .ConfigureWarnings(builder => builder.Ignore(InMemoryEventId.TransactionIgnoredWarning))
        .Options;
      this.context = new BackgroundServiceDbContext(dbOptions);
    }
  }
}