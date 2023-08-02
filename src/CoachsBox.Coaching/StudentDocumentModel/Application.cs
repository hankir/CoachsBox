using CoachsBox.Core.Primitives;

namespace CoachsBox.Coaching.StudentDocumentModel
{
  /// <summary>
  /// Заявление на участие в тренировках.
  /// </summary>
  public class Application : StudentDocument
  {
    /// <summary>
    /// Создать экземпляр заявления.
    /// </summary>
    /// <param name="studentId">Идентификатор студента.</param>
    /// <param name="date">Дата заявления.</param>
    public Application(int studentId, Date date)
      : base(studentId)
    {
      this.Date = date;
    }

    /// <summary>
    /// Получить дату заявления.
    /// </summary>
    public Date Date { get; private set; }

    /// <summary>
    /// Исправить дату заявления.
    /// </summary>
    /// <param name="newDate">Новая дата заявления.</param>
    public void CorrectDate(Date newDate)
    {
      this.Date = newDate;
    }

    private Application()
    {
      // Требует Entity framework core
    }
  }
}
