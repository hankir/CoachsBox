using System.Collections.Generic;
using CoachsBox.Core;

namespace CoachsBox.Coaching.StudentModel
{
  /// <summary>
  /// Родство.
  /// </summary>
  public class Relationship : ValueObject
  {
    public static Relationship Mother { get { return new Relationship(nameof(Mother)); } }

    public static Relationship Father { get { return new Relationship(nameof(Father)); } }

    public static Relationship Grandmother { get { return new Relationship(nameof(Grandmother)); } }

    public static Relationship Grandfather { get { return new Relationship(nameof(Grandfather)); } }

    private Relationship(string name)
    {
      this.Name = name;
    }

    public string Name { get; private set; }

    protected override IEnumerable<object> GetAtomicValues()
    {
      yield return this.Name;
    }

    private Relationship()
    {
      // Требует Entity framework core
    }
  }
}
