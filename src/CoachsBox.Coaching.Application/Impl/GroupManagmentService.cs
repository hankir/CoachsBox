using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoachsBox.Coaching.GroupModel;
using CoachsBox.Coaching.ScheduleModel;

namespace CoachsBox.Coaching.Application.Impl
{
  public class GroupManagmentService : IGroupManagmentService
  {
    private readonly IGroupRepository groupRepository;
    private readonly IScheduleRepository scheduleRepository;

    public GroupManagmentService(
      IGroupRepository groupRepository,
      IScheduleRepository scheduleRepository)
    {
      this.groupRepository = groupRepository;
      this.scheduleRepository = scheduleRepository;
    }

    public int CreateNewGroup(int branchId, string name, Sport sport, int minAge, int maxAge)
    {
      var trainingProgram = new TrainingProgramSpecification(minAge, maxAge);
      var group = new Group(branchId, name, sport, trainingProgram);
      this.groupRepository.AddAsync(group).Wait();
      this.groupRepository.SaveAsync().Wait();
      return group.Id;
    }

    public IReadOnlyCollection<Group> ListCoachGroups(int coachId)
    {
      var coachSchedule = this.scheduleRepository.ListAsync(new ScheduleByCoachSpecification(coachId)).Result;
      return this.groupRepository.ListAsync(new GroupByIdSpecification(coachSchedule.Select(schedule => schedule.GroupId))).Result.ToList();
    }
  }
}
