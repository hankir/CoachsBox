using System;
using System.Threading;
using System.Threading.Tasks;
using CoachsBox.Coaching.AttendanceLogModel;
using CoachsBox.Coaching.GroupModel;
using MediatR;

namespace CoachsBox.Coaching.Application.Commands
{
  public class EnableTrialPeriodCommandHandler : IRequestHandler<EnableTrialPeriodCommand, bool>
  {
    private readonly IGroupRepository groupRepository;
    private readonly IAttendanceLogRepository attendanceLogRepository;

    public EnableTrialPeriodCommandHandler(
      IGroupRepository groupRepository,
      IAttendanceLogRepository attendanceLogRepository)
    {
      this.groupRepository = groupRepository;
      this.attendanceLogRepository = attendanceLogRepository;
    }

    public async Task<bool> Handle(EnableTrialPeriodCommand request, CancellationToken cancellationToken)
    {
      if (request == null)
        throw new ArgumentNullException(nameof(request));

      var groupId = request.GroupId;
      var studentId = request.StudentId;

      var group = await this.groupRepository.GetByIdAsync(groupId);
      if (group == null)
        throw new InvalidOperationException($"Group with id {groupId} not found");

      if (!this.attendanceLogRepository.HasTrialTrainingMark(groupId, studentId))
      {
        group.EnableTrialPeriod(studentId, 1);
        await this.groupRepository.UpdateAsync(group);
        await this.groupRepository.SaveAsync();
      }

      return true;
    }
  }
}
