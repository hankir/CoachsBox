using System.ComponentModel.DataAnnotations;

namespace CoachsBox.WebApp.AppFacade.Students.DTO
{
  public class StudentDetailsRelativeDTO
  {
    public int PersonId { get; set; }

    public string Phone { get; set; }

    public string Email { get; set; }

    public string PersonFullName { get; set; }

    [Display(Name = "Родство")]
    public string RelationshipName { get; set; }
  }
}
