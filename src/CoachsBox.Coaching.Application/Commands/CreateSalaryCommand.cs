using System;
using MediatR;

namespace CoachsBox.Coaching.Application.Commands
{
  /// <summary>
  /// Команда создания расчета зарплаты.
  /// </summary>
  public class CreateSalaryCommand : IRequest<CommandResult>
  {
    public CreateSalaryCommand(int year, int month, DateTime paymentsPeriodEnds)
    {
      this.Year = year;
      this.Month = month;
      this.PaymentsPeriodEnds = paymentsPeriodEnds;
    }

    /// <summary>
    /// Получить год, за который расчитывается зарплата.
    /// </summary>
    public int Year { get; private set; }

    /// <summary>
    /// Получить месяц, за который расчитывается зарплата.
    /// </summary>
    public int Month { get; private set; }

    /// <summary>
    /// Получить или установить дату по которую учитывать платежи.
    /// </summary>
    public DateTime PaymentsPeriodEnds { get; private set; }
  }
}
