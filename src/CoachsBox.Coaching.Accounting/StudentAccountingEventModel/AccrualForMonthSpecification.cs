using CoachsBox.Coaching.Accounting.CoachingAccountingEventModel;
using CoachsBox.Core;

namespace CoachsBox.Coaching.Accounting.StudentAccountingEventModel
{
  public class AccrualForMonthSpecification : BaseSpecification<MonthlyAccrualAccountingEvent>
  {
    public AccrualForMonthSpecification(int studentId, int month, int year)
      : base(accrual =>
      accrual.Account.StudentId == studentId &&
      accrual.Month.Number == month &&
      accrual.Year == year)
    {
    }

    public AccrualForMonthSpecification WithResultingAccountEntries()
    {
      this.AddInclude($"{nameof(MonthlyAccrualAccountingEvent.ProcessingState)}.{nameof(MonthlyAccrualAccountingEvent.ProcessingState.ResultingEntries)}.{nameof(AccountingEventResultingEntry.AccountEntry)}");
      return this;
    }
  }
}
