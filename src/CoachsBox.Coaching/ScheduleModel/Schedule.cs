using System;
using System.Collections.Generic;
using System.Linq;
using CoachsBox.Coaching.BranchModel;
using CoachsBox.Coaching.CoachModel;
using CoachsBox.Coaching.GroupModel;
using CoachsBox.Core;
using CoachsBox.Core.Primitives;

namespace CoachsBox.Coaching.ScheduleModel
{
  // Привязана к филиалу
  // Имеет отдельная запись Распиания имеет место проведения

  public class Schedule : BaseEntity
  {
    private List<TrainingTime> trainingList = new List<TrainingTime>();

    public Schedule(ScheduleSpecification scheduleSpecification, IReadOnlyList<TrainingTime> traningsTime)
    {
      this.CoachId = scheduleSpecification.CoachId;
      this.GroupId = scheduleSpecification.GroupId;
      this.BranchId = scheduleSpecification.BranchId;
      this.trainingList = traningsTime.ToList();
      this.TrainingLocation = TrainingLocation.NotDefined;
    }

    /// <summary>
    /// Заменить тренера.
    /// </summary>
    /// <param name="coachId">Идентификатор нового тренера.</param>
    public void ReplaceCoachTo(int coachId)
    {
      if (coachId <= 0)
        throw new ArgumentException("Id is transient", nameof(coachId));

      this.CoachId = coachId;
      this.Coach = null;
    }

    /// <summary>
    /// Получить идентификатор тренера.
    /// </summary>
    public int CoachId { get; private set; }

    /// <summary>
    /// Получить тренера.
    /// </summary>
    public Coach Coach { get; private set; }

    /// <summary>
    /// Получить идентификатор группы.
    /// </summary>
    public int GroupId { get; private set; }

    /// <summary>
    /// Получить группу.
    /// </summary>
    public Group Group { get; private set; }

    /// <summary>
    /// Получить идентификатор филиала, для которого составлено расписание.
    /// </summary>
    public int BranchId { get; private set; }

    /// <summary>
    /// Получить филиал.
    /// </summary>
    public Branch Branch { get; private set; }

    /// <summary>
    /// Получить место проведения тренировок.
    /// </summary>
    public TrainingLocation TrainingLocation { get; private set; }

    public IReadOnlyCollection<TrainingTime> TrainingList => this.trainingList;

    public void AddTraningTime(Date date, TimeOfDay start, TimeOfDay end)
    {
      var newTrainingTime = new TrainingTime(date, start, end);
      if (this.TrainingList.Contains(newTrainingTime))
        throw new ArgumentException("Traning time already exist", nameof(date));

      this.trainingList.Add(newTrainingTime);
    }

    public void UpdateTrainings(IReadOnlyList<TrainingTime> newTrainingList)
    {
      this.trainingList.Clear();
      foreach (var newTrainingTime in newTrainingList)
        this.trainingList.Add(newTrainingTime);
    }

    private Schedule()
    {
      // Требует Entity framework core
    }
  }
}
