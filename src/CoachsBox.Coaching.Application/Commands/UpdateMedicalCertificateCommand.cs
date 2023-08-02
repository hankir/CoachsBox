using System;
using MediatR;

namespace CoachsBox.Coaching.Application.Commands
{
  /// <summary>
  /// Команда на добавления заявления студента.
  /// </summary>
  public class UpdateMedicalCertificateCommand : IRequest<bool>
  {
    /// <summary>
    /// Создать экземпляр команды.
    /// </summary>
    /// <param name="medicalCertificateId">Идентификатор сертификата.</param>
    /// <param name="date">Дата заявления.</param>
    public UpdateMedicalCertificateCommand(int medicalCertificateId, DateTime date, bool allowTraining, bool allowCompetition)
    {
      this.MedicalCertificateId = medicalCertificateId;
      this.Date = date;
      this.AllowTraining = allowTraining;
      this.AllowCompetition = allowCompetition;
    }

    /// <summary>
    /// Получить идентификатор студента.
    /// </summary>
    public int MedicalCertificateId { get; private set; }

    /// <summary>
    /// Получить дату заявления.
    /// </summary>
    public DateTime Date { get; private set; }

    /// <summary>
    /// Получить признак того, что разрешены тренировки.
    /// </summary>
    public bool AllowTraining { get; private set; }

    /// <summary>
    /// Получить признак того, что разрешены соревнования.
    /// </summary>
    public bool AllowCompetition { get; private set; }
  }
}
