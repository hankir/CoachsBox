using System;
using System.Threading;
using System.Threading.Tasks;
using CoachsBox.Coaching.GroupModel;
using MediatR;

namespace CoachsBox.Coaching.Application.Commands
{
  public class EnrollStudentCommandHandler : IRequestHandler<EnrollStudentCommand, bool>
  {
    private readonly IGroupRepository groupRepository;

    public EnrollStudentCommandHandler(IGroupRepository groupRepository)
    {
      this.groupRepository = groupRepository;
    }

    public async Task<bool> Handle(EnrollStudentCommand request, CancellationToken cancellationToken)
    {
      if (request == null)
        return false;

      var groupId = request.GroupId;
      var studentId = request.StudentId;
      var trialTrainingCount = (byte)(request.IsTrialTraining ? 1 : 0);

      var group = await this.groupRepository.GetByIdAsync(groupId);
      if (group == null)
        throw new InvalidOperationException($"Group with id {groupId} not found");

      group.EnrollStudent(studentId);
      if (trialTrainingCount > 0)
        group.EnableTrialPeriod(studentId, trialTrainingCount);
      await this.groupRepository.UpdateAsync(group);
      await this.groupRepository.SaveAsync();

      return true;
    }
  }
}
