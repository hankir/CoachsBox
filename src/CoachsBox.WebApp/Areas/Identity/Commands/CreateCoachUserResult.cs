using System;
using Microsoft.AspNetCore.Identity;

namespace CoachsBox.WebApp.Areas.Identity.Commands
{
  public class CreateCoachUserResult
  {
    public CreateCoachUserResult(IdentityResult identityResult, string userId)
    {
      if (identityResult == null)
        throw new ArgumentNullException(nameof(identityResult));

      this.IdentityResult = identityResult;
      this.UserId = userId;
    }

    public IdentityResult IdentityResult { get; }

    public string UserId { get; }

    public bool IsSucceeded() => this.IdentityResult.Succeeded;
  }
}
