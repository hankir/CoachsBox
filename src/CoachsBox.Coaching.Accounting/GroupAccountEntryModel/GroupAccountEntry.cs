using System;
using CoachsBox.Accounting;
using CoachsBox.Coaching.Accounting.GroupAccountModel;
using CoachsBox.Core.Primitives;

namespace CoachsBox.Coaching.Accounting.GroupAccountEntryModel
{
  public class GroupAccountEntry : AccountEntry
  {
    public GroupAccountEntry(GroupAccountEntryType entryType, Money amount, DateTime date)
      : base(entryType, amount, date)
    {
    }

    public GroupAccount GroupAccount { get; private set; }

    private GroupAccountEntry()
    {
      // Требует Entity framework core
    }
  }
}
