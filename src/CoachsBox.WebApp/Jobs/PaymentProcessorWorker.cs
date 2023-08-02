using System;
using System.Threading;
using CoachsBox.Coaching.Application;
using CoachsBox.Core;
using CoachsBox.WebApp.Jobs.Data;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace CoachsBox.WebApp.Jobs
{
  public class PaymentProcessorWorker : ServiceWorker
  {
    public PaymentProcessorWorker(IServiceProvider services, ILogger<PaymentProcessorWorker> logger)
      : base(services, logger, typeof(PaymentProcessorWorker).FullName)
    {
    }

    protected override void ScheduleNextStart(ServiceInfo serviceInfo, DateTime utcLastRun)
    {
      serviceInfo.ScheduleNextStart(Watch.Now.Date.AddDays(1).ToUniversalTime());
    }

    protected override void ServeAsync(IServiceScope scope, DateTime utcLastRun, CancellationToken stoppingToken)
    {
      var paymentService = scope.ServiceProvider.GetRequiredService<IPaymentService>();
      paymentService.ProcessPayments();
    }
  }
}
