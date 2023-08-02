using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CoachsBox.Core.Primitives;

namespace CoachsBox.Coaching.Accounting.StudentAccountModel
{
  public interface IStudentAccountQueryService
  {
    Task<IReadOnlyDictionary<int, Money>> GetStudentsBalancesByGroup(int groupId, DateTime accrualsEndPeriod, DateTime paymentsEndPeriod);

    Task<IReadOnlyDictionary<int, Money>> GetStudentsBalances(IEnumerable<int> studentIds);

    Task<IReadOnlyList<int>> GetStudentsIdsWithDebts();

    Task<IReadOnlyList<int>> GetStudentsIdsWithDebtsByCoach(int coachId);
  }
}
