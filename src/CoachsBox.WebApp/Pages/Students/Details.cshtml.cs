using CoachsBox.Coaching.StudentModel;
using CoachsBox.WebApp.AppFacade.Students.DTO;
using CoachsBox.WebApp.Areas.Admin.Facade;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CoachsBox.WebApp.Pages.Students
{
  public class DetailsModel : PageModel
  {
    private readonly IAdministrationServiceFacade administrationServiceFacade;

    public DetailsModel(IAdministrationServiceFacade administrationServiceFacade)
    {
      this.administrationServiceFacade = administrationServiceFacade;
    }

    public StudentDetailsDTO Student { get; set; }

    public IActionResult OnGetAsync(int? studentId)
    {
      if (studentId == null)
        return NotFound();

      this.Student = this.administrationServiceFacade.StudentDetails(studentId.Value);

      if (this.Student == null)
        return NotFound();

      return Page();
    }

    public IActionResult OnPostAddStudentRelative()
    {
      this.ViewData["AddStudentRelativeInvalidForm"] = false;
      return this.Page();
    }
  }
}
