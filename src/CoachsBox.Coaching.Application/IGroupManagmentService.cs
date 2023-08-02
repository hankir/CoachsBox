using System.Collections.Generic;
using System.Threading.Tasks;
using CoachsBox.Coaching.GroupModel;

namespace CoachsBox.Coaching.Application
{
  /// <summary>
  /// Сервис управления группами.
  /// </summary>
  public interface IGroupManagmentService
  {
    /// <summary>
    /// Создать новую группу.
    /// </summary>
    /// <param name="branchId">Идентификатор филиала.</param>
    /// <param name="name">Название группы.</param>
    /// <param name="sport">Вид спорта.</param>
    /// <param name="minAge">Минимальный возраст группы.</param>
    /// <param name="maxAge">Максимальный возраст группы.</param>
    /// <returns>Идентификатор группы.</returns>
    int CreateNewGroup(int branchId, string name, Sport sport, int minAge, int maxAge);

    /// <summary>
    /// Получить список групп для тренера.
    /// </summary>
    /// <param name="coachId">Идентификатор тренера.</param>
    /// <returns>Список групп тренера.</returns>
    IReadOnlyCollection<Group> ListCoachGroups(int coachId);
  }
}
