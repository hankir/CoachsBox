using MediatR;

namespace CoachsBox.WebApp.Areas.Identity.Commands
{
  public class CreateCoachUserCommand : IRequest<CreateCoachUserResult>
  {
    public CreateCoachUserCommand(string email, string phoneNumber, int personId)
    {
      this.Email = email;
      this.PhoneNumber = phoneNumber;
      this.PersonId = personId;
    }

    public string Email { get; }

    public string PhoneNumber { get; }

    public int PersonId { get; }
  }
}
