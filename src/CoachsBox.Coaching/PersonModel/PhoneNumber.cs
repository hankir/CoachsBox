using CoachsBox.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoachsBox.Coaching.PersonModel
{
  /// <summary>
  /// Значение номера телефона.
  /// </summary>
  public class PhoneNumber : ValueObject
  {
    /// <summary>
    /// Создать экземпляр значения номера телефона.
    /// </summary>
    /// <param name="number">Номер телефона.</param>
    public PhoneNumber(string number)
    {
      this.Value = number;
    }

    /// <summary>
    /// Получить значение номер телефона.
    /// </summary>
    public string Value { get; private set; }

    protected override IEnumerable<object> GetAtomicValues()
    {
      yield return this.Value;
    }

    private PhoneNumber()
    {
      // Требует Entity framework core
    }
  }
}
