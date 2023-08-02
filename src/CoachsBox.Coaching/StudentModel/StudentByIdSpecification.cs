using System;
using System.Collections.Generic;
using System.Linq;
using CoachsBox.Core;

namespace CoachsBox.Coaching.StudentModel
{
  public class StudentByIdSpecification : BaseSpecification<Student>
  {
    public StudentByIdSpecification(int studentId)
      : base(student => student.Id == studentId)
    {
      this.AddInclude(student => student.Person);
    }

    public StudentByIdSpecification(IEnumerable<int> studentIds)
      : base(student => studentIds.Contains(student.Id))
    {
      this.AddInclude(student => student.Person);
    }

    public StudentByIdSpecification WithRelatives()
    {
      this.AddInclude($"{nameof(Student.Relatives)}.{nameof(Relative.Person)}");
      return this;
    }
  }
}
