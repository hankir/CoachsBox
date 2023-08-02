using System;
using System.Collections.Generic;
using System.Text;
using CoachsBox.Core.Primitives;

namespace CoachsBox.Coaching.StudentDocumentModel
{
  /// <summary>
  /// Медицинская справка.
  /// </summary>
  public class MedicalCertificate : StudentDocument
  {
    /// <summary>
    /// Создать медицинскую справку с допуском до тернировок.
    /// </summary>
    /// <param name="studentId">Идентификатор студента.</param>
    /// <param name="date">Дата выдачи справки.</param>
    /// <param name="endDate">Дата завершения действия справки.</param>
    /// <returns>Медицинская справка.</returns>
    public static MedicalCertificate CreateWithAllowTraining(int studentId, Date date, Date endDate)
    {
      return new MedicalCertificate(studentId)
      {
        Date = date,
        EndDate = endDate,
        AllowTraining = true,
        AllowCompetition = false
      };
    }

    /// <summary>
    /// Создать медицинскую справку с допуском до тернировок и соревнований.
    /// </summary>
    /// <param name="studentId">Идентификатор студента.</param>
    /// <param name="date">Дата выдачи справки.</param>
    /// <param name="endDate">Дата завершения действия справки.</param>
    /// <returns>Медицинская справка.</returns>
    public static MedicalCertificate CreateWithAllowCompetition(int studentId, Date date, Date endDate)
    {
      return new MedicalCertificate(studentId)
      {
        Date = date,
        EndDate = endDate,
        AllowTraining = true,
        AllowCompetition = true
      };
    }

    private MedicalCertificate(int studentId)
      : base(studentId)
    {
    }

    /// <summary>
    /// Получить дату выдачи справки.
    /// </summary>
    public Date Date { get; private set; }

    /// <summary>
    /// Получить дату завершения действия справки.
    /// </summary>
    public Date EndDate { get; private set; }

    /// <summary>
    /// Получить признак того, что разрешены тренировки.
    /// </summary>
    public bool AllowTraining { get; private set; }

    /// <summary>
    /// Получить признак того, что разрешены соревнования.
    /// </summary>
    public bool AllowCompetition { get; private set; }

    public void CorrectDates(Date date, Date endDate)
    {
      this.Date = date;
      this.EndDate = endDate;
    }

    public void CorrectAllows(bool allowTraining, bool allowCompetition)
    {
      this.AllowTraining = allowTraining;
      this.AllowCompetition = allowCompetition;
    }

    private MedicalCertificate()
    {
      // Требует Entity framework core
    }
  }
}
