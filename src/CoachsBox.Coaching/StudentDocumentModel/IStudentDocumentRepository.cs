using System;
using System.Collections.Generic;
using System.Text;
using CoachsBox.Core.Interfaces;

namespace CoachsBox.Coaching.StudentDocumentModel
{
  public interface IStudentDocumentRepository : IAsyncRepository<StudentDocument>
  {
  }
}
