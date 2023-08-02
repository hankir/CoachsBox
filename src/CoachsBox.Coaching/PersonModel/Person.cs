using System;
using CoachsBox.Core;
using CoachsBox.Core.Primitives;

namespace CoachsBox.Coaching.PersonModel
{
  public class Person : BaseEntity
  {
    public Person(PersonName name)
    {
      this.Name = name;
      this.Gender = Gender.Empty;
      this.Birthday = Date.Empty;
    }

    public Person(PersonName name, Gender gender)
    {
      this.Name = name;
      this.Gender = gender;
      this.Birthday = Date.Empty;
    }

    public Person(PersonName name, Gender gender, Date birthdate)
    {
      this.Name = name;
      this.Gender = gender;
      this.Birthday = birthdate;
    }

    /// <summary>
    /// Получить имя персоны.
    /// </summary>
    public PersonName Name { get; private set; }

    /// <summary>
    /// Получить пол персоны.
    /// </summary>
    public Gender Gender { get; private set; }

    /// <summary>
    /// Получить дату рождения.
    /// </summary>
    public Date Birthday { get; private set; }

    /// <summary>
    /// Получить контактную информацию персоны.
    /// </summary>
    public PersonalInformation PersonalInformation { get; private set; }

    /// <summary>
    /// Связать контактную информацию с персоной.
    /// </summary>
    /// <param name="contactInformation">Контактная информация.</param>
    public void AssignPersonalInformation(PersonalInformation contactInformation)
    {
      this.PersonalInformation = contactInformation;
    }

    /// <summary>
    /// Скорректировать имя персоны.
    /// </summary>
    /// <param name="name">Скорректированное имя персоны.</param>
    public void CorrectName(PersonName name)
    {
      this.Name = name;
    }

    /// <summary>
    /// Скорректировать пол.
    /// </summary>
    /// <param name="gender">Пол.</param>
    public void CorrectGender(Gender gender)
    {
      this.Gender = gender;
    }

    /// <summary>
    /// Скорректировть дату рождения.
    /// </summary>
    /// <param name="birthday">Дата рождения.</param>
    public void CorrectBirthday(Date birthday)
    {
      this.Birthday = birthday;
    }

    /// <summary>
    /// Город персоны.
    /// </summary>
    /// <returns></returns>
    public string City()
    {
      return this.PersonalInformation?.Address?.City;
    }

    /// <summary>
    /// Субъект страны персоны.
    /// </summary>
    /// <returns>Название субъекта страны персоны.</returns>
    public string State()
    {
      return this.PersonalInformation?.Address?.State;
    }

    /// <summary>
    /// Улица персоны.
    /// </summary>
    /// <returns>Название улицы персоны.</returns>
    public string Street()
    {
      return this.PersonalInformation?.Address?.Street;
    }

    /// <summary>
    /// Страна персоны.
    /// </summary>
    /// <returns>Название страны персоны.</returns>
    public string Country()
    {
      return this.PersonalInformation?.Address?.Country;
    }

    /// <summary>
    /// Почтовый индекс персоны.
    /// </summary>
    /// <returns>Номер почтового индекса персоны.</returns>
    public string ZipCode()
    {
      return this.PersonalInformation?.Address?.ZipCode;
    }

    /// <summary>
    /// Эл. почта персоны.
    /// </summary>
    /// <returns></returns>
    public string Email()
    {
      return this.PersonalInformation?.Email?.Value;
    }

    /// <summary>
    /// Телефон персоны.
    /// </summary>
    /// <returns>Номер телефона персоны.</returns>
    public string PhoneNumber()
    {
      return this.PersonalInformation?.PhoneNumber?.Value;
    }

    private Person()
    {
      // Требует Entity framework core
    }
  }
}
