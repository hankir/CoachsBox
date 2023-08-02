using CoachsBox.WebApp.AppFacade.Primitives.DTO;

namespace CoachsBox.WebApp.AppFacade.Students.DTO
{
  public class StudentListItemDTO
  {
    public PersonDTO Person { get; set; }

    public int StudentId { get; set; }

    public bool IsTrialTraining { get; set; }

    public decimal? Balance { get; set; }
  }
}
