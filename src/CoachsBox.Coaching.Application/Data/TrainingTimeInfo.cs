using System;

namespace CoachsBox.Coaching.Application.Data
{
  public struct TrainingTimeInfo
  {
    public TrainingTimeInfo(DateTime date, TimeSpan start, TimeSpan end, bool isGhost, bool isRegular)
    {
      this.Date = date;
      this.Start = start;
      this.End = end;
      this.IsGhost = isGhost;
      this.IsRegular = isRegular;
    }

    /// <summary>
    /// Получить дату тренировки.
    /// </summary>
    public DateTime Date { get; private set; }

    /// <summary>
    /// Получить время начала тренировки.
    /// </summary>
    public TimeSpan Start { get; private set; }

    /// <summary>
    /// Получить время конца тренировки.
    /// </summary>
    public TimeSpan End { get; private set; }

    /// <summary>
    /// Получить признак того, что отметка установлена на дату, которой нет в расписании.
    /// </summary>
    public bool IsGhost { get; private set; }

    /// <summary>
    /// Получить признак того, что время тренировки указано регулярное.
    /// </summary>
    public bool IsRegular { get; private set; }
  }
}
