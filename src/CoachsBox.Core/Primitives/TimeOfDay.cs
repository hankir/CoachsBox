using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace CoachsBox.Core.Primitives
{
  /// <summary>
  /// Время дня.
  /// </summary>
  [DebuggerDisplay("{Hours}:{Minutes}")]
  public class TimeOfDay : ValueObject, IComparable
  {
    public TimeOfDay(byte hours, byte minutes)
    {
      // Не нужно проверять на значение меньше нуля. Тип byte не позволит это задать.
      if (hours > 23)
        throw new ArgumentOutOfRangeException(nameof(TimeOfDay.Hours), hours, "Hours should be greater or equal 0 and less or equal than 23");

      // Не нужно проверять на значение меньше нуля. Тип byte не позволит это задать.
      if (minutes > 59)
        throw new ArgumentOutOfRangeException(nameof(TimeOfDay.Minutes), minutes, "Minutes should be greater or equal 0 and less or equal than 59");

      this.Hours = hours;
      this.Minutes = minutes;
    }

    /// <summary>
    /// Получить часы.
    /// </summary>
    public byte Hours { get; private set; }

    /// <summary>
    /// Получить минуты.
    /// </summary>
    public byte Minutes { get; private set; }

    protected override IEnumerable<object> GetAtomicValues()
    {
      yield return this.Hours;
      yield return this.Minutes;
    }

    public static bool operator >(TimeOfDay a, TimeOfDay b)
    {
      return a.CompareTo(b) < 0;
    }

    public static bool operator <(TimeOfDay a, TimeOfDay b)
    {
      return a.CompareTo(b) > 0;
    }

    public static bool operator >=(TimeOfDay a, TimeOfDay b)
    {
      return a.CompareTo(b) <= 0;
    }

    public static TimeOfDay Create(TimeSpan time)
    {
      return new TimeOfDay((byte)time.Hours, (byte)time.Minutes);
    }

    public static TimeOfDay Create(TimeOfDay time)
    {
      return new TimeOfDay(time.Hours, time.Minutes);
    }

    public static bool operator <=(TimeOfDay a, TimeOfDay b)
    {
      return a.CompareTo(b) >= 0;
    }

    public int CompareTo(object obj)
    {
      var timeOfDay = obj as TimeOfDay;
      if (timeOfDay == null)
        throw new ArgumentException($"Object is not a {nameof(TimeOfDay)}");

      // Время равно.
      if (this.Equals(timeOfDay))
        return 0;

      // Часы равны, минуты разные.
      if (this.Hours == timeOfDay.Hours)
        return this.Minutes > timeOfDay.Minutes ? -1 : 1;

      // Часы разные, на минуты можно не смотреть.
      if (this.Hours > timeOfDay.Hours)
        return -1;
      else
        return 1;
    }

    private TimeOfDay()
    {
      // Требует Entity framework code
    }
  }
}
