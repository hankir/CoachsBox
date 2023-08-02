using System;
using System.Threading;

namespace CoachsBox.Core
{
  public static class Watch
  {
    private static IDateTimeProvider defaultDateTimeProvider;

    private static readonly ThreadLocal<IDateTimeProvider> currentProvider =
      new ThreadLocal<IDateTimeProvider>(() => defaultDateTimeProvider ?? new LocalDateTimeProvider());

    public static TimeZoneInfo TimeZone => CurrentProvider.TimeZone;

    public static DateTimeOffset Now => CurrentProvider.Now;

    public static DateTime LocalNow => CurrentProvider.Now.LocalDateTime;

    public static DateTime UtcNow => CurrentProvider.Now.UtcDateTime;

    public static DateTime ConvertTo(DateTime dateTime, TimeSpan toOffset)
    {
      return new DateTimeOffset(dateTime, CurrentProvider.TimeZone.BaseUtcOffset).ToOffset(toOffset).DateTime;
    }

    public static DateTime ConvertFrom(DateTime dateTime, TimeSpan fromOffset)
    {
      return new DateTimeOffset(dateTime, fromOffset).ToOffset(CurrentProvider.TimeZone.BaseUtcOffset).DateTime;
    }

    internal static IDateTimeProvider CurrentProvider
    {
      get => currentProvider.Value;
      set => currentProvider.Value = value;
    }

    public static void WindUp(IDateTimeProvider timeZoneDateTimeProvider)
    {
      defaultDateTimeProvider = timeZoneDateTimeProvider;
    }
  }
}
