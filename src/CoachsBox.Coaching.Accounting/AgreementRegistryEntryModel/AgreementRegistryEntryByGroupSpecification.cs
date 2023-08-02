using System.Collections.Generic;
using System.Linq;
using CoachsBox.Coaching.Accounting.CoachingServiceAgreementModel;
using CoachsBox.Core;

namespace CoachsBox.Coaching.Accounting.AgreementRegistryEntryModel
{
  public class AgreementRegistryEntryByGroupSpecification : BaseSpecification<AgreementRegistryEntry>
  {
    public AgreementRegistryEntryByGroupSpecification(int groupId)
      : base(agreement => agreement.GroupId == groupId)
    {
      this.AddInclude(agreement => agreement.Agreement);
    }

    public AgreementRegistryEntryByGroupSpecification(IEnumerable<int> groupIds)
      : base(agreement => groupIds.Contains(agreement.GroupId))
    {
      this.AddInclude(agreement => agreement.Agreement);
    }

    public AgreementRegistryEntryByGroupSpecification WithPostingRules()
    {
      this.AddInclude($"{nameof(AgreementRegistryEntry.Agreement)}.{nameof(AgreementRegistryEntry.Agreement.PostingRules)}.{nameof(AgreedPostingRule.PostingRule)}");
      return this;
    }

    public AgreementRegistryEntryByGroupSpecification WithGroupAccount()
    {
      this.AddInclude(agreement => agreement.GroupAccount);
      return this;
    }
  }
}
