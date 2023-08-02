using MediatR;

namespace CoachsBox.Coaching.Application.Commands
{
  public class UseTrialTrainingCommand : IRequest<bool>
  {
    public UseTrialTrainingCommand(int groupId, int studentId)
    {
      this.GroupId = groupId;
      this.StudentId = studentId;
    }

    public int GroupId { get; }

    public int StudentId { get; }
  }
}