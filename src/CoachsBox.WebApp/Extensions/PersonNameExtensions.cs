using CoachsBox.Coaching.PersonModel;

namespace CoachsBox.WebApp.Extensions
{
  public static class PersonNameExtensions
  {
    public static string ShortName(this PersonName name)
    {
      if (name == null)
        return string.Empty;

      var shortName = name.Surname;
      if (!string.IsNullOrWhiteSpace(name.Name))
        shortName += $" {name.Name[0]}.";

      if (!string.IsNullOrWhiteSpace(name.Patronymic))
        shortName += $" {name.Patronymic[0]}.";

      return shortName;
    }
  }
}
