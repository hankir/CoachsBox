using System;
using MediatR;

namespace CoachsBox.WebApp.Areas.Identity.Commands
{
  public class SendInvitationCommand : IRequest
  {
    public SendInvitationCommand(string userId)
    {
      if (string.IsNullOrWhiteSpace(userId))
        throw new ArgumentException("User id should be not empty", nameof(userId));

      this.UserId = userId;
    }

    public string UserId { get; }
  }
}
