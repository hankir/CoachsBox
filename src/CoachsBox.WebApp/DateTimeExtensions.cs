using System;
using System.Security.Claims;
using CoachsBox.Core;
using CoachsBox.WebApp.Areas.Identity.Data;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CoachsBox.WebApp
{
  public static class DateTimeExtensions
  {
    public static DateTime ToUserTime(this ClaimsPrincipal user, DateTime dateTime)
    {
      if (user != null)
      {
        var offset = CoachsBoxClaimTypes.GetTimeZoneOffset(user);
        return new DateTimeOffset(dateTime, Watch.TimeZone.BaseUtcOffset).ToOffset(offset).DateTime;
      }
      return dateTime;
    }

    public static DateTime FromUserTime(this ClaimsPrincipal user, DateTime dateTime)
    {
      if (user != null)
      {
        var offset = CoachsBoxClaimTypes.GetTimeZoneOffset(user);
        return new DateTimeOffset(dateTime, offset).ToOffset(Watch.TimeZone.BaseUtcOffset).DateTime;
      }
      return dateTime;
    }

    public static DateTime GetUserNow(this ClaimsPrincipal user)
    {
      return user.ToUserTime(Watch.Now.DateTime);
    }

    public static IHtmlContent DisplayUserDateTimeFor<TModel>(this IHtmlHelper<TModel> htmlHelper, ClaimsPrincipal user, DateTime dateTime)
    {
      var userDateTime = user.ToUserTime(dateTime);
      return htmlHelper.DisplayFor(model => userDateTime);
    }

    public static DateTime StartOfWeek(this DateTime dt, DayOfWeek startOfWeek)
    {
      int diff = (7 + (dt.DayOfWeek - startOfWeek)) % 7;
      return dt.AddDays(-1 * diff).Date;
    }
  }
}
