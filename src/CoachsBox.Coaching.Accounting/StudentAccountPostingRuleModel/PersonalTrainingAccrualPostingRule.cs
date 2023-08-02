using CoachsBox.Accounting;
using CoachsBox.Coaching.Accounting.StudentAccountEntryModel;
using CoachsBox.Coaching.Accounting.StudentAccountingEventModel;
using CoachsBox.Core.Primitives;

namespace CoachsBox.Coaching.Accounting.StudentAccountPostingRuleModel
{
  public class PersonalTrainingAccrualPostingRule : StudentAccountPostingRule
  {
    public PersonalTrainingAccrualPostingRule(StudentAccountEntryType entryType)
      : base(entryType)
    {
    }

    protected override Money CalculateAmout(AccountingEvent accountingEvent)
    {
      var personalAccrualEvent = (PersonalTrainingAccrualAccountingEvent)accountingEvent;
      var agreement = personalAccrualEvent.ServiceAgreement;
      return agreement.Rate.Negate();
    }

    private PersonalTrainingAccrualPostingRule()
    {
      // Требует Entity framework core
    }
  }
}
