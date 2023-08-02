using CoachsBox.Coaching.StudentModel;
using CoachsBox.DocumentHandling;

namespace CoachsBox.Coaching.StudentDocumentModel
{
  /// <summary>
  /// Базовый класс всех видов документов на студента.
  /// </summary>
  public abstract class StudentDocument : BaseDocument
  {
    /// <summary>
    /// Создать экземпляр документа на студента.
    /// </summary>
    /// <param name="studentId">Идентификатор студента.</param>
    public StudentDocument(int studentId)
    {
      this.StudentId = studentId;
    }

    /// <summary>
    /// Получить студента.
    /// </summary>
    public Student Student { get; private set; }

    /// <summary>
    /// Получить идентификатор студента.
    /// </summary>
    public int StudentId { get; private set; }

    protected StudentDocument()
    {
      // Требует Entity framework core
    }
  }
}
