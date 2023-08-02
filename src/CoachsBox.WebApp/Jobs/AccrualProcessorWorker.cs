using System;
using System.Threading;
using CoachsBox.Coaching.Application;
using CoachsBox.Core;
using CoachsBox.WebApp.Jobs.Data;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace CoachsBox.WebApp.Jobs
{
  public class AccrualProcessorWorker : ServiceWorker
  {
    public AccrualProcessorWorker(IServiceProvider services, ILogger<AccrualProcessorWorker> logger)
      : base(services, logger, typeof(AccrualProcessorWorker).FullName)
    {
    }

    protected override void ScheduleNextStart(ServiceInfo serviceInfo, DateTime utcLastRun)
    {
      var startWeekDay = Watch.ConvertFrom(utcLastRun, DateTimeOffset.UtcNow.Offset).StartOfWeek(DayOfWeek.Monday);
      serviceInfo.ScheduleNextStart(startWeekDay.AddDays(7).ToUniversalTime());
    }

    protected override void ServeAsync(IServiceScope scope, DateTime utcLastRun, CancellationToken stoppingToken)
    {
      var accrualService = scope.ServiceProvider.GetRequiredService<IAccrualService>();
      accrualService.ProcessAccruals();
    }
  }
}
