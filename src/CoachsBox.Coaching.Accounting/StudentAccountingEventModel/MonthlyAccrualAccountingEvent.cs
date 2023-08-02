using System;
using CoachsBox.Coaching.Accounting.CoachingServiceAgreementModel;
using CoachsBox.Coaching.Accounting.StudentAccountModel;
using CoachsBox.Core.Primitives;

namespace CoachsBox.Coaching.Accounting.StudentAccountingEventModel
{
  public class MonthlyAccrualAccountingEvent : StudentAccountingEvent
  {
    /// <summary>
    /// Получить количество тренировок в месяц.
    /// </summary>
    public byte TrainingsQuantity { get; private set; }

    /// <summary>
    /// Получить месяц, за который идет начисление.
    /// </summary>
    public Month Month { get; private set; }

    /// <summary>
    /// Получить год, за который идет начисление.
    /// </summary>
    public int Year { get; private set; }

    public override string BuildAccountEntryDescription()
    {
      return $"Ежемесячное начисление. Количество тренировок: {this.TrainingsQuantity}, месяц: {this.Month.Number}, год: {this.Year}";
    }

    public MonthlyAccrualAccountingEvent(
      byte trainingsQuantity,
      Month month,
      int year,
      DateTime whenOccured,
      DateTime whenNoticed,
      int groupId,
      StudentAccount account,
      CoachingServiceAgreement serviceAgreement)
      : base(StudentAccountingEventType.Accrual, whenOccured, whenNoticed, groupId, account, serviceAgreement)
    {
      this.TrainingsQuantity = trainingsQuantity;
      this.Month = month;
      this.Year = year;
    }

    public MonthlyAccrualAccountingEvent(
      byte trainingsQuantity,
      Month month,
      int year,
      DateTime whenOccured,
      DateTime whenNoticed,
      int groupId,
      int accountId,
      int serviceAgreementId)
      : base(StudentAccountingEventType.Accrual, whenOccured, whenNoticed, groupId, accountId, serviceAgreementId)
    {
      this.TrainingsQuantity = trainingsQuantity;
      this.Month = month;
      this.Year = year;
    }

    private MonthlyAccrualAccountingEvent()
    {
      // Требует Entity framework core
    }
  }
}
