using System.Collections.Generic;
using CoachsBox.Core;

namespace CoachsBox.Accounting
{
  public abstract class AccountingEventType : ValueObject
  {
    public AccountingEventType(string name)
    {
      this.Name = name;
    }

    public string Name { get; private set; }

    protected override IEnumerable<object> GetAtomicValues()
    {
      yield return this.Name;
    }

    protected AccountingEventType()
    {
      // Требует Entity framework core
    }
  }
}
