using CoachsBox.Infrastructure;

namespace CoachsBox.Coaching.Infrastructure
{
  public class CoachsBoxUnitOfWork : EfUnitOfWork<CoachsBoxContext>
  {
    public CoachsBoxUnitOfWork(CoachsBoxContext context)
      : base(context)
    {
    }
  }
}
