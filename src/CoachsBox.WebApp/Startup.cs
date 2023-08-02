using System;
using System.Globalization;
using System.IO;
using System.Reflection;
using CoachsBox.WebApp.Areas.Identity.Data;
using CoachsBox.WebApp.Jobs;
using Coravel;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace CoachsBox.WebApp
{
  public class Startup
  {
    private static readonly CultureInfo EnUSCulture = CultureInfo.GetCultureInfo("en-US");

    private readonly IWebHostEnvironment environment;

    public Startup(IConfiguration configuration, IWebHostEnvironment environment)
    {
      this.Configuration = configuration;
      this.environment = environment;
    }

    public IConfiguration Configuration { get; }

    // This method gets called by the runtime. Use this method to add services to the container.
    public void ConfigureServices(IServiceCollection services)
    {
      services.AddHttpContextAccessor();
      services.AddMediatR(typeof(Startup).GetTypeInfo().Assembly);
      services.Configure<CookiePolicyOptions>(options =>
      {
        // This lambda determines whether user consent for non-essential cookies is needed for a given request.
        options.CheckConsentNeeded = context => true;
        options.MinimumSameSitePolicy = SameSiteMode.None;
      });

      if (!environment.IsDevelopment())
      {
        services.AddHttpsRedirection(options =>
        {
          options.RedirectStatusCode = StatusCodes.Status301MovedPermanently;
          options.HttpsPort = 443;
        });
      }

      services.AddAuthorization(options =>
      {
        options.AddPolicy(CoachsBoxWebAppRole.AdministratorPolicyName, policy => policy.RequireRole(CoachsBoxWebAppRole.Administrator));
      });

      services.AddSession(options =>
      {
        options.Cookie.Name = ".CoachsBox.Session";
        options.IdleTimeout = TimeSpan.FromSeconds(10);
        options.Cookie.IsEssential = true;
      });

      services.AddControllersWithViews();
      services
        .AddRazorPages()
        .AddRazorPagesOptions(option =>
        {
          option.Conventions.AuthorizeFolder("/");
          option.Conventions.AuthorizeAreaFolder("Admin", "/", CoachsBoxWebAppRole.AdministratorPolicyName);
          option.Conventions.AllowAnonymousToPage("/Index");
        });

      services.AddLocalization(options => options.ResourcesPath = "Resources");

      var dataProtectionConfig = this.Configuration.GetSection("DataProtection");
      if (dataProtectionConfig != null)
      {
        var directory = dataProtectionConfig["Directory"];
        var certificateThumbprint = dataProtectionConfig["CertificateThumbprint"];
        var dataProtectionBuilder = services.AddDataProtection();
        if (!string.IsNullOrWhiteSpace(directory))
          dataProtectionBuilder.PersistKeysToFileSystem(new DirectoryInfo(directory));

        if (!string.IsNullOrWhiteSpace(certificateThumbprint))
          dataProtectionBuilder.ProtectKeysWithCertificate(certificateThumbprint);
      }

      services.AddCoachsBoxBackgroundServices(this.environment, this.Configuration);
      services.AddMemoryUsageLog();

      services.AddServerSideBlazor();
      services.AddSignalR(hubOptions =>
      {
        // Нужно для больших страниц Razor Pages + Blazor.
        // https://stackoverflow.com/questions/60311852/error-connection-disconnected-with-error-error-server-returned-an-error-on-cl
        hubOptions.MaximumReceiveMessageSize = 1 * 1024 * 1024; // 1 мб.
      });

      services.AddQueue();
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
      if (env.IsDevelopment())
      {
        app.UseDeveloperExceptionPage();
      }
      else
      {
        app.UseExceptionHandler("/Error");
        // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
        app.UseHsts();
      }

      app.UseHttpsRedirection();
      app.UseDefaultFiles();

      var supportedCultures = new[]
      {
        EnUSCulture,
        new CultureInfo("ru"),
      };

      app.UseRequestLocalization(new RequestLocalizationOptions
      {
        DefaultRequestCulture = new RequestCulture(EnUSCulture),
        // Formatting numbers, dates, etc.
        SupportedCultures = supportedCultures,
        // UI strings that we have localized.
        SupportedUICultures = supportedCultures
      });

      app.UseStaticFiles();
      app.UseCookiePolicy();
      app.UseSession();

      app.UseRouting();

      app.UseAuthentication();
      app.UseAuthorization();

      app.UseEndpoints(endpoints =>
      {
        endpoints.MapControllers();
        endpoints.MapRazorPages();
        endpoints.MapBlazorHub();
      });
    }
  }
}
