using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CoachsBox.Coaching.Application.Data;

namespace CoachsBox.Coaching.Application
{
  public interface IAccountingService
  {
    Task<IReadOnlyCollection<GroupBalanceInfo>> ListGroupsBalance(DateTime from, DateTime to);
  }
}
