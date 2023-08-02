using System.Collections.Generic;
using CoachsBox.Coaching.StudentModel;

namespace CoachsBox.WebApp.AppFacade.Students
{
  public static class Extensions
  {
    private static readonly Dictionary<string, string> relationshipLocalizations = new Dictionary<string, string>()
    {
      { Relationship.Mother.Name, "Мама" },
      { Relationship.Father.Name, "Папа" },
      { Relationship.Grandmother.Name, "Бабушка" },
      { Relationship.Grandfather.Name, "Дедушка" }
    };

    public static string GetLocalization(this Relationship relationship)
    {
      if (!relationshipLocalizations.TryGetValue(relationship.Name, out var result))
        result = relationship.Name;
      return result;
    }
  }
}
