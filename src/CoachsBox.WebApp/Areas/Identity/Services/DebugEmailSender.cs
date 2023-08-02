using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using System.Web;

namespace CoachsBox.WebApp.Areas.Identity.Services
{
  public class DebugEmailSender : IEmailSender
  {
    private readonly ILogger<DebugEmailSender> logger;

    public DebugEmailSender(ILogger<DebugEmailSender> logger)
    {
      this.logger = logger;
    }

    public Task SendEmailAsync(string email, string subject, string htmlMessage)
    {
      this.logger.LogDebug("Send email to: {ToEmail}, subject: {Subject}, body: {Body}",
        email, subject, HttpUtility.HtmlDecode(htmlMessage));
      return Task.CompletedTask;
    }
  }
}
