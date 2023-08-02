using System;
using System.Collections.Generic;
using CoachsBox.Coaching.PersonModel;
using CoachsBox.Coaching.StudentModel;

namespace CoachsBox.WebApp.Pages.Students.Components
{
  /// <summary>
  /// Фильтр списка студентов.
  /// </summary>
  public class StudentsListFilter
  {
    /// <summary>
    /// Создать фильтр списка по имени студента.
    /// </summary>
    /// <param name="studentName">Имя студента одной строкой в формате ФИО.</param>
    /// <returns>Фильтр списка студентов.</returns>
    public static StudentsListFilter CreateByStudentName(string studentName)
    {
      return new StudentsListFilter()
      {
        StudentName = studentName
      };
    }

    /// <summary>
    /// Создать фильтр списка студентов, дни рождения которых попадают в указанный период.
    /// </summary>
    /// <param name="from">Начало периода.</param>
    /// <param name="to">Конец периода.</param>
    /// <returns>Фильтр списка студентов.</returns>
    public static StudentsListFilter CreateCustomBirthdayPeriod(DateTime from, DateTime to)
    {
      return new StudentsListFilter()
      {
        BirthdayPeriod = Components.BirthdayPeriod.Custom,
        BirthdayFrom = from,
        BirthdayTo = to
      };
    }

    /// <summary>
    /// Создать фильтр списка студентов, дни рождения которых попадают в указанный период.
    /// </summary>
    /// <param name="birthdayPeriod">Предопределенный период дней рождений.</param>
    /// <returns>Фильтр списка студентов.</returns>
    public static StudentsListFilter CreateByBirthdayPeriod(BirthdayPeriod birthdayPeriod)
    {
      if (birthdayPeriod == Components.BirthdayPeriod.Custom)
        throw new ArgumentException($"For custom birthday period use {nameof(CreateCustomBirthdayPeriod)} creation method", nameof(birthdayPeriod));

      return new StudentsListFilter()
      {
        BirthdayPeriod = birthdayPeriod
      };
    }

    /// <summary>
    /// Получить или установить идентификатор фильтра.
    /// </summary>
    public Guid FilterId { get; set; }

    /// <summary>
    /// Получить или установить период дней рождения.
    /// </summary>
    public BirthdayPeriod? BirthdayPeriod { get; set; }

    /// <summary>
    /// Получить или установить начало периода дней рождения.
    /// </summary>
    public DateTime? BirthdayFrom { get; set; }

    /// <summary>
    /// Получить или установить конец периода дней рождения.
    /// </summary>
    public DateTime? BirthdayTo { get; set; }

    /// <summary>
    /// Получить или установить имя студента одной строкой в формате ФИО.
    /// </summary>
    public string StudentName { get; set; }

    /// <summary>
    /// Получить признак, того, что нужно показывать только должников.
    /// </summary>
    public bool IsShowDebtsOnly { get; set; }

    /// <summary>
    /// Создать спецификацию списка.
    /// </summary>
    /// <returns>Спецификация списка.</returns>
    public StudentListSpecification CreateSpecification(IEnumerable<int> studentIds = null)
    {
      var isStudentIdsSpecified = studentIds != null;

      if (!string.IsNullOrWhiteSpace(this.StudentName))
      {
        if (PersonName.TryParse(this.StudentName, out var personName))
          return isStudentIdsSpecified ?
            new FindStudentsByNameSpecification(personName, studentIds) :
            new FindStudentsByNameSpecification(personName);
      }

      if (this.BirthdayPeriod != null)
      {
        switch (this.BirthdayPeriod.Value)
        {
          case Components.BirthdayPeriod.Today:
            return BirthdayStudentListSpecification.CreateForToday(studentIds);
          case Components.BirthdayPeriod.Tomorrow:
            return BirthdayStudentListSpecification.CreateForTomorrow(studentIds);
          case Components.BirthdayPeriod.ThisWeek:
            return BirthdayStudentListSpecification.CreateForThisWeek(studentIds);
          case Components.BirthdayPeriod.ThisMonth:
            return BirthdayStudentListSpecification.CreateForThisMonth(studentIds);
          case Components.BirthdayPeriod.Custom:
            return isStudentIdsSpecified ?
              new BirthdayStudentListSpecification(this.BirthdayFrom.Value, this.BirthdayTo.Value, studentIds) :
              new BirthdayStudentListSpecification(this.BirthdayFrom.Value, this.BirthdayTo.Value);
        }
      }

      return isStudentIdsSpecified ? new StudentListSpecification(studentIds) : new StudentListSpecification();
    }

    public StudentsListFilter()
    {
      this.FilterId = Guid.NewGuid();
    }
  }

  /// <summary>
  /// Период дней рождения.
  /// </summary>
  public enum BirthdayPeriod
  {
    Today,
    Tomorrow,
    ThisWeek,
    ThisMonth,
    Custom
  }
}
