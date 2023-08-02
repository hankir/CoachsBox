using System;
using System.Collections.Generic;
using CoachsBox.Coaching.CoachModel;
using CoachsBox.Coaching.StudentModel;
using CoachsBox.Core;
using CoachsBox.Core.Primitives;

namespace CoachsBox.Coaching.AttendanceLogModel
{
  /// <summary>
  /// Отметка посещаемости студента.
  /// </summary>
  public class AttendanceLogEntry : ValueObject
  {
    /// <summary>
    /// Создать отметку посещаемости для студента.
    /// </summary>
    /// <param name="studentId">Идентификатор студента.</param>
    /// <param name="coachId">Идентификатор тренера, проводившего тренировку.</param>
    /// <param name="date">Дата посещения.</param>
    /// <param name="start">Начало тренировки.</param>
    /// <param name="end">Конец тренировки.</param>
    public AttendanceLogEntry(int studentId, int coachId, Date date, TimeOfDay start, TimeOfDay end)
    {
      this.StudentId = studentId;
      this.CoachId = coachId;
      this.Date = date;
      this.Start = start;
      this.End = end;
    }

    /// <summary>
    /// Создать отметку посещаемости для студента.
    /// </summary>
    /// <param name="studentId">Идентификатор студента.</param>
    /// <param name="coachId">Идентификатор тренера, проводившего тренировку.</param>
    /// <param name="date">Дата посещения.</param>
    /// <param name="start">Начало тренировки.</param>
    /// <param name="end">Конец тренировки.</param>
    /// <param name="isTrialTraining">Признак отметки посещения по пробному занятию.</param>
    public AttendanceLogEntry(int studentId, int coachId, Date date, TimeOfDay start, TimeOfDay end, bool isTrialTraining)
    {
      this.StudentId = studentId;
      this.CoachId = coachId;
      this.Date = date;
      this.Start = start;
      this.End = end;
      this.IsTrialTraining = isTrialTraining;
    }

    /// <summary>
    /// Создать отметку посещаемости для студента с причиной отсутствия.
    /// </summary>
    /// <param name="studentId">Идентификатор студента.</param>
    /// <param name="coachId">Идентификатор тренера, проводившего тренировку.</param>
    /// <param name="date">Дата посещения.</param>
    /// <param name="start">Начало тренировки.</param>
    /// <param name="end">Конец тренировки.</param>
    /// <param name="absenceReason">Причина отсутствия.</param>
    public AttendanceLogEntry(int studentId, int coachId, Date date, TimeOfDay start, TimeOfDay end, Absence absenceReason)
    {
      if (date?.Equals(Date.Empty) == true)
        throw new InvalidOperationException("Attendance date can not be empty");

      this.StudentId = studentId;
      this.CoachId = coachId;
      this.Date = date;
      this.Start = start;
      this.End = end;
      this.AbsenceReason = absenceReason;
    }

    /// <summary>
    /// Получить журнала посещаемости.
    /// </summary>
    public AttendanceLog AttendanceLog { get; private set; }

    /// <summary>
    /// Получить студента, для которого создана эта отметка посещаемости.
    /// </summary>
    public Student Student { get; private set; }

    /// <summary>
    /// Получить идентификатор студента.
    /// </summary>
    public int StudentId { get; private set; }

    /// <summary>
    /// Получить тренера, кто проводил тренировку.
    /// </summary>
    public Coach Coach { get; private set; }

    /// <summary>
    /// Получить идентификатор тренера, который проводил тренировку.
    /// </summary>
    public int CoachId { get; private set; }

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
    /// Получить причину отсутствия.
    /// </summary>
    public Absence AbsenceReason { get; private set; }

    /// <summary>
    /// Получить примечание к отметке посещаемости.
    /// </summary>
    public string Description { get; private set; }

    /// <summary>
    /// Получить признак того, что отметка посещаемости была по пробному занятию.
    /// </summary>
    public bool IsTrialTraining { get; private set; }

    /// <summary>
    /// Получить дату и время начала тренировки.
    /// </summary>
    /// <returns>Дата и время начала тренировки.</returns>
    public DateTime WhenStarted()
    {
      return new DateTime(this.Date.Year, this.Date.Month.Number, this.Date.Day, this.Start.Hours, this.Start.Minutes, 0);
    }

    /// <summary>
    /// Получить дату и время конца тренировки.
    /// </summary>
    /// <returns>Дата и время конца тренировки.</returns>
    public DateTime WhenEnded()
    {
      return new DateTime(this.Date.Year, this.Date.Month.Number, this.Date.Day, this.End.Hours, this.End.Minutes, 0);
    }

    protected override IEnumerable<object> GetAtomicValues()
    {
      yield return this.CoachId;
      yield return this.StudentId;
      yield return this.Date;
      yield return this.Start;
      yield return this.End;
      yield return this.AbsenceReason;
      yield return this.Description;
    }

    private AttendanceLogEntry()
    {
      // Требует Entity framework core
    }
  }
}
