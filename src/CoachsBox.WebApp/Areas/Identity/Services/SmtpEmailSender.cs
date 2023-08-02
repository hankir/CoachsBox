using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.UI.Services;

namespace CoachsBox.WebApp.Areas.Identity.Services
{
  public class SmtpEmailSender : IEmailSender
  {
    protected readonly string host;
    protected readonly int port;
    protected readonly bool enableSSL;
    protected readonly string userName;
    protected readonly string password;

    public SmtpEmailSender(string host, int port, bool enableSSL, string userName, string password)
    {
      this.host = host;
      this.port = port;
      this.enableSSL = enableSSL;
      this.userName = userName;
      this.password = password;
    }

    public async Task SendEmailAsync(string email, string subject, string htmlMessage)
    {
      using var client = new SmtpClient(this.host, this.port)
      {
        Credentials = new NetworkCredential(this.userName, this.password),
        EnableSsl = this.enableSSL
      };
      using var message = new MailMessage(this.userName, email, subject, htmlMessage) { IsBodyHtml = true };
      await client.SendMailAsync(message);
    }
  }

  public class SmtpEmailSenderWithAttachments : SmtpEmailSender, IEmailSenderWithAttachments
  {
    public SmtpEmailSenderWithAttachments(string host, int port, bool enableSSL, string userName, string password)
      : base(host, port, enableSSL, userName, password)
    {
    }

    public async Task SendEmailAsync(string email, string subject, string htmlMessage, IReadOnlyDictionary<string, Stream> attachments)
    {
      using var client = new SmtpClient(this.host, this.port)
      {
        Credentials = new NetworkCredential(this.userName, this.password),
        EnableSsl = this.enableSSL
      };
      using var message = new MailMessage(this.userName, email, subject, htmlMessage) { IsBodyHtml = true };
      foreach (var attachedStream in attachments)
      {
        message.Attachments.Add(new Attachment(attachedStream.Value, attachedStream.Key));
      }
      await client.SendMailAsync(message);
    }
  }
}
