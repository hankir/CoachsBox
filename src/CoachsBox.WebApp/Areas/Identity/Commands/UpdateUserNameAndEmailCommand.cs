using MediatR;
using Microsoft.AspNetCore.Identity;

namespace CoachsBox.WebApp.Areas.Identity.Commands
{
  public class UpdateUserNameAndEmailCommand : IRequest<IdentityResult>
  {
    public UpdateUserNameAndEmailCommand(string userName, string newEmail, string newUserName)
    {
      this.UserName = userName;
      this.NewEmail = newEmail;
      this.NewUserName = newUserName;
    }

    public string UserName { get; }

    public string NewEmail { get; }

    public string NewUserName { get; }
  }
}
