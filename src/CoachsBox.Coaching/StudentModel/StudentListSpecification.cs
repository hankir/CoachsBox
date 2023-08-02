using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using CoachsBox.Core;

namespace CoachsBox.Coaching.StudentModel
{
  public class StudentListSpecification : BaseSpecification<Student>
  {
    public StudentListSpecification(IEnumerable<int> studentsQuery)
      : base(student => studentsQuery.Contains(student.Id))
    {
    }

    protected StudentListSpecification(Expression<Func<Student, bool>> criteria)
      : base(criteria)
    {
    }

    public StudentListSpecification() : base(student => true)
    {
    }

    public StudentListSpecification SortBySurname()
    {
      this.ApplyOrderBy(student => student.Person.Name.Surname);
      return this;
    }

    public StudentListSpecification SortByName()
    {
      this.ApplyOrderBy(student => student.Person.Name.Name);
      return this;
    }

    public StudentListSpecification SortByPatronymic()
    {
      this.ApplyOrderBy(student => student.Person.Name.Patronymic);
      return this;
    }

    public StudentListSpecification SortByBirthday()
    {
      this.ApplyOrderBy(student => student.Person.Birthday.Month.Number * 31 + student.Person.Birthday.Day);
      return this;
    }

    public StudentListSpecification WithPerson()
    {
      this.AddInclude(student => student.Person);
      return this;
    }
  }
}
