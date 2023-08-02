using System;
using CoachsBox.Coaching.Infrastructure;
using CoachsBox.WebApp.AppFacade;
using CoachsBox.WebApp.AppFacade.Accounting;
using CoachsBox.WebApp.AppFacade.Accounting.Internal;
using CoachsBox.WebApp.AppFacade.Internal;
using CoachsBox.WebApp.Areas.Admin.Facade;
using CoachsBox.WebApp.Areas.Admin.Facade.Internal;
using CoachsBox.WebApp.Pages.Facade;
using CoachsBox.WebApp.Pages.Facade.Internal;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MySql.Data.MySqlClient;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;

[assembly: HostingStartup(typeof(CoachsBox.WebApp.CoachingApplicationStartup))]
namespace CoachsBox.WebApp
{
  public class CoachingApplicationStartup : IHostingStartup
  {
    private static string DefaultConnectionString = "CoachsBox";

    public void Configure(IWebHostBuilder builder)
    {
      if (builder != null)
      {
        builder.ConfigureServices((context, services) =>
        {
          var environment = context.HostingEnvironment;
          var configuration = context.Configuration;

          var activeConnectionString = configuration["ActiveConnectionString"];
          if (string.IsNullOrWhiteSpace(activeConnectionString))
            activeConnectionString = DefaultConnectionString;

          var connectionStringBuilder = new MySqlConnectionStringBuilder(configuration.GetConnectionString(activeConnectionString));
          if (environment.IsDevelopment())
            connectionStringBuilder.Password = configuration["CoachsBoxConnectionPassword"];

          services.AddDbContext<CoachsBoxContext>(options =>
            options.UseMySql(connectionStringBuilder.ConnectionString, dbOptions => ConfigureMySqlDbOptions(environment, dbOptions)));

          services.AddDbContext<ReadOnlyCoachsBoxContext>(options =>
            options.UseMySql(connectionStringBuilder.ConnectionString, dbOptions => ConfigureMySqlDbOptions(environment, dbOptions)));

          services.AddCoachingInfrastructure();

          // Службы фасада
          services.AddScoped(typeof(IGroupManagmentServiceFacade), typeof(GroupManagmentServiceFacade));
          services.AddScoped(typeof(IAdministrationServiceFacade), typeof(AdministrationServiceFacade));
          services.AddScoped(typeof(ISchedulingServiceFacade), typeof(SchedulingServiceFacade));
          services.AddScoped(typeof(IAttendanceLogServiceFacade), typeof(AttendanceLogServiceFacade));
          services.AddScoped(typeof(IAccountingServiceFacade), typeof(AccountingServiceFacade));
          services.AddScoped(typeof(IDisplayNameServiceFacade), typeof(DisplayNameServiceFacade));
          services.AddScoped(typeof(ISalaryServiceFacade), typeof(SalaryServiceFacade));
        });
      }
    }

    private static void ConfigureMySqlDbOptions(IWebHostEnvironment environment, MySqlDbContextOptionsBuilder dbOptions)
    {
      dbOptions.MigrationsAssembly("CoachsBox.Coaching.Infrastructure.MySqlMigrations");
      if (environment.IsDevelopment())
        dbOptions.ServerVersion(new Version(10, 4, 6), ServerType.MariaDb);
      else
        dbOptions.ServerVersion(new Version(5, 6, 44), ServerType.MySql);
    }
  }
}
