using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using CoachsBox.Core.Interfaces;

namespace CoachsBox.Coaching.StudentModel
{
  public interface IStudentRepository : IAsyncRepository<Student>
  {
  }
}
