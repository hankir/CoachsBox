using CoachsBox.Core;

namespace CoachsBox.Coaching.Accounting.StudentAccountModel
{
  public class StudentAccountSpecification : BaseSpecification<StudentAccount>
  {
    public StudentAccountSpecification(int studentId)
      : base(studentAccount => studentAccount.StudentId == studentId)
    {
    }
  }
}
