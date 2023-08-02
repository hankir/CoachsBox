using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CoachsBox.WebApp.Areas.Identity.Data;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace CoachsBox.WebApp.Areas.Identity.Commands
{
  public class CreateCoachUserCommandHandler : IRequestHandler<CreateCoachUserCommand, CreateCoachUserResult>
  {
    private readonly UserManager<CoachsBoxWebAppUser> userManager;
    private readonly RoleManager<CoachsBoxWebAppRole> roleManager;
    private readonly ILogger<CreateCoachUserCommandHandler> logger;

    public CreateCoachUserCommandHandler(
      UserManager<CoachsBoxWebAppUser> userManager,
      RoleManager<CoachsBoxWebAppRole> roleManager,
      ILogger<CreateCoachUserCommandHandler> logger)
    {
      this.userManager = userManager;
      this.roleManager = roleManager;
      this.logger = logger;
    }

    public async Task<CreateCoachUserResult> Handle(CreateCoachUserCommand request, CancellationToken cancellationToken)
    {
      if (request == null)
        throw new ArgumentNullException(nameof(request));

      if (!await this.roleManager.RoleExistsAsync(CoachsBoxWebAppRole.Coach))
      {
        var createRoleResult = await this.roleManager.CreateAsync(CoachsBoxWebAppRole.CreateCoach());
        if (!createRoleResult.Succeeded)
          return new CreateCoachUserResult(createRoleResult, null);
      }

      var user = new CoachsBoxWebAppUser
      {
        UserName = request.Email,
        Email = request.Email,
        PhoneNumber = request.PhoneNumber,
        PersonId = request.PersonId
      };

      var password = GenerateRandomPassword(this.userManager.Options.Password);
      var addUserResult = await userManager.CreateAsync(user, password);

      var result = addUserResult.Succeeded ?
        await this.userManager.AddToRoleAsync(user, CoachsBoxWebAppRole.Coach) :
        addUserResult;

      if (result.Succeeded)
        logger.LogInformation("Coach user created. PersonId: {PersonId}, Email: {Email}, PhoneEmail: {Phone}", request.PersonId, request.Email, user.PhoneNumber);
      else
        logger.LogInformation("Coach user failed. PersonId: {PersonId}, Email: {Email}, PhoneEmail: {Phone}, Reasons: {Reasons}",
          request.PersonId, request.Email, user.PhoneNumber, string.Join(';', result.Errors.Select(e => $"{e.Code}:{e.Description}")));

      return new CreateCoachUserResult(result, user.Id);
    }

    /// <summary>
    /// Generates a Random Password
    /// respecting the given strength requirements.
    /// </summary>
    /// <param name="opts">A valid PasswordOptions object
    /// containing the password strength requirements.</param>
    /// <returns>A random password</returns>
    /// <remarks>Source code from "https://www.ryadel.com/en/c-sharp-random-password-generator-asp-net-core-mvc"</remarks>
    public static string GenerateRandomPassword(PasswordOptions opts = null)
    {
      if (opts == null) opts = new PasswordOptions()
      {
        RequiredLength = 8,
        RequiredUniqueChars = 4,
        RequireDigit = true,
        RequireLowercase = true,
        RequireNonAlphanumeric = true,
        RequireUppercase = true
      };

      string[] randomChars = new[] {
        "ABCDEFGHJKLMNOPQRSTUVWXYZ",    // uppercase 
        "abcdefghijkmnopqrstuvwxyz",    // lowercase
        "0123456789",                   // digits
        "!@$?_-"                        // non-alphanumeric
    };
      var rand = new Random(Environment.TickCount);
      var chars = new List<char>();

      if (opts.RequireUppercase)
        chars.Insert(rand.Next(0, chars.Count),
            randomChars[0][rand.Next(0, randomChars[0].Length)]);

      if (opts.RequireLowercase)
        chars.Insert(rand.Next(0, chars.Count),
            randomChars[1][rand.Next(0, randomChars[1].Length)]);

      if (opts.RequireDigit)
        chars.Insert(rand.Next(0, chars.Count),
            randomChars[2][rand.Next(0, randomChars[2].Length)]);

      if (opts.RequireNonAlphanumeric)
        chars.Insert(rand.Next(0, chars.Count),
            randomChars[3][rand.Next(0, randomChars[3].Length)]);

      for (int i = chars.Count; i < opts.RequiredLength
          || chars.Distinct().Count() < opts.RequiredUniqueChars; i++)
      {
        string rcs = randomChars[rand.Next(0, randomChars.Length)];
        chars.Insert(rand.Next(0, chars.Count),
            rcs[rand.Next(0, rcs.Length)]);
      }

      return new string(chars.ToArray());
    }
  }
}
