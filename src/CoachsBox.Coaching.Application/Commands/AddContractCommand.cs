using System;
using MediatR;

namespace CoachsBox.Coaching.Application.Commands
{
  /// <summary>
  /// Команда на добавления заявления студента.
  /// </summary>
  public class AddContractCommand : IRequest<bool>
  {
    /// <summary>
    /// Создать экземпляр команды.
    /// </summary>
    /// <param name="studentId">Идентификатор студента.</param>
    /// <param name="date">Дата заявления.</param>
    public AddContractCommand(int studentId, DateTime date, string number)
    {
      this.StudentId = studentId;
      this.Date = date;
      this.Number = number;
    }

    /// <summary>
    /// Получить идентификатор студента.
    /// </summary>
    public int StudentId { get; private set; }

    /// <summary>
    /// Получить дату заявления.
    /// </summary>
    public DateTime Date { get; private set; }

    /// <summary>
    /// Получить номер договора.
    /// </summary>
    public string Number { get; private set; }
  }
}
