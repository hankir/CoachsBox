using CoachsBox.Coaching.AttendanceLogModel;
using CoachsBox.Core.Primitives;

namespace CoachsBox.Coaching.Application
{
  public interface IAccrualService
  {
    void CalculateAccrualForDate(Date date);

    void CalculateAccrualForPeriod(Date from, Date to);

    void ProcessAccruals();

    /// <summary>
    /// Проверить является ли отметка посещаемости подотчетная для начислений.
    /// </summary>
    /// <param name="isTrialTraining">Является ли тренировка пробной.</param>
    /// <param name="absence">Отсутсвие.</param>
    /// <returns>True если отметка посещаемости является подотчетной, иначе - false.</returns>
    bool IsAttendanceLogEntryAccountable(bool isTrialTraining, Absence absence);

    /// <summary>
    /// Посчитать количество подотчетных тренировок по статистике посещений.
    /// </summary>
    /// <returns>Количество подотчетных тренировок.</returns>
    int AccountableTrainingCount(AttendanceStatisticsInfo attendanceStatistics);
  }
}
