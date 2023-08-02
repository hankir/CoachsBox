using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.UI.Services;

namespace CoachsBox.WebApp.Areas.Identity.Services
{
  public interface IEmailSenderWithAttachments : IEmailSender
  {
    Task SendEmailAsync(string email, string subject, string htmlMessage, IReadOnlyDictionary<string, Stream> attachments);
  }
}
