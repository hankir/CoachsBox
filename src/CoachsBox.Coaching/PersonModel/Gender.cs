using System.Collections.Generic;
using System.Linq;
using CoachsBox.Core;

namespace CoachsBox.Coaching.PersonModel
{
  /// <summary>
  /// Перечисление полов.
  /// </summary>
  public sealed class Gender : ValueObject
  {
    /// <summary>
    /// Получить пустое значение пола.
    /// </summary>
    public static Gender Empty { get { return new Gender(nameof(Empty)); } }

    /// <summary>
    /// Получить значение мужского пола.
    /// </summary>
    public static Gender Male { get { return new Gender(nameof(Male)); } }

    /// <summary>
    /// Получить значение женского пола.
    /// </summary>
    public static Gender Female { get { return new Gender(nameof(Female)); } }

    public static Gender Create(string fromString)
    {
      return Gender.GetAll<Gender>().Where(g => g.Value == fromString).Single();
    }

    private Gender(string genderValue)
    {
      this.Value = genderValue;
    }

    /// <summary>
    /// Получить значение пола.
    /// </summary>
    public string Value { get; private set; }

    protected override IEnumerable<object> GetAtomicValues()
    {
      yield return this.Value;
    }

    private Gender()
    {
      // Требует Entity framework core
    }
  }
}
