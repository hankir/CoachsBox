using System.Collections.Generic;
using System.Linq;
using CoachsBox.Core;

namespace CoachsBox.Coaching.AttendanceLogModel
{
  /// <summary>
  /// Отсутствие.
  /// </summary>
  public class Absence : ValueObject
  {
    /// <summary>
    /// Получить значение отсутствия по причине отсутствия.
    /// </summary>
    /// <param name="absenceReason">Причина отсутствия.</param>
    /// <returns>Значение отсутствия или null, если отсутствия не было.</returns>
    public static Absence GetAbsence(string absenceReason)
    {
      if (string.IsNullOrEmpty(absenceReason))
        return null;

      return GetAll<Absence>().SingleOrDefault(reason => reason.Reason == absenceReason);
    }

    /// <summary>
    /// Пропуск по болезни.
    /// </summary>
    public static Absence Sickness { get { return new Absence(nameof(Sickness)); } }

    /// <summary>
    /// Пропуск без уважительной причины.
    /// </summary>
    public static Absence WithoutValidExcuse { get { return new Absence(nameof(WithoutValidExcuse)); } }

    /// <summary>
    /// Получить причину отсутствия.
    /// </summary>
    public string Reason { get; private set; }

    protected override IEnumerable<object> GetAtomicValues()
    {
      yield return this.Reason;
    }

    private Absence(string reason)
    {
      this.Reason = reason;
    }

    private Absence()
    {
      // Требует Entity framework core
    }
  }
}
