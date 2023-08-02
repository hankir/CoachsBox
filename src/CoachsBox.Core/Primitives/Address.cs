using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CoachsBox.Core.Primitives
{
  /// <summary>
  /// Адрес.
  /// </summary>
  public class Address : ValueObject
  {
    public static Address Empty => new Address()
    {
      Street = string.Empty,
      City = string.Empty,
      State = string.Empty,
      Country = string.Empty,
      ZipCode = string.Empty
    };

    /// <summary>
    /// Создать экземпляр значения адреса.
    /// </summary>
    /// <param name="street">Улица.</param>
    /// <param name="city">Город.</param>
    /// <param name="state">Субъект.</param>
    /// <param name="country">Страна.</param>
    /// <param name="zipcode">Почтовый индекс.</param>
    public Address(string street, string city, string state, string country, string zipcode)
    {
      Street = street;
      City = city;
      State = state;
      Country = country;
      ZipCode = zipcode;
    }

    /// <summary>
    /// Получить улицу.
    /// </summary>
    public string Street { get; private set; }

    /// <summary>
    /// Получить город.
    /// </summary>
    public string City { get; private set; }

    /// <summary>
    /// Получить субъект.
    /// </summary>
    public string State { get; private set; }

    /// <summary>
    /// Получить страну.
    /// </summary>
    public string Country { get; private set; }

    /// <summary>
    /// Почтовый индекс.
    /// </summary>
    public string ZipCode { get; private set; }

    protected override IEnumerable<object> GetAtomicValues()
    {
      yield return Country;
      yield return State;
      yield return City;
      yield return Street;
      yield return ZipCode;
    }

    public override string ToString()
    {
      var builder = new StringBuilder();
      var notFirst = false;
      foreach (var addressPart in this.GetAtomicValues().OfType<string>())
      {
        if (!string.IsNullOrWhiteSpace(addressPart))
        {
          if (notFirst) builder.Append(", ");
          builder.Append(addressPart);
          notFirst = true;
        }
      }
      return builder.ToString();
    }

    private Address()
    {
      // Требует Entity framework code
    }
  }
}
