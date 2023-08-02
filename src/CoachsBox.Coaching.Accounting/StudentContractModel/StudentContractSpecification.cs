using CoachsBox.Core;

namespace CoachsBox.Coaching.Accounting.StudentContractModel
{
  public class StudentContractSpecification : BaseSpecification<StudentContract>
  {
    public StudentContractSpecification(int studentId)
      : base(contract => contract.StudentId == studentId)
    {
    }
  }
}
