using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace CoachsBox.Coaching.Infrastructure.MySqlMigrations
{
  class NoMediator : IMediator
  {
    public Task Publish<TNotification>(TNotification notification, CancellationToken cancellationToken = default(CancellationToken)) where TNotification : INotification
    {
      return Task.CompletedTask;
    }

    public Task Publish(object notification, CancellationToken cancellationToken = default)
    {
      return Task.CompletedTask;
    }

    public Task<TResponse> Send<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken = default(CancellationToken))
    {
      return Task.FromResult<TResponse>(default(TResponse));
    }

    public Task<object> Send(object request, CancellationToken cancellationToken = default)
    {
      return Task.FromResult<object>(default(object));
    }
  }
}
