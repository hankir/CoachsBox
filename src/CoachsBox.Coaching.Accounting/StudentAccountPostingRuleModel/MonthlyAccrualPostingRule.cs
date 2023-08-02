using CoachsBox.Accounting;
using CoachsBox.Coaching.Accounting.StudentAccountEntryModel;
using CoachsBox.Coaching.Accounting.StudentAccountingEventModel;
using CoachsBox.Core.Primitives;

namespace CoachsBox.Coaching.Accounting.StudentAccountPostingRuleModel
{
  public class MonthlyAccrualPostingRule : StudentAccountPostingRule
  {
    public MonthlyAccrualPostingRule(StudentAccountEntryType entryType)
      : base(entryType)
    {
    }

    protected override Money CalculateAmout(AccountingEvent accountingEvent)
    {
      var monthlyAccrualEvent = (MonthlyAccrualAccountingEvent)accountingEvent;
      var agreement = monthlyAccrualEvent.ServiceAgreement;
      return Money.CreateRuble(monthlyAccrualEvent.TrainingsQuantity * agreement.Rate.Quantity).Negate();
    }

    private MonthlyAccrualPostingRule()
    {
      // Требует Entity framework core
    }
  }
}
