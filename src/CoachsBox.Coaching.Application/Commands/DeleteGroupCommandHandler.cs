using System.Threading;
using System.Threading.Tasks;
using CoachsBox.Coaching.AttendanceLogModel;
using CoachsBox.Coaching.GroupModel;
using CoachsBox.Coaching.ScheduleModel;
using CoachsBox.Core.Interfaces;
using MediatR;

namespace CoachsBox.Coaching.Application.Commands
{
  public class DeleteGroupCommandHandler : IRequestHandler<DeleteGroupCommand, bool>
  {
    private readonly IGroupRepository groupRepository;
    private readonly IScheduleRepository scheduleRepository;
    private readonly IAttendanceLogRepository attendanceLogRepository;
    private readonly IUnitOfWork unitOfWork;

    public DeleteGroupCommandHandler(
      IGroupRepository groupRepository,
      IScheduleRepository scheduleRepository,
      IAttendanceLogRepository attendanceLogRepository,
      IUnitOfWork unitOfWork)
    {
      this.groupRepository = groupRepository;
      this.scheduleRepository = scheduleRepository;
      this.attendanceLogRepository = attendanceLogRepository;
      this.unitOfWork = unitOfWork;
    }

    public async Task<bool> Handle(DeleteGroupCommand request, CancellationToken cancellationToken)
    {
      if (request == null)
        return false;

      var groupId = request.GroupId;
      var group = await this.groupRepository.GetByIdAsync(groupId);
      if (group == null)
        return false;

      // Удаление журналов посещаемости групп.
      var attendanceLogSpecification = new GroupAttendanceSpecification(groupId);
      var groupAttendanceLogs = await this.attendanceLogRepository.ListAsync(attendanceLogSpecification);
      foreach (var attendanceLog in groupAttendanceLogs)
      {
        await this.attendanceLogRepository.DeleteAsync(attendanceLog);
      }

      // Удаление расписания группы.
      var scheduleSpecification = new ScheduleByGroupSpecification(groupId);
      var groupSchedules = await this.scheduleRepository.ListAsync(scheduleSpecification);
      foreach (var schedule in groupSchedules)
      {
        await this.scheduleRepository.DeleteAsync(schedule);
      }

      // Удаление самой группы.
      await this.groupRepository.DeleteAsync(group);

      await this.unitOfWork.SaveAsync();

      return true;
    }
  }
}
