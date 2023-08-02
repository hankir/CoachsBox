using CoachsBox.Core;

namespace CoachsBox.Coaching.Accounting.StudentAccountingEventModel
{
  public class PreviouslyUnprocessedAccrualsSpecification : BaseSpecification<StudentAccountingEvent>
  {
    public PreviouslyUnprocessedAccrualsSpecification(int count) : base(
      accrual => !accrual.ProcessingState.IsProcessed &&
      (accrual.EventType.Name == StudentAccountingEventType.Accrual.Name ||
      accrual.EventType.Name == StudentAccountingEventType.PersonalTrainingAccrual.Name))
    {
      this.ApplyPaging(0, count);
      this.ApplyOrderBy(accrual => accrual.WhenOccured);
      this.AddInclude(accrual => accrual.Account);
      this.AddInclude(accrual => accrual.ServiceAgreement);
      this.AddInclude("ServiceAgreement.PostingRules.PostingRule");
    }
  }
}
