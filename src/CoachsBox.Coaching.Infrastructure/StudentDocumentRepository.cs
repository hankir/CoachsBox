using CoachsBox.Coaching.StudentDocumentModel;
using CoachsBox.Infrastructure;

namespace CoachsBox.Coaching.Infrastructure
{
  public class StudentDocumentRepository : EfRepository<StudentDocument, CoachsBoxContext>, IStudentDocumentRepository
  {
    public StudentDocumentRepository(CoachsBoxContext dbContext)
      : base(dbContext)
    {
    }
  }
}
