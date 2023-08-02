using System;
using System.Collections.Generic;
using System.Linq;
using CoachsBox.Core;

namespace CoachsBox.Coaching.GroupModel
{
  public class GroupByIdSpecification : BaseSpecification<Group>
  {
    public GroupByIdSpecification(int groupId)
      : base(group => group.Id == groupId)
    {
    }

    public GroupByIdSpecification(IEnumerable<int> groupIds)
      : base(group => groupIds.Contains(group.Id))
    {
    }

    public GroupByIdSpecification WithBranch()
    {
      this.AddInclude(group => group.Branch);
      return this;
    }

    public GroupByIdSpecification WithStudents()
    {
      this.AddInclude($"{nameof(Group.EnrolledStudents)}.{nameof(EnrolledStudent.Student)}.{nameof(EnrolledStudent.Student.Person)}");
      return this;
    }
  }
}
