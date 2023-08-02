using System;
using System.Runtime;
using CoachsBox.Coaching.Infrastructure;
using CoachsBox.Core;
using CoachsBox.WebApp.Jobs.Data;
using CoachsBox.WebApp.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;

namespace CoachsBox.WebApp
{
  public class Program
  {
    public static void Main(string[] args)
    {
      var host = CreateHostBuilder(args).Build();
      using (var scope = host.Services.CreateScope())
      {
        var services = scope.ServiceProvider;
        var logger = services.GetRequiredService<ILogger<Program>>();
        ConfigureTimeZone(services, logger);
        MigrateDatabase(services, logger);
        logger.LogDebug("Is server garbage collection is {IsServerGC}", GCSettings.IsServerGC ? "enabled" : "disabled");
      }

      host.Run();
    }

    private static void ConfigureTimeZone(IServiceProvider services, ILogger<Program> logger)
    {
      var configuration = services.GetRequiredService<IConfiguration>();
      var timeZoneId = configuration["TimeZone"];
      if (!string.IsNullOrWhiteSpace(timeZoneId))
      {
        try
        {
          var timeZone = TimeZoneInfo.FindSystemTimeZoneById(timeZoneId);
          Watch.WindUp(new TimeZoneDateTimeProvider(timeZone));
          logger.LogInformation("Configure time zone to '{TimeZoneDisplayName}' by time zone id '{TimeZoneId}'", timeZone.DisplayName, timeZone.Id);
        }
        catch (TimeZoneNotFoundException tne)
        {
          logger.LogError(tne, "Time zone '{TimeZone}' not found.", timeZoneId);
        }
        catch (Exception ex)
        {
          logger.LogError(ex, "An error occurred while configure timezone '{TimeZone}'.", timeZoneId);
        }
      }
      else
      {
        logger.LogInformation("Use default time zone '{TimeZoneDisplayName}' with id {TimeZoneId}", Watch.TimeZone.DisplayName, Watch.TimeZone.Id);
      }
    }

    private static void MigrateDatabase(IServiceProvider services, ILogger<Program> logger)
    {
      try
      {
        var context = services.GetRequiredService<CoachsBoxContext>();
        context.Database.Migrate();

        var identityContext = services.GetRequiredService<CoachsBoxWebAppContext>();
        identityContext.Database.Migrate();

        var serviceContext = services.GetRequiredService<BackgroundServiceDbContext>();
        serviceContext.Database.Migrate();

        logger.LogInformation("Database is ready.");
      }
      catch (Exception ex)
      {
        logger.LogError(ex, "An error occurred while seeding the database.");
      }
    }

    public static IHostBuilder CreateHostBuilder(string[] args) =>
      Host.CreateDefaultBuilder(args)
        .ConfigureWebHostDefaults(webBuilder => webBuilder.UseStartup<Startup>())
        .UseSerilog((context, loggerConfiguration) => loggerConfiguration.ReadFrom.Configuration(context.Configuration), writeToProviders: true);
  }
}
