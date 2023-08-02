using System;
using CoachsBox.Coaching.Accounting.CoachingAccountingEventModel;
using CoachsBox.Core;
using CoachsBox.Core.Primitives;

namespace CoachsBox.Coaching.Accounting.StudentAccountingEventModel
{
  public class PersonalTrainingAccrualEventSpecification : BaseSpecification<PersonalTrainingAccrualAccountingEvent>
  {
    public PersonalTrainingAccrualEventSpecification(int studentId, Date date)
      : base(accrual =>
      accrual.Account.StudentId == studentId &&
      accrual.TrainingDate.Year == date.Year &&
      accrual.TrainingDate.Month.Number == date.Month.Number &&
      accrual.TrainingDate.Day == date.Day)
    {
      this.ApplyOrderBy(accrual => accrual.WhenOccured);
    }

    public PersonalTrainingAccrualEventSpecification(int groupId, int agreementId, int studentId, DateTime trainingStart, DateTime trainingEnd)
      : base(accrual =>
      accrual.GroupId == groupId &&
      accrual.ServiceAgreement.Id == agreementId &&
      accrual.Account.StudentId == studentId &&
      accrual.TrainingDate.Year == trainingStart.Year &&
      accrual.TrainingDate.Month.Number == trainingStart.Month &&
      accrual.TrainingDate.Day == trainingStart.Day &&
      accrual.StartOfTraining.Hours == trainingStart.Hour &&
      accrual.StartOfTraining.Minutes == trainingStart.Minute &&
      accrual.EndOfTraining.Hours == trainingEnd.Hour &&
      accrual.EndOfTraining.Minutes == trainingEnd.Minute)
    {

    }

    public PersonalTrainingAccrualEventSpecification WithResultingAccountEntries()
    {
      this.AddInclude($"{nameof(PersonalTrainingAccrualAccountingEvent.ProcessingState)}.{nameof(PersonalTrainingAccrualAccountingEvent.ProcessingState.ResultingEntries)}.{nameof(AccountingEventResultingEntry.AccountEntry)}");
      return this;
    }
  }
}
