using System.Threading.Tasks;
using CoachsBox.Core.Interfaces;

namespace CoachsBox.Coaching.CoachModel
{
  public interface ICoachRepository : IAsyncRepository<Coach>
  {
    /// <summary>
    /// Получить тренера по идентификатору персоны.
    /// </summary>
    /// <param name="personId">Идентификатор персоны.</param>
    /// <returns>Тренер или null, если ничего не найдено.</returns>
    Task<Coach> GetByPersonIdAsync(int personId);
  }
}
