using CoachsBox.WebApp.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CoachsBox.WebApp.Models
{
  public class CoachsBoxWebAppContext : IdentityDbContext<CoachsBoxWebAppUser, CoachsBoxWebAppRole, string>
  {
    public CoachsBoxWebAppContext(DbContextOptions<CoachsBoxWebAppContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
      base.OnModelCreating(builder);
      // Customize the ASP.NET Identity model and override the defaults if needed.
      // For example, you can rename the ASP.NET Identity table names and more.
      // Add your customizations after calling base.OnModelCreating(builder);
    }
  }
}
