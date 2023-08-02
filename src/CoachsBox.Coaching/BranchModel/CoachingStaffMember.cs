using System;
using System.Collections.Generic;
using System.Text;
using CoachsBox.Coaching.CoachModel;
using CoachsBox.Core;

namespace CoachsBox.Coaching.BranchModel
{
  public class CoachingStaffMember : ValueObject
  {
    public CoachingStaffMember(int coachId)
    {
      if (coachId <= 0)
        throw new ArgumentException("Id is transient", nameof(coachId));

      this.CoachId = coachId;
    }

    public CoachingStaffMember(Coach coach)
    {
      this.Coach = coach;
    }

    public int CoachId { get; private set; }

    public Coach Coach { get; private set; }

    protected override IEnumerable<object> GetAtomicValues()
    {
      yield return this.CoachId;
    }

    private CoachingStaffMember()
    {
      // Требует Entity framework core
    }
  }
}
