using CoachsBox.Accounting;

namespace CoachsBox.Coaching.Accounting.GroupAccountEntryModel
{
  public class GroupAccountEntryType : AccountEntryType
  {
    public static GroupAccountEntryType Deposit { get { return new GroupAccountEntryType(nameof(Deposit)); } }

    public static GroupAccountEntryType Withdraw { get { return new GroupAccountEntryType(nameof(Withdraw)); } }

    public GroupAccountEntryType CreateFrom(GroupAccountEntryType groupAccountEntryType)
    {
      return new GroupAccountEntryType(groupAccountEntryType.Name);
    }

    public GroupAccountEntryType(string name)
      : base(name)
    {
    }
  }
}
