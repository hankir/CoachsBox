using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoachsBox.Coaching.Accounting.StudentAccountingEventModel;
using CoachsBox.Coaching.Application.Data;
using CoachsBox.Core.Primitives;

namespace CoachsBox.Coaching.Application.Impl
{
  public class AccountingService : IAccountingService
  {
    private readonly IStudentAccountingEventRepository coachingAccountingEventRepository;

    public AccountingService(
      IStudentAccountingEventRepository coachingAccountingEventRepository)
    {
      this.coachingAccountingEventRepository = coachingAccountingEventRepository;
    }

    public async Task<IReadOnlyCollection<GroupBalanceInfo>> ListGroupsBalance(DateTime from, DateTime to)
    {
      var eventsByGroup = await this.coachingAccountingEventRepository.ListGroupsEvents(from, to);
      var groupDebts = await this.coachingAccountingEventRepository.GetGroupsDebtsOn(to);
      var result = new List<GroupBalanceInfo>();
      foreach (var groupEvents in eventsByGroup)
      {
        var groupBalanceReports = new GroupBalanceReports(groupEvents.ToList());
        result.Add(new GroupBalanceInfo()
        {
          GroupId = groupEvents.Key,
          Debts = groupDebts.TryGetValue(groupEvents.Key, out Money balance) ? balance.Quantity : decimal.Zero,
          Income = groupBalanceReports.CalculatePayments().Quantity
        });
      }

      return result;
    }
  }
}
