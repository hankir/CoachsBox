using Microsoft.AspNetCore.Identity;

namespace CoachsBox.WebApp.Areas.Identity.Data
{
  public class CoachsBoxWebAppRole : IdentityRole
  {
    /// <summary>
    /// Роль "Администратор".
    /// </summary>
    public const string Administrator = nameof(Administrator);

    /// <summary>
    /// Роль "Тренер".
    /// </summary>
    public const string Coach = nameof(Coach);

    public static string AdministratorPolicyName = PolicyNameForRole(Administrator);

    public static string CoachPolicyName = PolicyNameForRole(Coach);

    public static CoachsBoxWebAppRole CreateAdmin()
    {
      return new CoachsBoxWebAppRole(Administrator);
    }

    public static CoachsBoxWebAppRole CreateCoach()
    {
      return new CoachsBoxWebAppRole(Coach);
    }

    private static string PolicyNameForRole(string roleName)
    {
      return $"Require{roleName}";
    }

    public CoachsBoxWebAppRole(string roleName)
      : base(roleName)
    {
    }

    private CoachsBoxWebAppRole()
    {
      // Требует EntityFrameworkCore.
    }
  }
}
