namespace CoachsBox.Coaching.AttendanceLogModel
{
  public class AttendanceStatisticsInfo
  {
    /// <summary>
    /// Получить или установить кол-во посещавщих, в том числе и пробные тренировки.
    /// </summary>
    public int NumberAttendedTotal { get; set; }

    /// <summary>
    /// Получить или установить кол-во посещавщих пробные тренировки.
    /// </summary>
    public int NumberAttendedTrial { get; set; }

    /// <summary>
    /// Получить или установить кол-во посещавщих, кроме пробных тренировок.
    /// </summary>
    public int NumberAttended { get; set; }

    /// <summary>
    /// Получить или установить кол-во отсутствующих.
    /// </summary>
    public int NumberAbsent { get; set; }

    /// <summary>
    /// Получить или установить кол-во заболевших.
    /// </summary>
    public int NumberSickened { get; set; }
  }
}
