using CoachsBox.Accounting;

namespace CoachsBox.Coaching.Accounting.StudentAccountingEventModel
{
  public class StudentAccountingEventType : AccountingEventType
  {
    public static StudentAccountingEventType Accrual { get { return new StudentAccountingEventType(nameof(Accrual)); } }

    public static StudentAccountingEventType PersonalTrainingAccrual { get { return new StudentAccountingEventType(nameof(PersonalTrainingAccrual)); } }

    public static StudentAccountingEventType Payment { get { return new StudentAccountingEventType(nameof(Payment)); } }

    private StudentAccountingEventType(string name)
      : base(name)
    {
    }

    private StudentAccountingEventType()
    {
      // Требует Entity framework core
    }
  }
}
