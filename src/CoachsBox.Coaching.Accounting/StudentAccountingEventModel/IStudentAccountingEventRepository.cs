using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoachsBox.Core.Interfaces;
using CoachsBox.Core.Primitives;

namespace CoachsBox.Coaching.Accounting.StudentAccountingEventModel
{
  public interface IStudentAccountingEventRepository : IAsyncRepository<StudentAccountingEvent>
  {
    Task<IReadOnlyList<IGrouping<int, StudentAccountingEvent>>> ListGroupsEvents(DateTime from, DateTime to);

    Task<IReadOnlyList<StudentAccountingEvent>> ListEventsByGroupId(int groupId);

    Task<IReadOnlyDictionary<int, Money>> GetGroupsDebtsOn(DateTime to);
  }
}
