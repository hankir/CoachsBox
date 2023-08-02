using System.Collections.Generic;
using CoachsBox.Core;

namespace CoachsBox.Accounting
{
  public abstract class AccountEntryType : ValueObject
  {
    public AccountEntryType(string name)
    {
      this.Name = name;
    }

    public string Name { get; private set; }

    protected override IEnumerable<object> GetAtomicValues()
    {
      yield return this.Name;
    }

    protected AccountEntryType()
    {
      // Требует Entity framework core
    }
  }
}