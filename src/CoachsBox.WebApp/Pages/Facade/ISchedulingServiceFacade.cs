using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoachsBox.WebApp.Pages.Facade.DTO;

namespace CoachsBox.WebApp.Pages.Facade
{
  public interface ISchedulingServiceFacade
  {
    IReadOnlyCollection<ScheduleTemplateDTO> ListScheduleTemplates();

    ScheduleDTO LoadSchedule(int scheduleId);

    int CreateSchedule(CreateScheduleCommand createCommand, ScheduleTemplateDTO scheduleTemplate);

    void UpdateSchedule(UpdateScheduleCommand updateCommand);

    IReadOnlyCollection<DayOfWeekDTO> ListDayOfWeek();

    IReadOnlyCollection<DayScheduleDTO> ListScheduleOnDateByCoach(int coachId, DateTime weekDate);

    IReadOnlyCollection<DayScheduleDTO> ListScheduleOnDate(DateTime weekDate);
  }
}
