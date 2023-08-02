using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoachsBox.Coaching.GroupModel;
using CoachsBox.Coaching.ScheduleModel;
using CoachsBox.WebApp.Areas.Admin.Facade;
using CoachsBox.WebApp.Areas.Admin.Facade.DTO;
using CoachsBox.WebApp.Pages.Facade;
using CoachsBox.WebApp.Pages.Facade.DTO;
using CoachsBox.WebApp.Resources;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Localization;

namespace CoachsBox.WebApp.Pages.Schedules
{
  public class EditModel : PageModel
  {
    private readonly IAdministrationServiceFacade administrativeService;
    private readonly ISchedulingServiceFacade schedulingService;
    private readonly IGroupRepository groupRepository;
    private readonly IStringLocalizer localizer;

    public EditModel(
      IAdministrationServiceFacade administrativeService,
      ISchedulingServiceFacade schedulingService,
      IGroupRepository groupRepository,
      IStringLocalizer<SharedResource> localizer)
    {
      this.administrativeService = administrativeService;
      this.schedulingService = schedulingService;
      this.groupRepository = groupRepository;
      this.localizer = localizer;
    }

    public ScheduleDTO Schedule { get; set; }

    public string GroupName { get; set; }

    public string CoachFullName { get; set; }

    public SelectList DayOfWeekList { get; set; }

    public SelectList CoachList { get; private set; }

    public IReadOnlyCollection<ScheduleTemplateDTO> ScheduleTemplates { get; set; }

    [BindProperty]
    public UpdateScheduleCommand UpdateCommand { get; set; }

    public async Task<IActionResult> OnGetAsync(int scheduleId)
    {
      var schedule = this.schedulingService.LoadSchedule(scheduleId);
      this.Schedule = schedule;

      var groupId = schedule.GroupId;
      var coachId = schedule.CoachId;

      await this.FillProperties(schedule);

      this.UpdateCommand = new UpdateScheduleCommand(scheduleId)
      {
        CoachId = coachId,
        GroupId = groupId
      };
      this.UpdateCommand.Schedule.AddRange(schedule.Trainings);

      if (!this.UpdateCommand.Schedule.Any())
      {
        this.ScheduleTemplates = this.schedulingService.ListScheduleTemplates();
      }

      return this.Page();
    }

    private async Task FillProperties(ScheduleDTO schedule)
    {
      var groupId = schedule.GroupId;
      var coachId = schedule.CoachId;

      var group = await this.groupRepository.GetByIdAsync(groupId);
      this.GroupName = group.Name;

      var coaches = this.administrativeService.ListCoachesForBranch(group.BranchId).ToList();
      this.CoachList = new SelectList(coaches, nameof(CoachDTO.Id), nameof(CoachDTO.FullName));

      var days = this.schedulingService.ListDayOfWeek();
      this.DayOfWeekList = new SelectList(days, nameof(DayOfWeekDTO.Id), nameof(DayOfWeekDTO.Name));

      // Даже на случай когда несколько тренеров, не будем падать по SingleOrDefault, возьмем первого.
      var scheduleCoach = coaches.FirstOrDefault(c => c.Id == schedule.CoachId);
      if (scheduleCoach != null)
        this.CoachFullName = scheduleCoach.FullName;
    }

    public async Task<IActionResult> OnPost()
    {
      if (!this.ModelState.IsValid)
        return this.Page();

      try
      {
        this.schedulingService.UpdateSchedule(this.UpdateCommand);
      }
      catch (ArgumentException aex)
      {
        this.ModelState.AddModelError(aex.ParamName, this.localizer[aex.Message]);

        var schedule = this.schedulingService.LoadSchedule(this.UpdateCommand.ScheduleId);
        this.Schedule = schedule;

        await this.FillProperties(schedule);

        return this.Page();
      }

      return this.RedirectToPage("/Groups/Details", new { groupId = this.UpdateCommand.GroupId });
    }

    public IActionResult OnPostDeleteTraining(int trainingIndex)
    {
      if (!this.ModelState.IsValid)
        return this.Page();

      this.UpdateCommand.Schedule.RemoveAt(trainingIndex);
      this.schedulingService.UpdateSchedule(this.UpdateCommand);
      return this.RedirectToPage("Edit", new { scheduleId = this.UpdateCommand.ScheduleId });
    }

    public IActionResult OnPostAddTraining()
    {
      if (!this.ModelState.IsValid)
        return this.Page();

      // TODO: Прикрутить более умное добавление, чтобы не добавлять одно и тоже и т.п.
      // Может быть вычислить алгоритм - понедельник, среда, а новый день будет пятница и т.п.
      this.UpdateCommand.Schedule.Add(new RepeatableTrainingTimeDTO()
      {
        DayOfWeek = DayOfWeek.Monday,
        Start = TimeSpan.Parse("18:00"),
        End = TimeSpan.Parse("19:00"),
      });
      this.schedulingService.UpdateSchedule(this.UpdateCommand);
      return this.RedirectToPage("Edit", new { scheduleId = this.UpdateCommand.ScheduleId });
    }

    public IActionResult OnPostAddTrainingByTemplate(int templateId)
    {
      var scheduleTemplates = this.schedulingService.ListScheduleTemplates();
      var template = scheduleTemplates.Single(t => t.Id == templateId);
      foreach (var trainingTimeDTO in template.CreateTrainings())
        this.UpdateCommand.Schedule.Add(trainingTimeDTO);
      this.schedulingService.UpdateSchedule(this.UpdateCommand);

      return this.RedirectToPage("Edit", new { scheduleId = this.UpdateCommand.ScheduleId });
    }
  }
}