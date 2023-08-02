using System.Collections.Generic;
using System.Threading.Tasks;

namespace CoachsBox.Coaching.StudentModel
{
  public interface IStudentQueryService
  {
    Task<IReadOnlyList<int>> QueryCoachsStudents(int coachId);
  }
}
