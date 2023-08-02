using System;
using System.Threading;
using System.Threading.Tasks;
using CoachsBox.Coaching.GroupModel;
using MediatR;

namespace CoachsBox.Coaching.Application.Commands
{
  public class ExcludeStudentCommandHandler : IRequestHandler<ExcludeStudentCommand, bool>
  {
    private readonly IGroupRepository groupRepository;

    public ExcludeStudentCommandHandler(IGroupRepository groupRepository)
    {
      this.groupRepository = groupRepository;
    }

    public async Task<bool> Handle(ExcludeStudentCommand request, CancellationToken cancellationToken)
    {
      if (request == null)
        return false;

      var groupId = request.GroupId;
      var studentId = request.StudentId;

      var group = await this.groupRepository.GetByIdAsync(groupId);
      if (group == null)
        throw new InvalidOperationException($"Group with id {groupId} not found");

      group.ExcludeStudent(studentId);

      await this.groupRepository.UpdateAsync(group);
      await this.groupRepository.SaveAsync();

      return true;
    }
  }
}
