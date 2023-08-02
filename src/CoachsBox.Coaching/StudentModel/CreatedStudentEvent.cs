using MediatR;

namespace CoachsBox.Coaching.StudentModel
{
  public class CreatedStudentEvent : INotification
  {
    public CreatedStudentEvent(Student student)
    {
      this.Student = student;
    }

    public Student Student { get; }
  }
}
