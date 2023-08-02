using MediatR;

namespace CoachsBox.Coaching.CoachModel
{
  public class CreatedCoachEvent : INotification
  {
    public CreatedCoachEvent(Coach coach)
    {
      this.Coach = coach;
    }

    public Coach Coach { get; }
  }
}
