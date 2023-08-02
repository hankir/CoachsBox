using System.Threading;
using System.Threading.Tasks;
using CoachsBox.Coaching.Accounting.GroupAccountEntryModel;
using CoachsBox.Coaching.Accounting.StudentAccountEntryModel;
using CoachsBox.Coaching.Accounting.StudentAccountingEventModel;
using CoachsBox.Core.Interfaces;
using MediatR;

namespace CoachsBox.Coaching.Application.Commands
{
  public class DeletePaymentCommandHandler : IRequestHandler<DeletePaymentCommand, bool>
  {
    private readonly IUnitOfWork unitOfWork;
    private readonly IStudentAccountingEventRepository eventRepository;
    private readonly IStudentAccountEntryRepository studentAccountEntryRepository;
    private readonly IGroupAccountEntryRepository groupAccountEntryRepository;

    public DeletePaymentCommandHandler(
      IUnitOfWork unitOfWork,
      IStudentAccountingEventRepository eventRepository,
      IStudentAccountEntryRepository studentAccountEntryRepository,
      IGroupAccountEntryRepository groupAccountEntryRepository)
    {
      this.unitOfWork = unitOfWork;
      this.eventRepository = eventRepository;
      this.studentAccountEntryRepository = studentAccountEntryRepository;
      this.groupAccountEntryRepository = groupAccountEntryRepository;
    }

    public async Task<bool> Handle(DeletePaymentCommand request, CancellationToken cancellationToken)
    {
      if (request == null)
        return false;

      var paymentSpecification = new PaymentSpecification(request.PaymentAccountingEventId).WithResultingAccountEntries();
      var payment = await this.eventRepository.GetBySpecAsync(paymentSpecification);
      if (payment != null)
      {
        foreach (var accountEntry in payment.ProcessingState.ResultingEntries)
        {
          if (accountEntry.AccountEntry is StudentAccountEntry studentAccountEntry)
            await this.studentAccountEntryRepository.DeleteAsync(studentAccountEntry);

          if (accountEntry.AccountEntry is GroupAccountEntry groupAccountEntry)
            await this.groupAccountEntryRepository.DeleteAsync(groupAccountEntry);
        }
        await this.eventRepository.DeleteAsync(payment);
        await this.unitOfWork.SaveAsync();
        return true;
      }

      return false;
    }
  }
}
