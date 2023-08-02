using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using CoachsBox.Core;

namespace CoachsBox.Coaching.GroupModel
{
  public class GroupByStudentsIdsSpecification : BaseSpecification<Group>
  {
    public GroupByStudentsIdsSpecification(IEnumerable<int> studentIds)
      : base(group => group.EnrolledStudents.Any(enrolled => studentIds.Contains(enrolled.StudentId)))
    {
    }
  }
}
