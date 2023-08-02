using System;
using System.Collections.Generic;
using System.Linq;
using CoachsBox.Core;

namespace CoachsBox.Coaching.Accounting.StudentAccountModel
{
  public class StudentsAccountsByStudentIdsSpecification : BaseSpecification<StudentAccount>
  {
    public StudentsAccountsByStudentIdsSpecification(IEnumerable<int> studentIds)
      : base(studentAccount => studentIds.Contains(studentAccount.StudentId))
    {
      this.AddInclude(studentAccount => studentAccount.Entries);
    }
  }
}
