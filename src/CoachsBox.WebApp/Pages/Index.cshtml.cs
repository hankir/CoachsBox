using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CoachsBox.WebApp.Pages
{
  public class IndexModel : PageModel
  {
    public DateTime WeekDate { get; set; }

    public void OnGet(string weekDate)
    {
      if (DateTime.TryParse(weekDate, out var weekDateParsed))
        this.WeekDate = weekDateParsed;
      else
        this.WeekDate = this.User.GetUserNow().Date;
    }

    public IActionResult OnGetMoveNext(string fromWeekDate)
    {
      if (!DateTime.TryParse(fromWeekDate, out var weekDateParsed))
        weekDateParsed = this.User.GetUserNow().Date;
      return this.RedirectToPage(new { weekDate = weekDateParsed.AddDays(7).ToShortDateString() });
    }

    public IActionResult OnGetMovePrev(string fromWeekDate)
    {
      if (!DateTime.TryParse(fromWeekDate, out var weekDateParsed))
        weekDateParsed = this.User.GetUserNow().Date;
      return this.RedirectToPage(new { weekDate = weekDateParsed.AddDays(-7).ToShortDateString() });
    }

    public IActionResult OnGetMoveToMonth(string fromWeekDate, int month)
    {
      if (!DateTime.TryParse(fromWeekDate, out var weekDateParsed))
        weekDateParsed = this.User.GetUserNow().Date;
      return this.RedirectToPage(new { weekDate = new DateTime(weekDateParsed.Year, month, 1).ToShortDateString() });
    }

    public IActionResult OnGetMoveToYear(string fromWeekDate, int year)
    {
      if (!DateTime.TryParse(fromWeekDate, out var weekDateParsed))
        weekDateParsed = this.User.GetUserNow().Date;
      var maxDayInMonth = DateTime.DaysInMonth(year, weekDateParsed.Month);
      return this.RedirectToPage(new { weekDate = new DateTime(year, weekDateParsed.Month, Math.Min(weekDateParsed.Day, maxDayInMonth)).ToShortDateString() });
    }
  }
}
