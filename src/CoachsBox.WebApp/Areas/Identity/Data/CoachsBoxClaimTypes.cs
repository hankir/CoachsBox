using System;
using System.Security.Claims;
using CoachsBox.Core;

namespace CoachsBox.WebApp.Areas.Identity.Data
{
  public static class CoachsBoxClaimTypes
  {
    public const string CoachId = nameof(CoachId);

    public const string TimeZoneOffset = nameof(TimeZoneOffset);

    public static int GetCoachId(this ClaimsPrincipal user)
    {
      if (user == null)
        throw new ArgumentNullException(nameof(user));

      var coachIdClaim = user.FindFirst(CoachsBoxClaimTypes.CoachId);
      return coachIdClaim != null && int.TryParse(coachIdClaim.Value, out var coachId) ? coachId : -1;
    }

    public static TimeSpan GetTimeZoneOffset(this ClaimsPrincipal user)
    {
      if (user == null)
        throw new ArgumentNullException(nameof(user));

      var timeZoneOffsetClaim = user.FindFirst(CoachsBoxClaimTypes.TimeZoneOffset);
      return timeZoneOffsetClaim != null && TimeSpan.TryParse(timeZoneOffsetClaim.Value, out var timeZoneOffset) ?
        timeZoneOffset :
        Watch.TimeZone.BaseUtcOffset;
    }
  }
}
