using MediatR;

namespace CoachsBox.Coaching.Application.Commands
{
  public class DeletePaymentCommand : IRequest<bool>
  {
    public DeletePaymentCommand(int paymentAccountingEventId)
    {
      this.PaymentAccountingEventId = paymentAccountingEventId;
    }

    public int PaymentAccountingEventId { get; }
  }
}
