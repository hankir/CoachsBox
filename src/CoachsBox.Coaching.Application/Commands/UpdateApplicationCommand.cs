using System;
using MediatR;

namespace CoachsBox.Coaching.Application.Commands
{
  /// <summary>
  /// Команда на добавления заявления студента.
  /// </summary>
  public class UpdateApplicationCommand : IRequest<bool>
  {
    /// <summary>
    /// Создать экземпляр команды.
    /// </summary>
    /// <param name="applicationId">Идентификатор заявления.</param>
    /// <param name="date">Дата заявления.</param>
    public UpdateApplicationCommand(int applicationId, DateTime date)
    {
      this.ApplicationId = applicationId;
      this.Date = date;
    }

    /// <summary>
    /// Получить идентификатор заявления.
    /// </summary>
    public int ApplicationId { get; }

    /// <summary>
    /// Получить дату заявления.
    /// </summary>
    public DateTime Date { get; private set; }
  }
}
