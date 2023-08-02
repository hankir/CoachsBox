using System.Collections.Generic;
using CoachsBox.Coaching.ScheduleModel;
using CoachsBox.Core.Primitives;

namespace CoachsBox.Coaching.Application
{
  public interface ISchedulingService
  {
    /// <summary>
    /// Создать расписание.
    /// </summary>
    /// <param name="scheduleSpecification">Спецификация расписания.</param>
    /// <param name="trainingList">Список тренировочных дней.</param>
    int CreateSchedule(ScheduleSpecification scheduleSpecification, IReadOnlyList<TrainingTime> trainingList);

    /// <summary>
    /// Получить расписание на день для тренера.
    /// </summary>
    /// <param name="coachId">Идентификатор тренера.</param>
    /// <param name="day">Требуемый день.</param>
    /// <returns>Список расписаний на день.</returns>
    IReadOnlyList<Schedule> ListScheduleOnDate(int coachId, Date day);

    /// <summary>
    /// Получить расписание на день для тренера.
    /// </summary>
    /// <param name="day">Требуемый день.</param>
    /// <returns>Список расписаний на день.</returns>
    IReadOnlyList<Schedule> ListScheduleOnDate(Date day);

    /// <summary>
    /// Обновить расписание.
    /// </summary>
    /// <param name="scheduleId">Идентификатор расписания.</param>
    /// <param name="trainingList">Список тренировочных дней.</param>
    /// <param name="coachId">Идентификатор тренера.</param>
    void UpdateTraining(int scheduleId, IReadOnlyList<TrainingTime> trainingList, int coachId);
  }
}
