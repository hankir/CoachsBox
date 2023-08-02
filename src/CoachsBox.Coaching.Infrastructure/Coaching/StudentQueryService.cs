using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoachsBox.Coaching.StudentModel;
using Microsoft.EntityFrameworkCore;

namespace CoachsBox.Coaching.Infrastructure.Coaching
{
  public class StudentQueryService : IStudentQueryService
  {
    private readonly ReadOnlyCoachsBoxContext readOnlyCoachsBox;

    public StudentQueryService(ReadOnlyCoachsBoxContext readOnlyCoachsBox)
    {
      this.readOnlyCoachsBox = readOnlyCoachsBox;
    }

    public async Task<IReadOnlyList<int>> QueryCoachsStudents(int coachId)
    {
      return await this.readOnlyCoachsBox
        .Schedules
        .Include(schedule => schedule.Group)
        .ThenInclude(group => group.EnrolledStudents)
        .Where(schedule => schedule.CoachId == coachId)
        .SelectMany(schedule => schedule.Group.EnrolledStudents)
        .Select(enrolled => enrolled.StudentId)
        .ToListAsync();
    }
  }
}
