using Microsoft.AspNetCore.Identity;

namespace CoachsBox.WebApp.Areas.Identity.Data
{
  public class CoachsBoxWebAppUser : IdentityUser
  {
    /// <summary>
    /// Получить или установить идентификатор персоны.
    /// </summary>
    public int? PersonId { get; set; }
  }
}
