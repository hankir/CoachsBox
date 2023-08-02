using CoachsBox.WebApp.AppFacade.Primitives.Commands;

namespace CoachsBox.WebApp.AppFacade.Students.Commands
{
  public class CreateStudentCommand : CreatePersonCommand
  {
    public int? GroupId { get; set; }
  }
}
