using System;
using System.Collections.Generic;
using System.Text;

namespace CoachsBox.Core
{
  public interface IDateTimeProvider
  {
    DateTimeOffset Now { get; }

    public TimeZoneInfo TimeZone { get; }
  }
}
