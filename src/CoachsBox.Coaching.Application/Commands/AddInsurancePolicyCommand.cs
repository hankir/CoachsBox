using System;
using MediatR;

namespace CoachsBox.Coaching.Application.Commands
{
  /// <summary>
  /// Команда на добавления заявления студента.
  /// </summary>
  public class AddInsurancePolicyCommand : IRequest<bool>
  {
    /// <summary>
    /// Создать экземпляр команды.
    /// </summary>
    /// <param name="studentId">Идентификатор студента.</param>
    /// <param name="date">Дата заявления.</param>
    public AddInsurancePolicyCommand(int studentId, DateTime date, DateTime endDate, string number)
    {
      this.StudentId = studentId;
      this.Date = date;
      this.EndDate = endDate;
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

    public DateTime EndDate { get; internal set; }

    public string Number { get; internal set; }
  }
}
