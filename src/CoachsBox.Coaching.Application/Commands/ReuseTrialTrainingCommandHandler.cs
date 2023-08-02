using System;
using System.Threading;
using System.Threading.Tasks;
using CoachsBox.Coaching.GroupModel;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CoachsBox.Coaching.Application.Commands
{
  public class ReuseTrialTrainingCommandHandler : IRequestHandler<ReuseTrialTrainingCommand, bool>
  {
    private readonly IGroupRepository groupRepository;
    private readonly ILogger<ReuseTrialTrainingCommandHandler> logger;

    public ReuseTrialTrainingCommandHandler(IGroupRepository groupRepository, ILogger<ReuseTrialTrainingCommandHandler> logger)
    {
      this.groupRepository = groupRepository;
      this.logger = logger;
    }

    public async Task<bool> Handle(ReuseTrialTrainingCommand request, CancellationToken cancellationToken)
    {
      if (request == null)
        return false;

      var groupId = request.GroupId;
      var studentId = request.StudentId;
      var group = await this.groupRepository.GetByIdAsync(groupId);
      if (group == null)
        throw new InvalidOperationException($"Group with id {groupId} not found");

      if (!group.IsEnrolled(studentId))
        throw new InvalidOperationException($"Student with id {studentId} not enrolled in group with id {groupId}");

      group.ReuseTrialTraining(studentId);

      await this.groupRepository.UpdateAsync(group);
      await this.groupRepository.SaveAsync();
      this.logger.LogDebug("Enrolled student with id {StudentId} in group with id {GroupId} undo trial training and can be reuse.", studentId, groupId);
      return true;
    }
  }
}
