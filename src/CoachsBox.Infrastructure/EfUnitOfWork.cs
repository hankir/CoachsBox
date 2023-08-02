using System;
using System.Threading.Tasks;
using CoachsBox.Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CoachsBox.Infrastructure
{
  public class EfUnitOfWork<T> : IUnitOfWork, IDisposable
    where T : DbContext
  {
    private bool disposed = false;

    private readonly T context;

    public EfUnitOfWork(T context)
    {
      this.context = context;
    }

    public void Save()
    {
      this.context.SaveChanges();
    }

    public async Task SaveAsync()
    {
      await this.context.SaveChangesAsync();
    }

    protected virtual void Dispose(bool disposing)
    {
      if (!this.disposed)
      {
        if (disposing)
        {
          this.context.Dispose();
        }
      }
      this.disposed = true;
    }

    public void Dispose()
    {
      this.Dispose(true);
      GC.SuppressFinalize(this);
    }
  }
}
