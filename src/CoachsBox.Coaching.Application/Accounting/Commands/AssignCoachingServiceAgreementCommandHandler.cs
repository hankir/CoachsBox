using System;
using System.Threading;
using System.Threading.Tasks;
using CoachsBox.Coaching.Accounting.AgreementRegistryEntryModel;
using CoachsBox.Coaching.Accounting.CoachingServiceAgreementModel;
using CoachsBox.Coaching.Accounting.GroupAccountModel;
using MediatR;

namespace CoachsBox.Coaching.Application.Accounting.Commands
{
  public class AssignCoachingServiceAgreementCommandHandler : IRequestHandler<AssignCoachingServiceAgreementCommand, bool>
  {
    private readonly ICoachingServiceAgreementRepository serviceAgreementRepository;
    private readonly IAgreementRegistryEntryRepository agreementRegistryRepository;
    private readonly IGroupAccountRepository groupAccountRepository;

    public AssignCoachingServiceAgreementCommandHandler(
      ICoachingServiceAgreementRepository serviceAgreementRepository,
      IAgreementRegistryEntryRepository agreementRegistryRepository,
      IGroupAccountRepository groupAccountRepository)
    {
      this.serviceAgreementRepository = serviceAgreementRepository;
      this.agreementRegistryRepository = agreementRegistryRepository;
      this.groupAccountRepository = groupAccountRepository;
    }

    public async Task<bool> Handle(AssignCoachingServiceAgreementCommand request, CancellationToken cancellationToken)
    {
      if (request == null)
        throw new ArgumentNullException(nameof(request));

      var agreementId = request.AgreementId;
      var groupId = request.GroupId;

      var agreement = await this.serviceAgreementRepository.GetByIdAsync(agreementId);
      if (agreement != null)
      {
        var groupAccount = await this.GetGroupAccount(groupId);
        var groupAgreement = new AgreementRegistryEntry(agreementId, groupId, groupAccount.Id);
        await this.agreementRegistryRepository.AddAsync(groupAgreement);
        await this.agreementRegistryRepository.SaveAsync();
      }
      else
        throw new InvalidOperationException("Coaching service agreement not found");

      return true;
    }

    private async Task<GroupAccount> GetGroupAccount(int groupId)
    {
      var groupAccountSpecification = new GroupAccountSpecification(groupId).AsReadOnly();
      var groupAccount = await this.groupAccountRepository.GetBySpecAsync(groupAccountSpecification);
      if (groupAccount == null)
      {
        groupAccount = new GroupAccount(groupId);
        await this.groupAccountRepository.AddAsync(groupAccount);
        await this.groupAccountRepository.SaveAsync();
      }

      return groupAccount;
    }
  }
}
