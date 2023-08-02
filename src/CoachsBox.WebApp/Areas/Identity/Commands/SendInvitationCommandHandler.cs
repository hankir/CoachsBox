using System;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using CoachsBox.WebApp.Areas.Identity.Data;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace CoachsBox.WebApp.Areas.Identity.Commands
{
  public class SendInvitationCommandHandler : IRequestHandler<SendInvitationCommand>
  {
    private readonly IConfiguration configuration;
    private readonly UserManager<CoachsBoxWebAppUser> userManager;
    private readonly ILogger<SendInvitationCommandHandler> logger;
    private readonly IEmailSender emailSender;

    public SendInvitationCommandHandler(
      IConfiguration configuration,
      UserManager<CoachsBoxWebAppUser> userManager,
      ILogger<SendInvitationCommandHandler> logger,
      IEmailSender emailSender)
    {
      this.configuration = configuration;
      this.userManager = userManager;
      this.logger = logger;
      this.emailSender = emailSender;
    }

    public async Task<Unit> Handle(SendInvitationCommand request, CancellationToken cancellationToken)
    {
      if (request == null)
        throw new ArgumentNullException(nameof(request));

      var user = await this.userManager.FindByIdAsync(request.UserId);
      var code = await this.userManager.GenerateEmailConfirmationTokenAsync(user);

      var host = this.configuration.GetValue<string>("LinkHost");
      var callbackUrl = $"{host}/Identity/Account/ConfirmInvitation?userId={HttpUtility.UrlEncode(user.Id)}&code={HttpUtility.UrlEncode(code)}";

      var subject = "Приглашение в цифровую тренерскую";
      var message = $@"Вы приглашены в цифровую тренерскую CoachsBox!<br />
Пожалуйста, подвердите вашу электронную почту. Перейдите по <a href='{callbackUrl}'>ссылке</a>.<br />
Если вы считаете, что вас пригласили по ошибке, то просто проигнорируйте это письмо.";

      await this.emailSender.SendEmailAsync(user.Email, subject, message);

      this.logger.LogInformation("Sending invitation success. Email: {Email}, PersonId: {PersonId}", user.Email, user.PersonId);

      return Unit.Value;
    }
  }
}
