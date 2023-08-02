using System;
using CoachsBox.Core.Primitives;

namespace CoachsBox.Core
{
  public static class DateExtensions
  {
    public static DateTime? ToDateTime(this Date date)
    {
      return date?.Equals(Date.Empty) == false ? new Nullable<DateTime>(new DateTime(date.Year, date.Month.Number, date.Day)) : null;
    }

    public static TimeSpan ToTimeSpan(this TimeOfDay timeOfDay)
    {
      return new TimeSpan(timeOfDay.Hours, timeOfDay.Minutes, 0);
    }
  }
}
