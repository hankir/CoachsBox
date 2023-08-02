using System;
using CoachsBox.Accounting;
using CoachsBox.Coaching.Accounting.CoachingAccountingEventModel;
using CoachsBox.Coaching.Accounting.SalaryModel;

namespace CoachsBox.Coaching.Accounting.SalaryAccountingEventModel
{
  public abstract class SalaryAccountingEvent : CoachingAccountingEvent
  {
    public SalaryAccountingEvent(SalaryAccountingEventType eventType, Salary salary, DateTime whenOccured, DateTime whenNoticed)
      : base(eventType, whenOccured, whenNoticed)
    {
      this.Salary = salary;
      this.SalaryId = salary.Id;
    }

    /// <summary>
    /// Получить идентификатор зарплаты.
    /// </summary>
    public int SalaryId { get; private set; }

    /// <summary>
    /// Получить зарплату.
    /// </summary>
    public Salary Salary { get; private set; }

    protected override PostingRule FindRule()
    {
      return this.Salary.GetPostingRule(this.EventType);
    }

    protected SalaryAccountingEvent()
    {
      // Требует Entity framework core
    }
  }
}
