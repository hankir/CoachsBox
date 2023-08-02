using CoachsBox.Accounting;

namespace CoachsBox.Coaching.Accounting.StudentAccountEntryModel
{
  public class StudentAccountEntryType : AccountEntryType
  {
    public static StudentAccountEntryType Accrual { get { return new StudentAccountEntryType(nameof(Accrual)); } }

    public static StudentAccountEntryType Payment { get { return new StudentAccountEntryType(nameof(Payment)); } }

    public static StudentAccountEntryType CreateFrom(StudentAccountEntryType entryType)
    {
      return new StudentAccountEntryType(entryType.Name);
    }

    private StudentAccountEntryType(string name)
      : base(name)
    {
    }

    private StudentAccountEntryType()
    {
      // Требует Entity framework core
    }
  }
}
