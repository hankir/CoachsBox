using CoachsBox.WebApp.AppFacade.Students.Commands;
using CoachsBox.WebApp.Areas.Admin.Facade;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CoachsBox.WebApp
{
  public class AddRelativeModel : PageModel
  {
    private readonly IAdministrationServiceFacade administrationServiceFacade;

    public AddRelativeModel(IAdministrationServiceFacade administrationServiceFacade)
    {
      this.administrationServiceFacade = administrationServiceFacade;
    }

    [BindProperty]
    public AddOrUpdateStudentRelativeCommand Command { get; set; }

    [TempData]
    public string StudentFullName { get; set; }

    [TempData]
    public string SubmitButtonTitle { get; set; }

    [TempData]
    public string Title { get; set; }

    [TempData]
    public bool IsEditing { get; set; }

    public IActionResult OnGet(int studentId, string studentFullName)
    {
      if (studentId <= 0)
        return this.NotFound();

      this.SetTitles(studentFullName);
      this.Command = new AddOrUpdateStudentRelativeCommand()
      {
        StudentId = studentId,
        Birthdate = null
      };

      return this.Page();
    }

    public IActionResult OnGetEdit(int studentId, int personId, string studentFullName)
    {
      if (studentId <= 0 || personId <= 0)
        return this.NotFound();

      this.IsEditing = true;
      this.SetTitles(studentFullName);
      this.Command = this.administrationServiceFacade.EditStudentRelative(studentId, personId);

      return this.Page();
    }

    public IActionResult OnPost(int? personId)
    {
      this.IgnoreAddressValidation();
      if (this.ModelState.IsValid)
      {
        if (personId != null)
          this.administrationServiceFacade.UpdateStudentRelative(personId.Value, this.Command);
        else
          this.administrationServiceFacade.AddStudentRelative(this.Command);
        return this.RedirectToPage("Details", new { this.Command.StudentId });
      }

      this.SetTitles(this.StudentFullName);
      this.TempData.Keep();

      return this.Page();
    }

    private void IgnoreAddressValidation()
    {
      this.ModelState.Remove($"{nameof(this.Command)}.{nameof(this.Command.Address)}.{nameof(this.Command.Address.City)}");
      this.ModelState.Remove($"{nameof(this.Command)}.{nameof(this.Command.Address)}.{nameof(this.Command.Address.Street)}");
      this.ModelState.Remove($"{nameof(this.Command)}.{nameof(this.Command.Address)}.{nameof(this.Command.Address.Country)}");
    }

    private void SetTitles(string studentFullName)
    {
      if (this.IsEditing)
      {
        this.Title = "Изменение родственника";
        this.SubmitButtonTitle = "Сохранить";
      }
      else
      {
        this.Title = "Новый родственник";
        this.SubmitButtonTitle = "Добавить родственника";
      }
      this.StudentFullName = studentFullName;
    }
  }
}