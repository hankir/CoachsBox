using MediatR;

namespace CoachsBox.Coaching.Application.Commands
{
  public class EnrollStudentCommand : IRequest<bool>
  {
    public EnrollStudentCommand(int groupId, int studentId, bool isTrialTraining)
    {
      this.GroupId = groupId;
      this.StudentId = studentId;
      this.IsTrialTraining = isTrialTraining;
    }

    public int GroupId { get; }

    public int StudentId { get; }

    public bool IsTrialTraining { get; }
  }
}
