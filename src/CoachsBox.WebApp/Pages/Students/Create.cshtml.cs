using System.Linq;
using CoachsBox.Coaching.PersonModel;
using CoachsBox.WebApp.AppFacade.Primitives.DTO;
using CoachsBox.WebApp.AppFacade.Students.Commands;
using CoachsBox.WebApp.Areas.Admin.Facade;
using CoachsBox.WebApp.Areas.Identity.Data;
using CoachsBox.WebApp.DataAnnotations;
using CoachsBox.WebApp.Pages.Facade;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;

namespace CoachsBox.WebApp.Pages.Students
{
  [Authorize(Roles = CoachsBoxWebAppRole.Administrator)]
  public class CreateModel : PageModel
  {
    private readonly IAdministrationServiceFacade administrationServiceFacade;
    private readonly IGroupManagmentServiceFacade groupManagmentServiceFacade;

    public CreateModel(
      IAdministrationServiceFacade administrationServiceFacade,
      IGroupManagmentServiceFacade groupManagmentServiceFacade,
      IConfiguration configuration)
    {
      this.administrationServiceFacade = administrationServiceFacade;
      this.groupManagmentServiceFacade = groupManagmentServiceFacade;
      this.Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    [BindProperty]
    public CreateStudentCommand CreateCommand { get; set; }

    public IActionResult OnGet(string surname, string name, string patronymic)
    {
      this.CreateCommand = new CreateStudentCommand()
      {
        Surname = surname,
        Name = name,
        Patronymic = patronymic,
        Gender = Gender.Male.Value,
        Birthdate = null,
        Address = new AddressDTO()
      };
      return Page();
    }

    public IActionResult OnPost()
    {
      if (!ModelState.IsValid)
      {
        // Администратору разрешается заводить ученика без адреса.
        if (this.User.IsInRole(CoachsBoxWebAppRole.Administrator))
          this.ModelState.SuppressFor(() => this.CreateCommand.Address);

        if (!this.ModelState.IsValid)
          return Page();
      }

      var existingPersons = this.administrationServiceFacade.FindExistStudent(this.CreateCommand);
      if (existingPersons.Any())
      {
        // TODO: Список похожих персон.
        this.ModelState.AddModelError("StudentAlreadyExist", "Ученик с точно таким именем уже существует");
        return this.Page();
      }

      var studentId = this.administrationServiceFacade.CreateStudent(this.CreateCommand);
      return this.RedirectToPage("./Details", new { studentId });
    }
  }
}