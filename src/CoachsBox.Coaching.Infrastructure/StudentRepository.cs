using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using CoachsBox.Coaching.StudentModel;
using CoachsBox.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace CoachsBox.Coaching.Infrastructure
{
  public class StudentRepository : EfRepository<Student, CoachsBoxContext>, IStudentRepository
  {
    public StudentRepository(CoachsBoxContext dbContext)
      : base(dbContext)
    {
    }

    public override async Task<Student> GetByIdAsync(int id)
    {
      var student = await this.context.Students.FindAsync(id);

      // Загрузим персону.
      if (student != null)
        await this.context.Entry(student).Reference(s => s.Person).LoadAsync();

      return student;
    }

    public override async Task<IReadOnlyList<Student>> ListAllAsync()
    {
      return await this.context.Students.Include(c => c.Person).ToListAsync();
    }
  }
}
