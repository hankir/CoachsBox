using System.Collections.Generic;
using CoachsBox.Core;
using CoachsBox.Core.Primitives;

namespace CoachsBox.Coaching.PersonModel
{
  /// <summary>
  /// Контактная информация.
  /// </summary>
  public class PersonalInformation : ValueObject
  {
    /// <summary>
    /// Получить номер телефона.
    /// </summary>
    public PhoneNumber PhoneNumber { get; private set; }

    /// <summary>
    /// Получить адрес электронной почты.
    /// </summary>
    public EmailAddress Email { get; private set; }

    /// <summary>
    /// Получить адрес проживания.
    /// </summary>
    public Address Address { get; private set; }

    protected override IEnumerable<object> GetAtomicValues()
    {
      yield return this.PhoneNumber;
      yield return this.Email;
      yield return this.Address;
    }

    public PersonalInformation(PhoneNumber phone, EmailAddress email)
      : this(phone, email, Address.Empty)
    {
    }

    public PersonalInformation(PhoneNumber phone, EmailAddress email, Address address)
    {
      this.PhoneNumber = phone;
      this.Email = email;
      this.Address = address;
    }

    private PersonalInformation()
    {
      // Требует Entity framework core
    }
  }
}
