using System;
using System.Threading;
using System.Threading.Tasks;
using CoachsBox.Core;
using CoachsBox.WebApp.Jobs.Data;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace CoachsBox.WebApp.Jobs
{
  public abstract class ServiceWorker : BackgroundService
  {
    private readonly IServiceProvider services;
    private readonly ILogger logger;
    protected readonly string serviceId;

    public ServiceWorker(IServiceProvider services, ILogger logger, string serviceId)
    {
      this.services = services;
      this.logger = logger;
      this.serviceId = serviceId;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
      var serviceName = this.GetType().Name;
      this.logger.LogDebug($"{serviceName} is starting.");
      stoppingToken.Register(() => this.logger.LogDebug($"{serviceName} is stopping."));

      while (!stoppingToken.IsCancellationRequested)
      {
        ServiceInfo serviceInfo;
        using (var scope = this.services.CreateScope())
        {
          var serviceContext = scope.ServiceProvider.GetRequiredService<BackgroundServiceDbContext>();
          serviceInfo = serviceContext.Find<ServiceInfo>(this.serviceId);
          if (serviceInfo == null)
          {
            serviceInfo = new ServiceInfo(this.serviceId);
            serviceContext.Add(serviceInfo);
          }

          var utcNextStart = serviceInfo.NextStart;
          if (utcNextStart == default)
            utcNextStart = DateTime.UtcNow;

          var utcLastRun = serviceContext.GetServiceUtcLastRun(this.serviceId);
          if (DateTime.UtcNow >= utcNextStart)
          {
            try
            {
              if (utcLastRun == default)
                utcLastRun = DateTime.UtcNow;

              this.ServeAsync(scope, utcLastRun, stoppingToken);
              var serviceEvent = ServiceEvent.MakeSuccessful(serviceInfo);
              serviceContext.Add(serviceEvent);
              this.ScheduleNextStart(serviceInfo, serviceEvent.UtcLastRun);
            }
            catch (Exception ex)
            {
              var serviceEvent = ServiceEvent.MakeFailed(serviceInfo, ex.ToString());
              serviceContext.Add(serviceEvent);
              serviceInfo.ScheduleNextStart(serviceEvent.UtcLastRun.AddHours(1));
              this.logger.LogError(ex, "Error occurred during process accruals. Will continue attempt in next iteration.");
            }
            await serviceContext.SaveChangesAsync();
          }
          else
          {
            if (utcLastRun == default)
              logger.LogDebug("{Service} has not been run before", this.serviceId);
            else
              logger.LogDebug("{Service} last run {LastRun} (utc last run: {UtcLastRun})",
                this.serviceId, Watch.ConvertFrom(utcLastRun, DateTimeOffset.UtcNow.Offset), utcLastRun);
          }
        }

        // Отложим следующее начисление.
        var now = Watch.Now;
        // TODO: Подумать, не все гладко. Если RunInterval меньше суток, то delay может быть отрицательным и сервис остановится.
        var delay = serviceInfo.NextStart - DateTime.UtcNow;

        if (delay.TotalMilliseconds < 0)
        {
          this.logger.LogWarning("{ServiceName} delay {Delay} less than zero. Service will be stopped.", serviceName, delay.ToString());
          break;
        }

        this.logger.LogInformation("{ServiceName} delay {Delay} to {DateTime} (utc next start: {UtcNextStart})",
          serviceName, delay.ToString(), Watch.ConvertFrom(serviceInfo.NextStart, DateTimeOffset.UtcNow.Offset), serviceInfo.NextStart);
        await Task.Delay(delay, stoppingToken);
      }

      this.logger.LogDebug($"{serviceName} is stopping.");
    }

    protected abstract void ScheduleNextStart(ServiceInfo serviceInfo, DateTime utcLastRun);

    protected abstract void ServeAsync(IServiceScope scope, DateTime utcLastRun, CancellationToken stoppingToken);
  }
}
