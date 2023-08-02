using System;
using MediatR;

namespace CoachsBox.Coaching.Application.Accounting.Commands
{
  /// <summary>
  /// Команда на обработку платежей за указанный период.
  /// </summary>
  public class ProcessPaymentsForPeriodCommand : IRequest<bool>
  {
    /// <summary>
    /// Создать экземпляр команды на обработку платежей.
    /// </summary>
    /// <param name="periodBeginning">Начало периода.</param>
    /// <param name="periodEnding">Конец периода.</param>
    public ProcessPaymentsForPeriodCommand(DateTime periodBeginning, DateTime periodEnding)
    {
      this.PeriodBeginning = periodBeginning;
      this.PeriodEnding = periodEnding;
    }

    public DateTime PeriodBeginning { get; }

    public DateTime PeriodEnding { get; }
  }
}
