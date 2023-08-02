using System;
using CoachsBox.Accounting;
using CoachsBox.Coaching.Accounting.StudentAccountModel;
using CoachsBox.Core.Primitives;

namespace CoachsBox.Coaching.Accounting.StudentAccountEntryModel
{
  public class StudentAccountEntry : AccountEntry
  {
    public StudentAccountEntry(AccountEntryType entryType, Money amount, DateTime date, string description)
      : base(entryType, amount, date)
    {
      this.Description = description;
    }

    public StudentAccount StudentAccount { get; private set; }

    public string Description { get; private set; }

    private StudentAccountEntry()
    {

    }
  }
}
