using CoachsBox.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoachsBox.Coaching.PersonModel
{
  /// <summary>
  /// Адрес электронной почты.
  /// </summary>
  public class EmailAddress : ValueObject
  {
    /// <summary>
    /// Создать экземпляр адреса электронной почты.
    /// </summary>
    /// <param name="email"></param>
    public EmailAddress(string email)
    {
      this.Value = email;
    }

    /// <summary>
    /// Получить значение адреса электронной почты.
    /// </summary>
    public string Value { get; private set; }

    protected override IEnumerable<object> GetAtomicValues()
    {
      yield return this.Value;
    }

    private EmailAddress()
    {
      // Требует Entity framework core
    }
  }
}
