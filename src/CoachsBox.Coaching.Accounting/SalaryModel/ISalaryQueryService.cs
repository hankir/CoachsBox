using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CoachsBox.Coaching.Accounting.SalaryAccountingEventModel;

namespace CoachsBox.Coaching.Accounting.SalaryModel
{
  public interface ISalaryQueryService
  {
    Task<IReadOnlyList<int>> GetNotPaidSalaries();

    Task<IReadOnlyList<CoachSalaryCalculation>> GetCoachSalaryCalculations(int salaryId, int coachId);

    Task<SalaryFund> GetSalaryFundAsync(DateTime to);

    Task<IReadOnlyList<SalaryDebtAccountingEvent>> GetCoachSalaryDebts();
  }
}
