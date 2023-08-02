using System;
using System.Collections.Generic;
using System.Linq;
using CoachsBox.Core;

namespace CoachsBox.Coaching.StudentModel
{
  public class BirthdayStudentListSpecification : StudentListSpecification
  {
    public static BirthdayStudentListSpecification CreateForToday(IEnumerable<int> studentIds = null)
    {
      var nowDate = Watch.Now.DateTime;
      return studentIds == null ?
        new BirthdayStudentListSpecification(nowDate.Month, nowDate.Day) :
        new BirthdayStudentListSpecification(nowDate.Month, nowDate.Day, studentIds);
    }

    public static BirthdayStudentListSpecification CreateForTomorrow(IEnumerable<int> studentIds = null)
    {
      var nowDate = Watch.Now.DateTime.AddDays(1);
      return studentIds == null ?
        new BirthdayStudentListSpecification(nowDate.Month, nowDate.Day) :
        new BirthdayStudentListSpecification(nowDate.Month, nowDate.Day, studentIds);
    }

    public static BirthdayStudentListSpecification CreateForThisWeek(IEnumerable<int> studentIds = null)
    {
      var nowDate = Watch.Now.DateTime;
      int diff = (7 + (nowDate.DayOfWeek - DayOfWeek.Monday)) % 7;
      var startWeek = nowDate.AddDays(-1 * diff).Date;

      return studentIds == null ?
        new BirthdayStudentListSpecification(startWeek, startWeek.AddDays(6)) :
        new BirthdayStudentListSpecification(startWeek, startWeek.AddDays(6), studentIds);
    }

    public static BirthdayStudentListSpecification CreateForThisMonth(IEnumerable<int> studentIds = null)
    {
      var nowDate = Watch.Now.DateTime;
      return studentIds == null ?
        new BirthdayStudentListSpecification(nowDate.Month) :
        new BirthdayStudentListSpecification(nowDate.Month, studentIds);
    }

    public BirthdayStudentListSpecification(int month, int day)
      : base(student => student.Person.Birthday.Month.Number == month && student.Person.Birthday.Day == day)
    {
    }

    public BirthdayStudentListSpecification(int month, int day, IEnumerable<int> studentIds)
      : base(student => studentIds.Contains(student.Id) && student.Person.Birthday.Month.Number == month && student.Person.Birthday.Day == day)
    {
    }

    public BirthdayStudentListSpecification(int month)
      : base(student => student.Person.Birthday.Month.Number == month)
    {
    }

    public BirthdayStudentListSpecification(int month, IEnumerable<int> studentIds)
      : base(student => studentIds.Contains(student.Id) && student.Person.Birthday.Month.Number == month)
    {
    }

    public BirthdayStudentListSpecification(DateTime from, DateTime to) : base(student =>
      ((
        (from.Year * 365 + student.Person.Birthday.Month.Number * 30 + student.Person.Birthday.Day) >= (from.Year * 365 + from.Month * 30 + from.Day) &&
        (to.Year * 365 + student.Person.Birthday.Month.Number * 30 + student.Person.Birthday.Day) <= (to.Year * 365 + to.Month * 30 + to.Day)
      )
      ||
      (
        (to.Year * 365 + student.Person.Birthday.Month.Number * 30 + student.Person.Birthday.Day) >= (to.Year * 365 + to.Month * 30 + to.Day) &&
        (to.Year * 365 + student.Person.Birthday.Month.Number * 30 + student.Person.Birthday.Day) <= (to.Year * 365 + to.Month * 30 + to.Day)
      ))
      &&
      !(student.Person.Birthday.Year == 1 && student.Person.Birthday.Month.Number == 1 && student.Person.Birthday.Day == 1)
    )
    {
    }

    public BirthdayStudentListSpecification(DateTime from, DateTime to, IEnumerable<int> studentIds) : base(student =>
      studentIds.Contains(student.Id) &&
      ((
        (from.Year * 365 + student.Person.Birthday.Month.Number * 30 + student.Person.Birthday.Day) >= (from.Year * 365 + from.Month * 30 + from.Day) &&
        (to.Year * 365 + student.Person.Birthday.Month.Number * 30 + student.Person.Birthday.Day) <= (to.Year * 365 + to.Month * 30 + to.Day)
      )
      ||
      (
        (to.Year * 365 + student.Person.Birthday.Month.Number * 30 + student.Person.Birthday.Day) >= (to.Year * 365 + to.Month * 30 + to.Day) &&
        (to.Year * 365 + student.Person.Birthday.Month.Number * 30 + student.Person.Birthday.Day) <= (to.Year * 365 + to.Month * 30 + to.Day)
      ))
      &&
      !(student.Person.Birthday.Year == 1 && student.Person.Birthday.Month.Number == 1 && student.Person.Birthday.Day == 1)
    )
    {
    }
  }
}
