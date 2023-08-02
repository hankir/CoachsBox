using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using CoachsBox.WebApp.AppFacade;
using CoachsBox.WebApp.Areas.Identity.Data;
using CoachsBox.WebApp.Pages.Facade;
using CoachsBox.WebApp.Pages.Facade.DTO;
using CoachsBox.WebApp.Resources;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace CoachsBox.WebApp.Pages.Components
{
  public class MyScheduleWidgetViewComponent : ViewComponent
  {
    private readonly IAttendanceLogRestriction attendanceLogRestriction;

    private readonly ISchedulingServiceFacade schedulingServiceFacade;

    public WeekNavigationModel WeekNavigation { get; set; }

    public IReadOnlyCollection<DayScheduleDTO> DayTrainings { get; set; }

    public string EmptyListMessage { get; set; }

    public bool CanChangeAttendanceLog { get; set; }

    public IStringLocalizer<SharedResource> Localizer { get; private set; }

    public MyScheduleWidgetViewComponent(
      IAttendanceLogRestriction attendanceLogRestriction,
      ISchedulingServiceFacade schedulingServiceFacade,
      IStringLocalizer<SharedResource> localizer)
    {
      this.attendanceLogRestriction = attendanceLogRestriction;
      this.schedulingServiceFacade = schedulingServiceFacade;
      this.Localizer = localizer;
      this.DayTrainings = new List<DayScheduleDTO>();
    }

    public async Task<IViewComponentResult> InvokeAsync(DateTime weekDate)
    {
      this.WeekNavigation = WeekNavigationModel.CreateFromNow(weekDate, this.Localizer);

      var coachId = this.UserClaimsPrincipal.GetCoachId();
      this.DayTrainings = coachId > 0 ?
        this.schedulingServiceFacade.ListScheduleOnDateByCoach(coachId, weekDate) :
        this.schedulingServiceFacade.ListScheduleOnDate(weekDate);

      if (this.DayTrainings.Count < 1)
        this.EmptyListMessage = this.UserClaimsPrincipal.GetUserNow().Date == weekDate.Date ? "Сегодня тренировок нет" : "Тренировки не запланированы";

      return this.View(this);
    }
  }

  public class WeekNavigationModel
  {
    private readonly List<Item> weekDays = new List<Item>();

    public Item Current { get; set; }

    public IReadOnlyCollection<Item> WeekDays => this.weekDays;

    public static WeekNavigationModel CreateFromNow(DateTime now, IStringLocalizer localizer)
    {
      var startWeekDay = now.StartOfWeek(DayOfWeek.Monday);

      var result = new WeekNavigationModel();
      var currentDate = startWeekDay;
      for (int i = 0; i < 7; i++)
      {
        var item = new Item(currentDate, localizer);
        result.weekDays.Add(item);
        if (currentDate == now.Date)
          result.Current = item;
        currentDate = currentDate.AddDays(1);
      }

      return result;
    }

    private WeekNavigationModel() { }

    public class Item
    {
      public Item(DateTime date, IStringLocalizer localizer)
      {
        var names = CultureInfo.InvariantCulture.DateTimeFormat.AbbreviatedDayNames;
        this.Date = date;
        this.AbbreviatedDayName = localizer[names[(int)date.DayOfWeek]];
      }

      public string AbbreviatedDayName { get; }

      public DateTime Date { get; }
    }
  }
}
