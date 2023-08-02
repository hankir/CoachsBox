using System;

namespace CoachsBox.Core
{
  public class TimeZoneDateTimeProvider : IDateTimeProvider
  {
    public TimeZoneDateTimeProvider(TimeZoneInfo timeZone)
    {
      if (timeZone == null)
        throw new ArgumentNullException(nameof(timeZone));

      this.TimeZone = timeZone;
    }

    public DateTimeOffset Now => TimeZoneInfo.ConvertTime(DateTimeOffset.Now, this.TimeZone);

    public TimeZoneInfo TimeZone { get; }
  }
}
