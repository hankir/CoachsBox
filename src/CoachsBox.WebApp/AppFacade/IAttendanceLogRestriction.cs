using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoachsBox.WebApp.AppFacade
{
  public interface IAttendanceLogRestriction
  {
    bool CanChangeAttendanceLogOnDate(DateTime dateTime);

    DateTime NextCloseDate { get; }

    DateTime LastCloseDate { get; }
  }
}
