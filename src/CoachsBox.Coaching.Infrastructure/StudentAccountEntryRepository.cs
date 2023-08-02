using CoachsBox.Coaching.Accounting.StudentAccountEntryModel;
using CoachsBox.Infrastructure;

namespace CoachsBox.Coaching.Infrastructure
{
  public class StudentAccountEntryRepository : EfRepository<StudentAccountEntry, CoachsBoxContext>, IStudentAccountEntryRepository
  {
    public StudentAccountEntryRepository(CoachsBoxContext dbContext)
      : base(dbContext)
    {
    }
  }
}
