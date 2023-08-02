using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoachsBox.Coaching.Application.Commands;
using CoachsBox.Coaching.PersonModel;
using CoachsBox.Coaching.StudentModel;
using CoachsBox.WebApp.Areas.Admin.Facade.Internal;
using CoachsBox.WebApp.Pages.Facade.DTO;
using CoachsBox.WebApp.Pages.Facade.Internal.Assembler;
using MediatR;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.Extensions.DependencyInjection;

namespace CoachsBox.WebApp.Pages.Groups.Components
{
  public partial class EnrollStudentButton : OwningComponentBase<IMediator>
  {
    private string surname;
    private string name;
    private string patronymic;
    private bool isFinding = false;
    private IReadOnlyCollection<GroupStudentDTO> studentList = new List<GroupStudentDTO>();

    [Parameter]
    public int GroupId { get; set; }

    [Parameter]
    public EventCallback<int> StudentAdded { get; set; }

    public async Task OnFindButtonClick()
    {
      if (string.IsNullOrWhiteSpace(this.surname) && string.IsNullOrWhiteSpace(this.name) && string.IsNullOrWhiteSpace(this.patronymic))
      {
        this.studentList = new List<GroupStudentDTO>();
        this.StateHasChanged();
        return;
      }

      try
      {
        this.isFinding = true;
        var personName = new PersonName(this.surname, this.name, this.patronymic);
        var findByNameSpecification = new FindStudentsByNameSpecification(personName).WithPerson();
        var studentRepository = this.ScopedServices.GetRequiredService<IStudentRepository>();
        var exisingStudents = await studentRepository.ListAsync(findByNameSpecification);

        var personAssembler = new PersonDTOAssembler();
        var studentAssembler = new GroupStudentDTOAssembler();
        this.studentList = studentAssembler.ToDTOList(exisingStudents, personAssembler);
      }
      finally
      {
        this.isFinding = false;
        this.StateHasChanged();
      }
    }

    public void OnTrialTrainingChecked(int studentId, ChangeEventArgs eventArgs)
    {
      var foundedStudent = this.studentList.SingleOrDefault(student => student.StudentId == studentId);
      foundedStudent.IsTrialTraining = (bool)eventArgs.Value;
    }

    public async Task OnKeyDown(KeyboardEventArgs e)
    {
      if (e.Code == "Enter" || e.Code == "NumpadEnter")
        await this.OnFindButtonClick();
    }

    public async Task OnAddClick(int studentId)
    {
      var candidate = this.studentList.SingleOrDefault(student => student.StudentId == studentId);
      if (candidate == null)
        return;

      await this.Service.Send(new EnrollStudentCommand(this.GroupId, studentId, candidate.IsTrialTraining));
      await this.StudentAdded.InvokeAsync(studentId);
      this.ClearForm();
    }

    private void ClearForm()
    {
      this.surname = string.Empty;
      this.name = string.Empty;
      this.patronymic = string.Empty;
      this.studentList = new List<GroupStudentDTO>();
      this.StateHasChanged();
    }
  }
}
