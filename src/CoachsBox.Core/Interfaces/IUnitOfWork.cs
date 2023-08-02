using System;
using System.Threading.Tasks;

namespace CoachsBox.Core.Interfaces
{
  public interface IUnitOfWork
  {
    void Save();

    Task SaveAsync();
  }
}
