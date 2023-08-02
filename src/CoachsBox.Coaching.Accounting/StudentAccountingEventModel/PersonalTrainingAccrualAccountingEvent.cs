using System;
using CoachsBox.Coaching.Accounting.CoachingServiceAgreementModel;
using CoachsBox.Coaching.Accounting.StudentAccountModel;
using CoachsBox.Core;
using CoachsBox.Core.Primitives;

namespace CoachsBox.Coaching.Accounting.StudentAccountingEventModel
{
  public class PersonalTrainingAccrualAccountingEvent : StudentAccountingEvent
  {
    public Date TrainingDate { get; private set; }

    public TimeOfDay StartOfTraining { get; private set; }

    public TimeOfDay EndOfTraining { get; private set; }

    public override string BuildAccountEntryDescription()
    {
      return $"Начисление за тренировку. Дата тренировки: {this.TrainingDate.ToDateTime().Value.ToShortDateString()}, с {this.StartOfTraining.ToTimeSpan()} по {this.EndOfTraining.ToTimeSpan()}";
    }

    public PersonalTrainingAccrualAccountingEvent(
      Date trainingDate,
      TimeOfDay startOfTraining,
      TimeOfDay endOfTraining,
      DateTime whenOccured,
      DateTime whenNoticed,
      int groupId,
      StudentAccount account,
      CoachingServiceAgreement serviceAgreement)
      : base(StudentAccountingEventType.PersonalTrainingAccrual, whenOccured, whenNoticed, groupId, account, serviceAgreement)
    {
      this.TrainingDate = trainingDate;
      this.StartOfTraining = startOfTraining;
      this.EndOfTraining = endOfTraining;
    }

    public PersonalTrainingAccrualAccountingEvent(
      Date trainingDate,
      TimeOfDay startOfTraining,
      TimeOfDay endOfTraining,
      DateTime whenOccured,
      DateTime whenNoticed,
      int groupId,
      int accountId,
      int serviceAgreementId)
      : base(StudentAccountingEventType.PersonalTrainingAccrual, whenOccured, whenNoticed, groupId, accountId, serviceAgreementId)
    {
      this.TrainingDate = trainingDate;
      this.StartOfTraining = startOfTraining;
      this.EndOfTraining = endOfTraining;
    }

    private PersonalTrainingAccrualAccountingEvent()
    {
      // Требует Entity framework core
    }
  }
}
