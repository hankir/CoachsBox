using System;
using CoachsBox.WebApp.Areas.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CoachsBox.WebApp.Pages.Attendance
{
  public class EditModel : PageModel
  {
    public int CoachId { get; private set; }

    public int GroupId { get; private set; }

    public DateTime Date { get; private set; }

    public TimeSpan Start { get; private set; }

    public TimeSpan End { get; private set; }

    public IActionResult OnGet(int groupId, DateTime date, TimeSpan start, TimeSpan end)
    {
      this.CoachId = this.User.GetCoachId();
      this.GroupId = groupId;
      this.Date = date;
      this.Start = start;
      this.End = end;
      return this.Page();
    }
  }
}