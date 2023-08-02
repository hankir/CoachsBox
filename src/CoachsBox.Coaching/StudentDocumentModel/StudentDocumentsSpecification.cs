using CoachsBox.Core;

namespace CoachsBox.Coaching.StudentDocumentModel
{
  /// <summary>
  /// Спецификация на все документы студента.
  /// </summary>
  public class StudentDocumentsSpecification : BaseSpecification<StudentDocument>
  {
    /// <summary>
    /// Создать спецификацию на документы студента.
    /// </summary>
    /// <param name="studentId">Идентификатор студента.</param>
    public StudentDocumentsSpecification(int studentId)
      : base(studentDocument => studentDocument.StudentId == studentId)
    {
    }

    /// <summary>
    /// Получить документы студента с получением ссылки на студента.
    /// </summary>
    /// <returns>Этот экземпляр спецификации на документы студента.</returns>
    public StudentDocumentsSpecification WithStudent()
    {
      this.Includes.Add(studentDocument => studentDocument.Student);
      return this;
    }
  }
}
