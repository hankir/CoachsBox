using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CoachsBox.WebApp.Areas.Admin.Pages.Salaries
{
  public class EditModel : PageModel
  {
    public int SalaryId { get; set; }

    public void OnGet(int salaryId)
    {
      this.SalaryId = salaryId;
    }
  }
}
