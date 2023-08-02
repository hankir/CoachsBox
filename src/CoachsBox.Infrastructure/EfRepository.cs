using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoachsBox.Core;
using CoachsBox.Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CoachsBox.Infrastructure
{
  /// <summary>
  /// "There's some repetition here - couldn't we have some the sync methods call the async?"
  /// https://blogs.msdn.microsoft.com/pfxteam/2012/04/13/should-i-expose-synchronous-wrappers-for-asynchronous-methods/
  /// </summary>
  /// <typeparam name="T"></typeparam>
  public class EfRepository<T, TContext> : IAsyncRepository<T>
    where T : BaseEntity
    where TContext : DbContext
  {
    protected readonly TContext context;

    public EfRepository(TContext dbContext)
    {
      context = dbContext;
    }

    public async Task<T> GetBySpecAsync(ISpecification<T> spec)
    {
      var entityList = await this.ListAsync(spec);
      return entityList.SingleOrDefault();
    }

    public async Task<TDescendant> GetBySpecAsync<TDescendant>(ISpecification<TDescendant> spec) where TDescendant : T
    {
      var entityList = await this.ListAsync(spec);
      return entityList.SingleOrDefault();
    }

    public virtual async Task<T> GetByIdAsync(int id)
    {
      return await context.Set<T>().FindAsync(id);
    }

    public virtual async Task<IReadOnlyList<T>> ListAllAsync()
    {
      return await context.Set<T>().ToListAsync();
    }

    public async Task<IReadOnlyList<T>> ListAsync(ISpecification<T> spec)
    {
      return await ApplySpecification(spec).ToListAsync();
    }

    public async Task<IReadOnlyList<TDescendant>> ListAsync<TDescendant>(ISpecification<TDescendant> spec) where TDescendant : T
    {
      return await ApplySpecification(spec).ToListAsync();
    }

    public async Task<T> AddAsync(T entity)
    {
      await context.Set<T>().AddAsync(entity);
      // TODO: Убрать после реализации UnitOfWork, а конкретно после реализации Hi/Lo алгоритма.
      // await context.SaveChangesAsync();
      return entity;
    }

    public virtual async Task UpdateAsync(T entity)
    {
      context.Set<T>().Update(entity);
      // TODO: Убрать после реализации UnitOfWork, а конкретно после реализации Hi/Lo алгоритма.
      // await context.SaveChangesAsync();
      await Task.CompletedTask;
    }

    public async Task DeleteAsync(T entity)
    {
      context.Set<T>().Remove(entity);
      // TODO: Убрать после реализации UnitOfWork, а конкретно после реализации Hi/Lo алгоритма.
      // await context.SaveChangesAsync();
      await Task.CompletedTask;
    }

    public async Task SaveAsync()
    {
      await this.context.SaveChangesAsync();
    }

    public async Task<int> CountAsync(ISpecification<T> spec)
    {
      return await ApplySpecification(spec).CountAsync();
    }

    public async Task<int> CountAsync<TDescendant>(ISpecification<TDescendant> spec) where TDescendant : BaseEntity
    {
      return await ApplySpecification(spec).CountAsync();
    }

    private IQueryable<T> ApplySpecification(ISpecification<T> spec)
    {
      var set = spec.IsReadOnly ? context.Set<T>().AsNoTracking() : context.Set<T>();
      return SpecificationEvaluator<T>.GetQuery(set.AsQueryable(), spec);
    }

    private IQueryable<TDescendant> ApplySpecification<TDescendant>(ISpecification<TDescendant> spec) where TDescendant : BaseEntity
    {
      var set = spec.IsReadOnly ? context.Set<TDescendant>().AsNoTracking() : context.Set<TDescendant>();
      return SpecificationEvaluator<TDescendant>.GetQuery(set.AsQueryable(), spec);
    }
  }
}
