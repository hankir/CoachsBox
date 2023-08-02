using System.Threading.Tasks;
using CoachsBox.Coaching.StudentModel;
using CoachsBox.WebApp.AppFacade.Students.Commands;
using CoachsBox.WebApp.Areas.Admin.Facade;
using CoachsBox.WebApp.Areas.Identity.Data;
using CoachsBox.WebApp.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CoachsBox.WebApp.Pages.Students
{
  public class EditModel : PageModel
  {
    private readonly IStudentRepository studentRepository;
    private readonly IAdministrationServiceFacade administrationService;

    public EditModel(
      IStudentRepository studentRepository,
      IAdministrationServiceFacade administrationService)
    {
      this.studentRepository = studentRepository;
      this.administrationService = administrationService;
    }

    [BindProperty]
    public UpdateStudentCommand UpdateCommand { get; set; }

    public async Task<IActionResult> OnGetAsync(int? studentId)
    {
      if (studentId == null)
        return NotFound();

      var student = await studentRepository.GetByIdAsync(studentId.Value);
      if (student == null)
        return NotFound();

      this.UpdateCommand = new UpdateStudentCommand(student);
      return Page();
    }

    public IActionResult OnPost()
    {
      if (!ModelState.IsValid)
      {
        // Администратору разрешается заводить ученика без адреса.
        if (this.User.IsInRole(CoachsBoxWebAppRole.Administrator))
          this.ModelState.SuppressFor(() => this.UpdateCommand.Address);

        if (!this.ModelState.IsValid)
          return this.Page();
      }

      this.administrationService.UpdateStudent(this.UpdateCommand);
      return this.RedirectToPage("Details", new { studentId = this.UpdateCommand.Id });
    }
  }
}
