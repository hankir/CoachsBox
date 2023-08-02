using CoachsBox.Core.Primitives;
using CoachsBox.DocumentHandling;

namespace CoachsBox.Coaching.Accounting.StudentContractModel
{
  /// <summary>
  /// Договор со студентом.
  /// </summary>
  public class StudentContract : BaseDocument
  {
    public StudentContract(int studentId, Date date, string number)
    {
      this.StudentId = studentId;
      this.Date = date;
      this.Number = number;
    }

    /// <summary>
    /// Получить идентификатор студента на кого оформлено заявления.
    /// </summary>
    public int StudentId { get; private set; }

    /// <summary>
    /// Получить номер договора.
    /// </summary>
    public string Number { get; private set; }

    /// <summary>
    /// Получить дату заключения договора.
    /// </summary>
    public Date Date { get; private set; }

    public void CorrectDate(Date date)
    {
      this.Date = date;
    }

    public void CorrectNumber(string number)
    {
      this.Number = number;
    }

    private StudentContract()
    {
      // Требует Entity framework core
    }
  }
}
