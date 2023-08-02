using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace CoachsBox.Coaching.Infrastructure.MySqlMigrations
{
  public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<CoachsBoxContext>
  {
    public CoachsBoxContext CreateDbContext(string[] args)
    {
      var configurationBuilder = new ConfigurationBuilder()
        .SetBasePath(Directory.GetCurrentDirectory())
        .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
      var configuration = configurationBuilder.Build();

      var builder = new DbContextOptionsBuilder<CoachsBoxContext>();
      var connectionString = configuration.GetConnectionString("MigrationDB");
      builder.UseMySql(connectionString, b => b.MigrationsAssembly("CoachsBox.Coaching.Infrastructure.MySqlMigrations"));
      return new CoachsBoxContext(builder.Options, new NoMediator());
    }
  }
}
