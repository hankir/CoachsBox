using System;

namespace CoachsBox.Core
{
  public sealed class UtcDateTimeProvider : IDateTimeProvider
  {
    public DateTimeOffset Now => DateTimeOffset.UtcNow;

    public TimeZoneInfo TimeZone => TimeZoneInfo.Utc;
  }
}
