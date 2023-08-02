using System;
using CoachsBox.WebApp.Pages.Students.Components;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CoachsBox.WebApp.Pages.Students
{
  public class IndexModel : PageModel
  {
    public string EmptyListMessage { get; set; }

    [BindProperty]
    public string StudentName { get; set; }

    public StudentsListFilter Filter { get; set; }

    public void OnGet(string studentName)
    {
      this.Filter = StudentsListFilter.CreateByStudentName(studentName);
      this.EmptyListMessage = "Нет учеников";
    }
  }
}
