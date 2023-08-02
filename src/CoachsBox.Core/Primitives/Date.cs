using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;

namespace CoachsBox.Core.Primitives
{
  /// <summary>
  /// Значение дата - день, месяц, год.
  /// </summary>
  [DebuggerDisplay("{Day}-{Month}-{Year}")]
  public class Date : ValueObject
  {
    public static Date Empty { get { return new Date() { Year = 0, Month = Month.Empty, Day = 0 }; } }

    public static Date Create(DateTime fromDateTime)
    {
      var month = Month.GetAll<Month>().Where(m => m.Number == fromDateTime.Month).Single();
      return new Date(
        day: (byte)fromDateTime.Day,
        month: month,
        year: fromDateTime.Year);
    }

    public static Date Create(Date fromDate)
    {
      var month = Month.GetAll<Month>().Where(m => m.Equals(fromDate.Month)).Single();
      return new Date(
        day: fromDate.Day,
        month: month,
        year: fromDate.Year);
    }

    public Date(byte day, Month month, int year)
    {
      var dayInMonth = month.DaysInMonth(year);
      if (day < 1 || day > dayInMonth)
        throw new ArgumentOutOfRangeException(nameof(day), day, $"Day should be greater than zero and less or equal {dayInMonth} when month is {month.Name}.");

      this.Month = month;
      this.Year = year;
      this.Day = day;
    }

    /// <summary>
    /// Получить месяц.
    /// </summary>
    public Month Month { get; private set; }

    /// <summary>
    /// Получить год.
    /// </summary>
    public int Year { get; private set; }

    /// <summary>
    /// Получить день даты.
    /// </summary>
    public byte Day { get; private set; }

    protected override IEnumerable<object> GetAtomicValues()
    {
      yield return this.Day;
      yield return this.Month;
      yield return this.Year;
    }

    public override string ToString()
    {
      return !this.Equals(Empty) ?
        new DateTime(this.Year, this.Month.Number, this.Day).ToShortDateString() :
        CultureInfo.CurrentUICulture.DateTimeFormat.ShortDatePattern;
    }

    private Date()
    {
      // Требует Entity framework code
    }
  }
}
