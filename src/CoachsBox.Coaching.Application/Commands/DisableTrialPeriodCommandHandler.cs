using System;
using System.Threading;
using System.Threading.Tasks;
using CoachsBox.Coaching.AttendanceLogModel;
using CoachsBox.Coaching.GroupModel;
using MediatR;

namespace CoachsBox.Coaching.Application.Commands
{
  public class DisableTrialPeriodCommandHandler : IRequestHandler<DisableTrialPeriodCommand, bool>
  {
    private readonly IGroupRepository groupRepository;

    public DisableTrialPeriodCommandHandler(IGroupRepository groupRepository)
    {
      this.groupRepository = groupRepository;
    }

    public async Task<bool> Handle(DisableTrialPeriodCommand request, CancellationToken cancellationToken)
    {
      if (request == null)
        throw new ArgumentNullException(nameof(request));

      var groupId = request.GroupId;
      var studentId = request.StudentId;

      var group = await this.groupRepository.GetByIdAsync(groupId);
      if (group == null)
        throw new InvalidOperationException($"Group with id {groupId} not found");

      group.DisableTrialPeriod(studentId);
      await this.groupRepository.UpdateAsync(group);
      await this.groupRepository.SaveAsync();

      return true;
    }
  }
}
