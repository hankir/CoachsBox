using System;
using MediatR;

namespace CoachsBox.Coaching.Application.Commands
{
  public class UpdateInsurancePolicyCommand : IRequest<bool>
  {
    /// <summary>
    /// Создать экземпляр команды.
    /// </summary>
    /// <param name="insurancePolicyId">Идентификатор страхового полиса.</param>
    /// <param name="date">Дата заявления.</param>
    public UpdateInsurancePolicyCommand(int insurancePolicyId, DateTime date, DateTime endDate, string number)
    {
      this.InsurancePolicyId = insurancePolicyId;
      this.Date = date;
      this.EndDate = endDate;
      this.Number = number;
    }

    /// <summary>
    /// Получить идентификатор страхового полиса.
    /// </summary>
    public int InsurancePolicyId { get; private set; }

    public DateTime Date { get; private set; }

    public DateTime EndDate { get; internal set; }

    public string Number { get; internal set; }
  }
}
