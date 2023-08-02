using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;

namespace CoachsBox.Core.Primitives
{
  /// <summary>
  /// Перечесление месяцов.
  /// </summary>
  [DebuggerDisplay("{Name}")]
  public sealed class Month : ValueObject
  {
    public static Month Empty { get { return new Month(0, string.Empty); } }

    public static Month January { get { return new Month(1, nameof(January)); } }
    public static Month February { get { return new Month(2, nameof(February)); } }
    public static Month March { get { return new Month(3, nameof(March)); } }

    public static Month April { get { return new Month(4, nameof(April)); } }
    public static Month May { get { return new Month(5, nameof(May)); } }
    public static Month June { get { return new Month(6, nameof(June)); } }

    public static Month July { get { return new Month(7, nameof(July)); } }
    public static Month August { get { return new Month(8, nameof(August)); } }
    public static Month September { get { return new Month(9, nameof(September)); } }

    public static Month October { get { return new Month(10, nameof(October)); } }
    public static Month November { get { return new Month(11, nameof(November)); } }
    public static Month December { get { return new Month(12, nameof(December)); } }

    public static Month Create(int month)
    {
      return GetAll<Month>().Single(m => m.Number == month);
    }

    private Month(int number, string name)
    {
      this.Number = number;
      this.Name = name;
    }

    public int Number { get; private set; }

    public string Name { get; private set; }

    public int DaysInMonth(int year)
    {
      return DateTime.DaysInMonth(year, this.Number);
    }

    protected override IEnumerable<object> GetAtomicValues()
    {
      yield return this.Number;
      yield return this.Name;
    }

    private Month()
    {
      // Требует Entity framework code
    }
  }
}
