using System.Collections.Generic;
using System.Threading.Tasks;
using CoachsBox.Coaching.Accounting.StudentAccountModel;
using CoachsBox.Core;
using CoachsBox.Core.Interfaces;

namespace CoachsBox.IntegrationTests.Accounting
{
  public class TestStudentAccountRepository : IStudentAccountRepository
  {
    private readonly IStudentAccountRepository studentAccountRepository;

    public int AddedCount { get; private set; }

    public Task<StudentAccount> AddAsync(StudentAccount entity)
    {
      this.AddedCount++;
      return this.studentAccountRepository.AddAsync(entity);
    }

    public Task<int> CountAsync(ISpecification<StudentAccount> spec)
    {
      return this.studentAccountRepository.CountAsync(spec);
    }

    public Task<int> CountAsync<TDescendant>(ISpecification<TDescendant> spec) where TDescendant : BaseEntity
    {
      return this.studentAccountRepository.CountAsync(spec);
    }

    public Task DeleteAsync(StudentAccount entity)
    {
      return this.studentAccountRepository.DeleteAsync(entity);
    }

    public Task<StudentAccount> GetByIdAsync(int id)
    {
      return this.studentAccountRepository.GetByIdAsync(id);
    }

    public Task<StudentAccount> GetBySpecAsync(ISpecification<StudentAccount> spec)
    {
      return this.studentAccountRepository.GetBySpecAsync(spec);
    }

    public Task<IReadOnlyList<StudentAccount>> ListAllAsync()
    {
      return this.studentAccountRepository.ListAllAsync();
    }

    public Task<IReadOnlyList<StudentAccount>> ListAsync(ISpecification<StudentAccount> spec)
    {
      return this.studentAccountRepository.ListAsync(spec);
    }

    public Task<IReadOnlyList<TDescendant>> ListAsync<TDescendant>(ISpecification<TDescendant> spec) where TDescendant : StudentAccount
    {
      return this.studentAccountRepository.ListAsync(spec);
    }

    public Task SaveAsync()
    {
      return this.studentAccountRepository.SaveAsync();
    }

    public Task UpdateAsync(StudentAccount entity)
    {
      return this.studentAccountRepository.UpdateAsync(entity);
    }

    public Task<TDescendant> GetBySpecAsync<TDescendant>(ISpecification<TDescendant> spec) where TDescendant : StudentAccount
    {
      return this.studentAccountRepository.GetBySpecAsync(spec);
    }

    public TestStudentAccountRepository(IStudentAccountRepository studentAccountRepository)
    {
      this.studentAccountRepository = studentAccountRepository;
    }
  }
}
