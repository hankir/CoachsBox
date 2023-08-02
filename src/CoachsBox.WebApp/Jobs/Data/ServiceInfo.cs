using System;
namespace CoachsBox.WebApp.Jobs.Data
{
  /// <summary>
  /// Информация о сервисе.
  /// </summary>
  public class ServiceInfo
  {
    public ServiceInfo(string serviceId)
    {
      this.ServiceId = serviceId;
    }

    /// <summary>
    /// Получить идентификатор сервиса.
    /// </summary>
    public string ServiceId { get; private set; }

    /// <summary>
    /// Получить время следующего запуска.
    /// </summary>
    public DateTime NextStart { get; private set; }

    /// <summary>
    /// Запланировать время следующего запуска.
    /// </summary>
    /// <param name="nextStart">Следующий запуск.</param>
    public void ScheduleNextStart(DateTime nextStart)
    {
      if (nextStart.Kind != DateTimeKind.Utc)
        throw new InvalidOperationException("Next start must be specified in UTC");

      this.NextStart = nextStart;
    }

    private ServiceInfo()
    {
      // Требует Entity framework core
    }
  }
}
