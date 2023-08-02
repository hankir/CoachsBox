using CoachsBox.Core;

namespace CoachsBox.Coaching.ScheduleModel
{
  public class ScheduleSpecification : BaseSpecification<Schedule>
  {
    public ScheduleSpecification(int coachId, int groupId, int branchId)
      : base(schedule =>
         schedule.CoachId == coachId &&
         schedule.GroupId == groupId &&
         schedule.BranchId == branchId)
    {
      this.CoachId = coachId;
      this.GroupId = groupId;
      this.BranchId = branchId;
    }

    public ScheduleSpecification(int groupId, int branchId)
      : base(schedule =>
         schedule.GroupId == groupId &&
         schedule.BranchId == branchId)
    {
      this.GroupId = groupId;
      this.BranchId = branchId;
    }

    public ScheduleSpecification(int groupId)
      : base(schedule => schedule.GroupId == groupId)
    {
      this.GroupId = groupId;
    }

    public int CoachId { get; }

    public int GroupId { get; }

    public int BranchId { get; }

    public ScheduleSpecification WithCoach()
    {
      this.AddInclude(schedule => schedule.Coach);
      this.AddInclude(schedule => schedule.Coach.Person);
      return this;
    }

    public ScheduleSpecification WithGroup()
    {
      this.AddInclude(schedule => schedule.Group);
      return this;
    }
  }
}
