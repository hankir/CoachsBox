using System;
using CoachsBox.WebApp.AppFacade;
using CoachsBox.WebApp.Jobs.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MySql.Data.MySqlClient;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;

namespace CoachsBox.WebApp.Jobs
{
  public static class ServiceCollectionExtensions
  {
    private static string DefaultConnectionString = "CoachsBoxWebAppContextConnection";

    public static void AddCoachsBoxBackgroundServices(this IServiceCollection services, IWebHostEnvironment environment, IConfiguration configuration)
    {
      var activeConnectionString = configuration["ActiveWebAppConnectionString"];
      if (string.IsNullOrWhiteSpace(activeConnectionString))
        activeConnectionString = DefaultConnectionString;

      var connectionStringBuilder = new MySqlConnectionStringBuilder(configuration.GetConnectionString(activeConnectionString));
      if (environment.IsDevelopment())
        connectionStringBuilder.Password = configuration["WebAppConnectionPassword"];

      services.AddDbContext<BackgroundServiceDbContext>(options =>
        options.UseMySql(connectionStringBuilder.ConnectionString, dbOption =>
        {
          if (environment.IsDevelopment())
            dbOption.ServerVersion(new Version(10, 4, 6), ServerType.MariaDb);
          else
            dbOption.ServerVersion(new Version(5, 6, 44), ServerType.MySql);

          dbOption.MigrationsHistoryTable("__EFBackgroundServiceDbContextMigrationsHistory");
        }));

      // Фоновые службы
      services.AddHostedService<AccrualProcessorWorker>();
      services.AddHostedService<PaymentProcessorWorker>();

      services.AddScoped(typeof(IAttendanceLogRestriction), typeof(AttendanceLogRestriction));
    }

    public static void AddMemoryUsageLog(this IServiceCollection services)
    {
      services.AddHostedService<MemoryUsageLogger>();
    }
  }
}
