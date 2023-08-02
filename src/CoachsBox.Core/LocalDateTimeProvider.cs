using System;
using System.Collections.Generic;
using System.Text;

namespace CoachsBox.Core
{
  public class LocalDateTimeProvider : IDateTimeProvider
  {
    public DateTimeOffset Now => DateTimeOffset.Now;
    public TimeZoneInfo TimeZone => TimeZoneInfo.Local;
  }
}
