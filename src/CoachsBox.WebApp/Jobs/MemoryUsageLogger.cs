using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace CoachsBox.WebApp.Jobs
{
  public class MemoryUsageLogger : BackgroundService
  {
    private Process currentProcess;
    private Timer memoryUsageTimer;
    private readonly ILogger<MemoryUsageLogger> logger;
    private TimeSpan dueTime;
    private TimeSpan period;

    public MemoryUsageLogger(
      ILogger<MemoryUsageLogger> logger,
      IConfiguration configuration)
    {
      this.logger = logger;
      var dueTimeConfig = configuration["MemoryUsageLogger:dueTime"];
      if (!string.IsNullOrWhiteSpace(dueTimeConfig))
      {
        if (!TimeSpan.TryParse(dueTimeConfig, out this.dueTime))
          this.dueTime = TimeSpan.FromMinutes(1);
      }

      var periodConfig = configuration["MemoryUsageLogger:period"];
      if (!string.IsNullOrWhiteSpace(periodConfig))
      {
        if (!TimeSpan.TryParse(periodConfig, out this.period))
          this.period = TimeSpan.FromMinutes(1);
      }
    }

    public override Task StartAsync(CancellationToken cancellationToken)
    {
      this.currentProcess = Process.GetCurrentProcess();
      this.memoryUsageTimer = new Timer(this.LogMemoryUsage, "Periodic memory usage tracing", this.dueTime, this.period);
      return base.StartAsync(cancellationToken);
    }

    private void LogMemoryUsage(object state)
    {
      try
      {
        if (this.currentProcess != null)
        {
          this.currentProcess.Refresh();
          this.logger.LogInformation("{State}. PagedMemorySize64: {PagedMemorySize64} mb; PrivateMemorySize64: {PrivateMemorySize64} mb; VirtualMemorySize64: {VirtualMemorySize64} mb; PeakVirtualMemorySize64: {PeakVirtualMemorySize64} mb",
            state,
            this.currentProcess.PagedMemorySize64 / 1024 / 1024,
            this.currentProcess.PrivateMemorySize64 / 1024 / 1024,
            this.currentProcess.VirtualMemorySize64 / 1024 / 1024,
            this.currentProcess.PeakVirtualMemorySize64 / 1024 / 1024);
        }
        else
          this.currentProcess = Process.GetCurrentProcess();
      }
      catch (Exception ex)
      {
        this.logger.LogError(ex, "Error during log memory usage");
      }
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
      this.LogMemoryUsage("First time memory usage traicing");
      return Task.CompletedTask;
    }

    public override async Task StopAsync(CancellationToken cancellationToken)
    {
      if (this.memoryUsageTimer != null)
        await this.memoryUsageTimer.DisposeAsync();

      this.LogMemoryUsage("Finish time memory usage traicing");
    }
  }
}
