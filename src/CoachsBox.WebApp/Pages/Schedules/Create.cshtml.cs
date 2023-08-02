using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoachsBox.Coaching.GroupModel;
using CoachsBox.WebApp.Areas.Admin.Facade;
using CoachsBox.WebApp.Areas.Admin.Facade.DTO;
using CoachsBox.WebApp.Areas.Identity.Data;
using CoachsBox.WebApp.Pages.Facade;
using CoachsBox.WebApp.Pages.Facade.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CoachsBox.WebApp.Pages.Schedules
{
  public class CreateModel : PageModel
  {
    private readonly IAdministrationServiceFacade administrativeService;
    private readonly ISchedulingServiceFacade schedulingService;
    private readonly IGroupRepository groupRepository;

    public CreateModel(
      IAdministrationServiceFacade administrativeService,
      ISchedulingServiceFacade schedulingService,
      IGroupRepository groupRepository)
    {
      this.administrativeService = administrativeService;
      this.schedulingService = schedulingService;
      this.groupRepository = groupRepository;
    }

    public string GroupName { get; set; }

    public SelectList CoachList { get; set; }

    public IReadOnlyCollection<ScheduleTemplateDTO> ScheduleTemplates { get; set; }

    [BindProperty]
    public CreateScheduleCommand CreateCommand { get; set; }

    public async Task<IActionResult> OnGetAsync(int groupId)
    {
      var group = await this.groupRepository.GetByIdAsync(groupId);
      if (group == null)
        return this.NotFound();

      var coaches = this.administrativeService.ListCoachesForBranch(group.BranchId).ToList();
      this.CoachList = new SelectList(coaches, nameof(CoachDTO.Id), nameof(CoachDTO.FullName));

      this.ScheduleTemplates = this.schedulingService.ListScheduleTemplates();

      this.GroupName = group.Name;
      var coachId = this.User.GetCoachId();
      this.CreateCommand = new CreateScheduleCommand(group.Id, group.BranchId)
      {
        CoachId = coachId
      };

      return this.Page();
    }

    public IActionResult OnPost(int templateId)
    {
      var scheduleTemplates = this.schedulingService.ListScheduleTemplates();
      var template = scheduleTemplates.Single(t => t.Id == templateId);
      var scheduleId = this.schedulingService.CreateSchedule(this.CreateCommand, template);
      return this.RedirectToPage("Edit", new { scheduleId });
    }
  }
}