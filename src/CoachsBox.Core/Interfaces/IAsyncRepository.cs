using System.Collections.Generic;
using System.Threading.Tasks;

namespace CoachsBox.Core.Interfaces
{
  public interface IAsyncRepository<T> where T : BaseEntity
  {
    Task<T> GetBySpecAsync(ISpecification<T> spec);

    Task<TDescendant> GetBySpecAsync<TDescendant>(ISpecification<TDescendant> spec) where TDescendant : T;

    Task<T> GetByIdAsync(int id);

    Task<IReadOnlyList<T>> ListAllAsync();

    Task<IReadOnlyList<T>> ListAsync(ISpecification<T> spec);

    Task<IReadOnlyList<TDescendant>> ListAsync<TDescendant>(ISpecification<TDescendant> spec) where TDescendant : T;

    Task<T> AddAsync(T entity);

    Task UpdateAsync(T entity);

    Task DeleteAsync(T entity);

    Task SaveAsync();

    Task<int> CountAsync(ISpecification<T> spec);

    Task<int> CountAsync<TDescendant>(ISpecification<TDescendant> spec) where TDescendant : BaseEntity;
  }
}
