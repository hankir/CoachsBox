using System;
using CoachsBox.WebApp.Areas.Identity.Data;
using CoachsBox.WebApp.Areas.Identity.Pages.Account.Manage;
using CoachsBox.WebApp.Areas.Identity.Services;
using CoachsBox.WebApp.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MySql.Data.MySqlClient;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;

[assembly: HostingStartup(typeof(CoachsBox.WebApp.Areas.Identity.IdentityHostingStartup))]
namespace CoachsBox.WebApp.Areas.Identity
{
  public class IdentityHostingStartup : IHostingStartup
  {
    private static string DefaultConnectionString = "CoachsBoxWebAppContextConnection";

    public void Configure(IWebHostBuilder builder)
    {
      if (builder != null)
      {
        builder.ConfigureServices((context, services) =>
        {
          var environment = context.HostingEnvironment;
          var configuration = context.Configuration;

          var activeConnectionString = configuration["ActiveWebAppConnectionString"];
          if (string.IsNullOrWhiteSpace(activeConnectionString))
            activeConnectionString = DefaultConnectionString;

          var connectionStringBuilder = new MySqlConnectionStringBuilder(configuration.GetConnectionString(activeConnectionString));
          if (environment.IsDevelopment())
            connectionStringBuilder.Password = configuration["WebAppConnectionPassword"];

          services.AddDbContext<CoachsBoxWebAppContext>(options =>
            options.UseMySql(connectionStringBuilder.ConnectionString, dbOption =>
            {
              if (environment.IsDevelopment())
                dbOption.ServerVersion(new Version(10, 4, 6), ServerType.MariaDb);
              else
                dbOption.ServerVersion(new Version(5, 6, 44), ServerType.MySql);
            }));

          services
            .AddIdentity<CoachsBoxWebAppUser, CoachsBoxWebAppRole>(config =>
            {
              config.SignIn.RequireConfirmedEmail = true;
            })
            .AddErrorDescriber<CoachsBoxIdentityErrorDescriber>()
            .AddEntityFrameworkStores<CoachsBoxWebAppContext>()
            .AddDefaultUI()
            .AddDefaultTokenProviders();

          services.ConfigureApplicationCookie(options =>
          {
            if (TimeSpan.TryParse(configuration["ApplicationCookieExpireTimeSpan"], out TimeSpan expireTimeSpan))
            {
              options.ExpireTimeSpan = expireTimeSpan;
            }
            else
            {
              // TODO: logging
              // this.logger.LogWarning("appsettings.json parameter not parsed. Param name: ApplicationCookieExpireTimeSpan. Use default.);
            }
          });

          services.AddScoped<IUserClaimsPrincipalFactory<CoachsBoxWebAppUser>, CoachsBoxWebAppUserClaimsPrincipalFactory>();

          if (environment.IsDevelopment())
            services.AddTransient<IEmailSender, DebugEmailSender>();
          else
            services.AddTransient<IEmailSender, SmtpEmailSender>(context =>
            {
              return new SmtpEmailSender(
                configuration["SmtpEmailSender:Host"],
                configuration.GetValue<int>("SmtpEmailSender:Port"),
                configuration.GetValue<bool>("SmtpEmailSender:EnableSSL"),
                configuration["SmtpEmailSender:UserName"],
                configuration["SmtpEmailSender:Password"]
                );
            });
          services.AddTransient<IEmailSenderWithAttachments, SmtpEmailSenderWithAttachments>(context =>
          {
            return new SmtpEmailSenderWithAttachments(
              configuration["SmtpEmailSender:Host"],
              configuration.GetValue<int>("SmtpEmailSender:Port"),
              configuration.GetValue<bool>("SmtpEmailSender:EnableSSL"),
              configuration["SmtpEmailSender:UserName"],
              configuration["SmtpEmailSender:Password"]
              );
          });
          services.AddTransient<SendSummaryGroupsAttendanceLogByMonthMail>();
          services.AddTransient<SummaryDataForSalaryByMonthMail>();
        });
      }
    }
  }
}