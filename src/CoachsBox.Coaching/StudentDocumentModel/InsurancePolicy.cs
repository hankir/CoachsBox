using System;
using System.Collections.Generic;
using System.Text;
using CoachsBox.Core.Primitives;

namespace CoachsBox.Coaching.StudentDocumentModel
{
  /// <summary>
  /// Страховой полис студента.
  /// </summary>
  public class InsurancePolicy : StudentDocument
  {
    public InsurancePolicy(int studentId, Date date, Date endDate, string number)
      : base(studentId)
    {
      this.Date = date;
      this.EndDate = endDate;
      this.Number = number;
    }

    /// <summary>
    /// Получить номер страхового полиса.
    /// </summary>
    public string Number { get; private set; }

    /// <summary>
    /// Получить дату начала действия полиса.
    /// </summary>
    public Date Date { get; private set; }

    /// <summary>
    /// Получить дату завершения действия полиса.
    /// </summary>
    public Date EndDate { get; private set; }

    public void CorrectDates(Date date, Date endDate)
    {
      this.Date = date;
      this.EndDate = endDate;
    }

    public void CorrectNumber(string number)
    {
      this.Number = number;
    }

    private InsurancePolicy()
    {
      // Требует Entity framework core
    }
  }
}
