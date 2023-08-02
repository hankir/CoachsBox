using System;
using System.Threading;
using System.Threading.Tasks;
using CoachsBox.Coaching.GroupModel;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CoachsBox.Coaching.Application.Commands
{
  public class UseTrialTrainingCommandHandler : IRequestHandler<UseTrialTrainingCommand, bool>
  {
    private readonly IGroupRepository groupRepository;
    private readonly ILogger<UseTrialTrainingCommandHandler> logger;

    public UseTrialTrainingCommandHandler(IGroupRepository groupRepository, ILogger<UseTrialTrainingCommandHandler> logger)
    {
      this.groupRepository = groupRepository;
      this.logger = logger;
    }

    public async Task<bool> Handle(UseTrialTrainingCommand request, CancellationToken cancellationToken)
    {
      if (request == null)
        return false;

      var groupId = request.GroupId;
      var studentId = request.StudentId;
      var group = await this.groupRepository.GetByIdAsync(groupId);
      if (group == null)
        throw new InvalidOperationException($"Group with id {groupId} not found");

      group.UseTrialTraining(studentId);

      await this.groupRepository.UpdateAsync(group);
      await this.groupRepository.SaveAsync();
      this.logger.LogDebug("Enrolled student with id {StudentId} in group with id {GroupId} use trial training.", studentId, groupId);
      return true;
    }
  }
}
