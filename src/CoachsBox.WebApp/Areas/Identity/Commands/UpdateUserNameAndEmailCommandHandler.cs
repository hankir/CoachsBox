using System.Threading;
using System.Threading.Tasks;
using CoachsBox.WebApp.Areas.Identity.Data;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace CoachsBox.WebApp.Areas.Identity.Commands
{
  public class UpdateUserNameAndEmailCommandHandler : IRequestHandler<UpdateUserNameAndEmailCommand, IdentityResult>
  {
    private readonly UserManager<CoachsBoxWebAppUser> userManager;

    public UpdateUserNameAndEmailCommandHandler(UserManager<CoachsBoxWebAppUser> userManager)
    {
      this.userManager = userManager;
    }

    public async Task<IdentityResult> Handle(UpdateUserNameAndEmailCommand request, CancellationToken cancellationToken)
    {
      if (request == null)
        return IdentityResult.Failed(new IdentityError()
        {
          Code = "UpdateUserNameAndEmailCommandIsNull",
          Description = "Команда для обновления эл. почты не найдена"
        });

      var user = await this.userManager.FindByNameAsync(request.UserName);
      if (user != null)
      {
        user.Email = request.NewEmail;
        user.UserName = request.NewUserName;
        return await this.userManager.UpdateAsync(user);
      }

      return IdentityResult.Failed(new IdentityError()
      {
        Code = "UserNameNotFound",
        Description = $"Пользователь '{request.UserName}' не найден"
      });
    }
  }
}
