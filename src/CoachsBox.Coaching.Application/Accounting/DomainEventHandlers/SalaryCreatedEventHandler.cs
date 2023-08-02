using System;
using System.Threading;
using System.Threading.Tasks;
using CoachsBox.Coaching.Accounting.SalaryModel.Events;
using CoachsBox.Coaching.Application.Accounting.Commands;
using CoachsBox.Core;
using MediatR;

namespace CoachsBox.Coaching.Application.Accounting.DomainEventHandlers
{
  public class SalaryCreatedEventHandler : INotificationHandler<SalaryCreatedEvent>
  {
    private readonly IMediator mediator;

    public SalaryCreatedEventHandler(IMediator mediator)
    {
      this.mediator = mediator;
    }

    public async Task Handle(SalaryCreatedEvent notification, CancellationToken cancellationToken)
    {
      if (notification == null)
        throw new ArgumentNullException(nameof(notification));

      var salary = notification.Salary;
      var periodBeginning = DateTime.MinValue;
      var periodEnding = salary.PaymentsPeriodEnding.ToDateTime().Value;

      // Для расчета фонда зарплаты платежи должны быть обработаны на указнный период в зарплате.
      var processPaymentsForPeriodCommand = new ProcessPaymentsForPeriodCommand(periodBeginning, periodEnding);
      await this.mediator.Send(processPaymentsForPeriodCommand);
    }
  }
}
