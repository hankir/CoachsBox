using CoachsBox.Core;

namespace CoachsBox.Coaching.StudentDocumentModel
{
  public class StudentApplicationSpecification : BaseSpecification<Application>
  {
    public StudentApplicationSpecification(int studentId)
      : base(application => application.StudentId == studentId)
    {
    }

    /// <summary>
    /// Получить документы студента с получением ссылки на студента.
    /// </summary>
    /// <returns>Этот экземпляр спецификации на документы студента.</returns>
    public StudentApplicationSpecification WithStudent()
    {
      this.Includes.Add(application => application.Student);
      return this;
    }
  }
}
