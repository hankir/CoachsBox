using System;
using MediatR;

namespace CoachsBox.Coaching.Application.Commands
{
  /// <summary>
  /// Команда на добавления заявления студента.
  /// </summary>
  public class UpdateContractCommand : IRequest<bool>
  {
    /// <summary>
    /// Создать экземпляр команды.
    /// </summary>
    /// <param name="contractId">Идентификатор студента.</param>
    /// <param name="date">Дата заявления.</param>
    /// <param name="number">Номер договора.</param>
    public UpdateContractCommand(int contractId, DateTime date, string number)
    {
      this.ContractId = contractId;
      this.Date = date;
      this.Number = number;
    }

    /// <summary>
    /// Получить идентификатор студента.
    /// </summary>
    public int ContractId { get; private set; }

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
