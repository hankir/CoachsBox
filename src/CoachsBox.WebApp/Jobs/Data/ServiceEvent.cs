using System;

namespace CoachsBox.WebApp.Jobs.Data
{
  /// <summary>
  /// Событие обработки сервиса.
  /// </summary>
  public class ServiceEvent
  {
    public static ServiceEvent MakeSuccessful(ServiceInfo service)
    {
      if (service == null)
        throw new ArgumentNullException(nameof(service));

      return new ServiceEvent(service, DateTime.UtcNow)
      {
        Result = ServiceRunResult.Success
      };
    }

    public static ServiceEvent MakeFailed(ServiceInfo service, string reason)
    {
      if (service == null)
        throw new ArgumentNullException(nameof(service));

      return new ServiceEvent(service, DateTime.UtcNow)
      {
        Result = ServiceRunResult.Failure,
        FailureReason = reason
      };
    }

    public ServiceEvent(ServiceInfo service, DateTime lastRun)
    {
      if (lastRun.Kind != DateTimeKind.Utc)
        throw new InvalidOperationException("Last run must be specified in UTC");

      if (service == null)
        throw new ArgumentNullException(nameof(service));

      this.Service = service;
      this.ServiceId = service.ServiceId;

      this.UtcLastRun = lastRun;
    }

    /// <summary>
    /// Получить идентификатор сервиса.
    /// </summary>
    public string ServiceId { get; private set; }

    /// <summary>
    /// Получить идентификатор службы.
    /// </summary>
    public ServiceInfo Service { get; private set; }

    /// <summary>
    /// Получить время последнего запуска в UTC формате.
    /// </summary>
    public DateTime UtcLastRun { get; private set; }

    /// <summary>
    /// Получить результат последнего запуска.
    /// </summary>
    public ServiceRunResult Result { get; private set; }

    /// <summary>
    /// Получить причину неудачи.
    /// </summary>
    public string FailureReason { get; private set; }

    private ServiceEvent()
    {
      // Требует Entity framework core
    }
  }
}
