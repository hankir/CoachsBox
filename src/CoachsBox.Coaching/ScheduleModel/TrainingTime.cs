using System;
using System.Collections.Generic;
using System.Diagnostics;
using CoachsBox.Core;
using CoachsBox.Core.Primitives;

namespace CoachsBox.Coaching.ScheduleModel
{
  /// <summary>
  /// Время по расписанию.
  /// </summary>
  [DebuggerDisplay("{Start}-{End} {DayOfWeek} {Date}")]
  public class TrainingTime : ValueObject
  {
    /// <summary>
    /// Создать время регулярной тренировки в определенный день недели.
    /// </summary>
    /// <param name="dayOfWeek">День недели.</param>
    /// <param name="start">Начало тренировки.</param>
    /// <param name="end">Конец тренировки.</param>
    public TrainingTime(DayOfWeek dayOfWeek, TimeOfDay start, TimeOfDay end)
    {
      if (start >= end)
        throw new ArgumentException("Time of start traininng should be less than time of end.", nameof(Start));

      this.Date = Date.Empty;
      this.DayOfWeek = dayOfWeek;
      this.Start = start;
      this.End = end;
    }

    /// <summary>
    /// Создать время тренировки на конкретную дату.
    /// </summary>
    /// <param name="date">Дата тренировки.</param>
    /// <param name="start">Начало тренировки.</param>
    /// <param name="end">Конец тренировки.</param>
    public TrainingTime(Date date, TimeOfDay start, TimeOfDay end)
      : this(CalculateDayOfWeek(date), start, end)
    {
      this.Date = date;
    }

    /// <summary>
    /// Получить тренировочный день недели.
    /// </summary>
    public DayOfWeek DayOfWeek { get; private set; }

    /// <summary>
    /// Проверить, что указанная дата совпадение с этим экземпляром времени тренировки.
    /// </summary>
    /// <param name="trainingDate">Проверяемая дата.</param>
    /// <returns>True если указанная дата совпадает с этим временем тренировки, иначе - false.</returns>
    public bool IsMatch(Date trainingDate)
    {
      var dayOfWeek = CalculateDayOfWeek(trainingDate);
      return Equals(this.Date, Date.Empty) ? dayOfWeek == this.DayOfWeek : this.Date.Equals(trainingDate);
    }

    /// <summary>
    /// Получить дату тренировки.
    /// </summary>
    public Date Date { get; private set; }

    /// <summary>
    /// Получить время начала тренировки.
    /// </summary>
    public TimeOfDay Start { get; private set; }

    /// <summary>
    /// Получить время конца тренировки.
    /// </summary>
    public TimeOfDay End { get; private set; }

    /// <summary>
    /// Расчитать день недели по точной дате.
    /// </summary>
    /// <param name="date">Дата.</param>
    /// <returns>День недели.</returns>
    public static DayOfWeek CalculateDayOfWeek(Date date)
    {
      return DateTime.SpecifyKind(new DateTime(date.Year, date.Month.Number, date.Day), DateTimeKind.Unspecified).DayOfWeek;
    }

    /// <summary>
    /// Получить признак того, что время тренировки указано регулярное.
    /// </summary>
    /// <returns></returns>
    public bool IsRegular()
    {
      return this.Date.Equals(Date.Empty);
    }

    protected override IEnumerable<object> GetAtomicValues()
    {
      yield return this.DayOfWeek;
      yield return this.Date;
      yield return this.Start;
      yield return this.End;
    }

    private TrainingTime()
    {
      // Требует Entity framework core
    }
  }
}
