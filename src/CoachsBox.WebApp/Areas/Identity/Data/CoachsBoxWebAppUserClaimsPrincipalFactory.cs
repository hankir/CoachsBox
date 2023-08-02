using System;
using System.Globalization;
using System.Security.Claims;
using System.Threading.Tasks;
using CoachsBox.Coaching.CoachModel;
using CoachsBox.Coaching.PersonModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace CoachsBox.WebApp.Areas.Identity.Data
{
  public class CoachsBoxWebAppUserClaimsPrincipalFactory : UserClaimsPrincipalFactory<CoachsBoxWebAppUser, CoachsBoxWebAppRole>
  {
    private readonly IPersonRepository personRepository;
    private readonly ICoachRepository coachRepository;
    private readonly IHttpContextAccessor httpContextAccessor;

    public CoachsBoxWebAppUserClaimsPrincipalFactory(
      UserManager<CoachsBoxWebAppUser> userManager,
      RoleManager<CoachsBoxWebAppRole> roleManager,
      IOptions<IdentityOptions> optionsAccessor,
      IPersonRepository personRepository,
      ICoachRepository coachRepository,
      IHttpContextAccessor httpContextAccessor)
      : base(userManager, roleManager, optionsAccessor)
    {
      this.personRepository = personRepository;
      this.coachRepository = coachRepository;
      this.httpContextAccessor = httpContextAccessor;
    }

    protected override async Task<ClaimsIdentity> GenerateClaimsAsync(CoachsBoxWebAppUser user)
    {
      var id = await base.GenerateClaimsAsync(user);

      if (user.PersonId != null)
      {
        var personId = user.PersonId.Value;
        var coach = await this.coachRepository.GetByPersonIdAsync(personId);
        if (coach != null)
          id.AddClaim(new Claim(CoachsBoxClaimTypes.CoachId, coach.Id.ToString(CultureInfo.InvariantCulture)));

        var person = coach?.Person ?? await this.personRepository.GetByIdAsync(personId);
        id.AddClaim(new Claim(ClaimTypes.Surname, person.Name.Surname));
        id.AddClaim(new Claim(ClaimTypes.GivenName, person.Name.Name));
      }

      if (this.httpContextAccessor.HttpContext.Request.Cookies.TryGetValue(".JSTimeZoneOffset", out var timezoneoffset))
      {
        if (int.TryParse(timezoneoffset, out var minutes))
        {
          id.AddClaim(new Claim(CoachsBoxClaimTypes.TimeZoneOffset, TimeSpan.FromMinutes(-minutes).ToString()));
        }
      }

      return id;
    }
  }
}
