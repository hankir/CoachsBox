using System;
using MediatR;

namespace CoachsBox.Coaching.Application.Commands
{
  /// <summary>
  /// Команда на добавления заявления студента.
  /// </summary>
  public class AddApplicationCommand : IRequest<bool>
  {
    /// <summary>
    /// Создать экземпляр команды.
    /// </summary>
    /// <param name="studentId">Идентификатор студента.</param>
    /// <param name="date">Дата заявления.</param>
    public AddApplicationCommand(int studentId, DateTime date)
    {
      this.StudentId = studentId;
      this.Date = date;
    }

    /// <summary>
    /// Получить идентификатор студента.
    /// </summary>
    public int StudentId { get; private set; }

    /// <summary>
    /// Получить дату заявления.
    /// </summary>
    public DateTime Date { get; private set; }
  }
}
