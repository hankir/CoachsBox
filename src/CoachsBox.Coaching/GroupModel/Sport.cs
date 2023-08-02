using System.Collections.Generic;
using CoachsBox.Core;

namespace CoachsBox.Coaching.GroupModel
{
  /// <summary>
  /// Перечисление видов спорта.
  /// </summary>
  /// <remarks>Пока нужен только один вид - тхэквондо.</remarks>
  public class Sport : ValueObject
  {
    public static Sport Taekwondo { get { return new Sport("Taekwondo"); } }

    public Sport(string name)
    {
      this.Name = name;
    }

    public string Name { get; private set; }

    protected override IEnumerable<object> GetAtomicValues()
    {
      yield return this.Name;
    }

    private Sport()
    {
      // Требует Entity framework core
    }
  }
}
