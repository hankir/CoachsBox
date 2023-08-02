using System;
using System.Threading;
using System.Threading.Tasks;
using CoachsBox.Coaching.Accounting.AgreementRegistryEntryModel;
using CoachsBox.Coaching.Accounting.StudentAccountingEventModel;
using CoachsBox.Coaching.Accounting.StudentAccountModel;
using CoachsBox.Core;
using CoachsBox.Core.Interfaces;
using CoachsBox.Core.Primitives;
using MediatR;

namespace CoachsBox.Coaching.Application.Commands
{
  public class RegisterPaymentCommandHandler : IRequestHandler<RegisterPaymentCommand, int>
  {
    private readonly IUnitOfWork unitOfWork;
    private readonly IStudentAccountingEventRepository eventRepository;
    private readonly IStudentAccountRepository studentAccountRepository;
    private readonly IAgreementRegistryEntryRepository agreementRegistryEntryRepository;

    public RegisterPaymentCommandHandler(
      IUnitOfWork unitOfWork,
      IStudentAccountingEventRepository eventRepository,
      IStudentAccountRepository studentAccountRepository,
      IAgreementRegistryEntryRepository agreementRegistryEntryRepository)
    {
      this.unitOfWork = unitOfWork;
      this.eventRepository = eventRepository;
      this.studentAccountRepository = studentAccountRepository;
      this.agreementRegistryEntryRepository = agreementRegistryEntryRepository;
    }

    public async Task<int> Handle(RegisterPaymentCommand request, CancellationToken cancellationToken)
    {
      if (request == null)
        return default;

      var amount = Money.CreateRuble(request.Amount);
      var whenOccured = request.WhenOccured;
      var studentId = request.StudentId;
      var groupId = request.GroupId;

      var studentAccountSpecification = new StudentAccountSpecification(studentId);
      var account = await this.studentAccountRepository.GetBySpecAsync(studentAccountSpecification);
      if (account == null)
      {
        account = new StudentAccount(studentId);
        await this.studentAccountRepository.AddAsync(account);
      }

      var agreementRegistryEntryByGroupSpec = new AgreementRegistryEntryByGroupSpecification(groupId)
        .WithPostingRules()
        .WithGroupAccount();
      var agreementRegistryEntry = await this.agreementRegistryEntryRepository.GetBySpecAsync(agreementRegistryEntryByGroupSpec);

      var serviceAgreement = agreementRegistryEntry?.Agreement;
      if (serviceAgreement == null)
        throw new InvalidOperationException($"Coaching service agreement not found for group. Group Id: {groupId}");

      var paymentEvent = new PaymentAccountingEvent(amount, whenOccured, Watch.Now.DateTime, account, agreementRegistryEntry);
      paymentEvent.Process();

      await this.eventRepository.AddAsync(paymentEvent);
      unitOfWork.Save();
      return paymentEvent.Id;
    }
  }
}
