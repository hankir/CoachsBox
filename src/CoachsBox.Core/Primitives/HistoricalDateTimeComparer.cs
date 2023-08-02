using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace CoachsBox.Core.Primitives
{
  /// <summary>
  /// Компаратор дат в историческом смысле.
  /// Даты случившиеся недавно меньше чем даты случившиеся давно.
  /// </summary>
  public class HistoricalDateTimeComparer : IComparer<DateTime>
  {
    public int Compare([NotNull] DateTime x, [NotNull] DateTime y)
    {
      if (x == y)
        return 0;

      if (x > y)
        return -1;

      return 1;
    }
  }
}
