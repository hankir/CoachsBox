using System;
using System.Collections.Generic;
using System.Linq;
using CoachsBox.Coaching.GroupModel;
using CoachsBox.Core;
using CoachsBox.Core.Primitives;

namespace CoachsBox.Coaching.AttendanceLogModel
{
  public class AttendanceLog : BaseEntity
  {
    private List<AttendanceLogEntry> entries = new List<AttendanceLogEntry>();

    /// <summary>
    /// Создать журнал посещаемости группы.
    /// </summary>
    /// <param name="groupId">Идентификатор группы.</param>
    public AttendanceLog(int groupId, int year)
    {
      this.GroupId = groupId;
      this.Year = year;
    }

    /// <summary>
    /// Получить идентификатор группы.
    /// </summary>
    public int GroupId { get; private set; }

    /// <summary>
    /// Получить группу, для которой ведется журнал посещаемости.
    /// </summary>
    public Group Group { get; private set; }

    /// <summary>
    /// Получить учебный год, за который ведется журнал посещаемости.
    /// </summary>
    public int Year { get; private set; }

    /// <summary>
    /// Получить записи журнала посещаемости.
    /// </summary>
    public IReadOnlyCollection<AttendanceLogEntry> Entries => this.entries;

    public void MarkStudent(int studentId, int coachId, Date date, TimeOfDay start, TimeOfDay end)
    {
      this.MarkStudent(studentId, coachId, date, start, end, false);
    }

    public void MarkStudent(int studentId, int coachId, Date date, TimeOfDay start, TimeOfDay end, bool isTrialTraining)
    {
      if (date.Year != this.Year)
        throw new ArgumentException("Attendance date year not match for this log year", nameof(date));

      var entry = new AttendanceLogEntry(studentId, coachId, date, start, end, isTrialTraining);
      if (this.entries.Contains(entry))
        throw new InvalidOperationException("AttendanceLogEntry exists");

      this.entries.Add(entry);

      this.AddDomainEvent(new StudentMarkedEvent(
        this.GroupId,
        entry.StudentId,
        entry.CoachId,
        entry.WhenStarted(),
        entry.WhenEnded(),
        entry.IsTrialTraining,
        string.Empty));
    }

    public void MarkStudent(int studentId, int coachId, Date date, TimeOfDay start, TimeOfDay end, Absence absenceReason)
    {
      if (date.Year != this.Year)
        throw new ArgumentException("Attendance date year not match for this log year", nameof(date));

      var entry = new AttendanceLogEntry(studentId, coachId, date, start, end, absenceReason);
      if (this.entries.Contains(entry))
        throw new InvalidOperationException("AttendanceLogEntry exists");

      this.entries.Add(entry);

      this.AddDomainEvent(new StudentMarkedEvent(
        this.GroupId,
        entry.StudentId,
        entry.CoachId,
        entry.WhenStarted(),
        entry.WhenEnded(),
        entry.IsTrialTraining,
        entry.AbsenceReason.Reason));
    }

    public void ClearMark(AttendanceLogEntry entry)
    {
      if (this.entries.Contains(entry))
      {
        this.entries.Remove(entry);

        var haveAttendanceInMonth = this.HaveAttendanceInMonth(entry.Date.Month.Number, entry.StudentId);
        this.AddDomainEvent(new StudentMarkClearedEvent(
          this.GroupId,
          entry.StudentId,
          entry.CoachId,
          entry.WhenStarted(),
          entry.WhenEnded(),
          entry.IsTrialTraining,
          entry.AbsenceReason?.Reason,
          haveAttendanceInMonth));
      }
    }

    public AttendanceLogEntry MarkExists(int studentId, Date date, TimeOfDay start, TimeOfDay end)
    {
      return this.entries.SingleOrDefault(entry =>
        entry.StudentId == studentId &&
        entry.Date.Equals(date) &&
        entry.Start.Equals(start) &&
        entry.End.Equals(end));
    }

    /// <summary>
    /// Проверить есть ли у студента отметка о пробной тренировке.
    /// </summary>
    /// <param name="studentId">Идентификатор студента.</param>
    /// <returns>True, если пробное занятие было у студента, иначе - false.</returns>
    public bool HasTrialTrainingMark(int studentId)
    {
      return this.entries.Any(entry => entry.StudentId == studentId && entry.IsTrialTraining);
    }

    /// <summary>
    /// Проверить есть ли посещения студента в указанном месяце.
    /// </summary>
    /// <param name="month">Месяц.</param>
    /// <param name="studentId">Идентификатор студента.</param>
    /// <returns>True, если посещения были, иначе - false.</returns>
    public bool HaveAttendanceInMonth(int month, int studentId)
    {
      return this.Entries
        .Where(attendance => attendance.StudentId == studentId)
        .Where(attendance => attendance.AbsenceReason == null)
        .Any(attendance => attendance.Date.Month.Number == month);
    }

    /// <summary>
    /// Проверить есть ли посещения студента в указанном месяце.
    /// </summary>
    /// <param name="month">Месяц.</param>
    /// <param name="studentId">Идентификатор студента.</param>
    /// <returns>True, если посещения были, иначе - false.</returns>
    public IReadOnlyCollection<AttendanceLogEntry> GetAttendanceInMonth(int month, int studentId)
    {
      return this.Entries
        .Where(attendance => attendance.StudentId == studentId)
        .Where(attendance => attendance.Date.Month.Number == month)
        .ToList();
    }

    /// <summary>
    /// Получить посещения студентов за определенный период.
    /// </summary>
    /// <param name="studentId">Идентификатор студента.</param>
    /// <param name="date">Дата, на которую нужно получить отметку посещаемости.</param>
    /// <returns>Коллекция посещений.</returns>
    public IReadOnlyCollection<AttendanceLogEntry> GetAttendanceByDate(int studentId, Date date)
    {
      return this.Entries
        .Where(attendance => attendance.StudentId == studentId)
        .Where(attendance => attendance.Date.Equals(date))
        .ToList();
    }

    /// <summary>
    /// Получить посещения студентов за определенный период.
    /// </summary>
    /// <param name="from">Начало периода.</param>
    /// <param name="to">Конец периода.</param>
    /// <returns>Коллекция посещений.</returns>
    public IReadOnlyCollection<AttendanceLogEntry> GetAttendanceByDate(DateTime from, DateTime to)
    {
      return this.Entries
        .Where(attendance => attendance.WhenStarted() >= from && attendance.WhenEnded() <= to)
        .ToList();
    }

    /// <summary>
    /// Получить отметку посещаемости для студента.
    /// </summary>
    /// <param name="studentId">Идентификатор студента.</param>
    /// <returns>Коллекция посещений.</returns>
    public IReadOnlyCollection<AttendanceLogEntry> GetStudentAttendance(int studentId)
    {
      return this.Entries
        .Where(attendance => attendance.StudentId == studentId)
        .Where(attendance => attendance.AbsenceReason == null)
        .ToList();
    }

    private AttendanceLog()
    {
      // Требует Entity framework core
    }
  }
}
