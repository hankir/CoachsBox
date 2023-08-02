using System.Collections.Generic;
using System.Linq;
using CoachsBox.Core;

namespace CoachsBox.Coaching.PersonModel
{
  /// <summary>
  /// Представление имени человека.
  /// </summary>
  public class PersonName : ValueObject
  {
    public static bool TryParse(string value, out PersonName personName)
    {
      personName = default;

      if (string.IsNullOrWhiteSpace(value))
        return false;

      var nameParts = value.Trim().Split(' ', 3);
      string surname = nameParts.Length >= 1 ? nameParts[0] : null;
      string name = nameParts.Length >= 2 ? nameParts[1] : null;
      string patronymic = nameParts.Length >= 3 ? nameParts[2] : null;

      if (string.IsNullOrWhiteSpace(surname) && string.IsNullOrWhiteSpace(name) && string.IsNullOrWhiteSpace(patronymic))
        return false;

      personName = new PersonName(surname, name, patronymic);
      return true;
    }

    /// <summary>
    /// Создать значение имени человека без отчества.
    /// </summary>
    /// <param name="surname">Фамилия.</param>
    /// <param name="name">Имя.</param>
    public PersonName(string surname, string name)
        : this(surname, name, null)
    {
    }

    /// <summary>
    /// Создать значение имени человека.
    /// </summary>
    /// <param name="surname">Фамилия.</param>
    /// <param name="name">Имя.</param>
    /// <param name="patronymic">Отчество.</param>
    public PersonName(string surname, string name, string patronymic)
    {
      this.Name = name;
      this.Surname = surname;
      this.Patronymic = patronymic;
    }

    /// <summary>
    /// Получить имя человека.
    /// </summary>
    public string Name { get; private set; }

    /// <summary>
    /// Получить фамилию человека.
    /// </summary>
    public string Surname { get; private set; }

    /// <summary>
    /// Получить отчество человека.
    /// </summary>
    public string Patronymic { get; private set; }

    /// <summary>
    /// Получить полное имя персоны.
    /// </summary>
    /// <returns>Полное имя персоны - Фамилия Имя Отчество. Пустые части имени будут пропущены.</returns>
    public string FullName()
    {
      var fullNameParts = new string[] { this.Surname, this.Name, this.Patronymic }
        .Where(p => !string.IsNullOrWhiteSpace(p));
      return string.Join(" ", fullNameParts);
    }

    protected override IEnumerable<object> GetAtomicValues()
    {
      yield return this.Name;
      yield return this.Surname;
      yield return this.Patronymic;
    }

    private PersonName()
    {
      // Требует Entity framework core
    }
  }
}
