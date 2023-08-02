using CoachsBox.Coaching.Accounting.StudentContractModel;
using CoachsBox.Infrastructure;

namespace CoachsBox.Coaching.Infrastructure
{
  public class StudentContractRepository : EfRepository<StudentContract, CoachsBoxContext>, IStudentContractRepository
  {
    public StudentContractRepository(CoachsBoxContext dbContext)
      : base(dbContext)
    {
    }
  }
}
