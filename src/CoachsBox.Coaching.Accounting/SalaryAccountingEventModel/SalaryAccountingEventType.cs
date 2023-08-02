using CoachsBox.Accounting;

namespace CoachsBox.Coaching.Accounting.SalaryAccountingEventModel
{
  public class SalaryAccountingEventType : AccountingEventType
  {
    public static SalaryAccountingEventType CoachSalaryPayment => new SalaryAccountingEventType(nameof(CoachSalaryPayment));

    public static SalaryAccountingEventType CoachSalaryDebt => new SalaryAccountingEventType(nameof(CoachSalaryDebt));

    private SalaryAccountingEventType(string name)
      : base(name)
    {
    }

    private SalaryAccountingEventType()
    {
      // Требует Entity framework core
    }
  }
}
