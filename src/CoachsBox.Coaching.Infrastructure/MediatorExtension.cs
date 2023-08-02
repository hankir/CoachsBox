using System.Linq;
using System.Threading.Tasks;
using CoachsBox.Core;
using MediatR;

namespace CoachsBox.Coaching.Infrastructure
{
  static class MediatorExtension
  {
    public static async Task DispatchDomainEventsAsync(this IMediator mediator, CoachsBoxContext context)
    {
      var domainEntities = context.ChangeTracker
          .Entries<BaseEntity>()
          .Where(x => x.Entity.DomainEvents != null && x.Entity.DomainEvents.Any());

      var domainEvents = domainEntities
          .SelectMany(x => x.Entity.DomainEvents)
          .ToList();

      domainEntities.ToList()
          .ForEach(entity => entity.Entity.ClearDomainEvents());

      foreach (var domainEvent in domainEvents)
        await mediator.Publish(domainEvent);
    }
  }
}
