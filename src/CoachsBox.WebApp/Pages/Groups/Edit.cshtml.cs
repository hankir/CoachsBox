using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using CoachsBox.Coaching.BranchModel;
using CoachsBox.Coaching.CoachModel;
using CoachsBox.Coaching.ScheduleModel;
using CoachsBox.Core;
using CoachsBox.WebApp.Areas.Admin.Facade;
using CoachsBox.WebApp.Areas.Admin.Facade.DTO;
using CoachsBox.WebApp.Areas.Identity.Data;
using CoachsBox.WebApp.Pages.Facade;
using CoachsBox.WebApp.Pages.Facade.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CoachsBox.WebApp.Pages.Groups
{
  [Authorize(Roles = CoachsBoxWebAppRole.Administrator)]
  public class EditModel : PageModel
  {
    private readonly IGroupManagmentServiceFacade groupManagmentService;
    private readonly IScheduleRepository scheduleRepository;
    private readonly ICoachRepository coachRepository;
    private readonly IBranchRepository branchRepository;
    private readonly IAccountingServiceFacade accountingServiceFacade;

    public EditModel(
      IGroupManagmentServiceFacade groupManagmentService,
      IScheduleRepository scheduleRepository,
      ICoachRepository coachRepository,
      IBranchRepository branchRepository,
      IAccountingServiceFacade accountingServiceFacade)
    {
      this.groupManagmentService = groupManagmentService;
      this.scheduleRepository = scheduleRepository;
      this.coachRepository = coachRepository;
      this.branchRepository = branchRepository;
      this.accountingServiceFacade = accountingServiceFacade;
    }

    [BindProperty]
    public UpdateGroupCommand UpdateCommand { get; set; }

    public int ScheduleId { get; set; }

    [Display(Name = "Тренер")]
    public string CoachName { get; set; }

    public GroupDTO Group { get; set; }

    public int CoachId { get; set; }

    public string BrancheName { get; set; }

    public SelectList ProgramList { get; set; }

    public SelectList TariffList { get; set; }

    public async Task<IActionResult> OnGetAsync(int groupId)
    {
      var group = this.groupManagmentService.LoadGroup(groupId);
      if (group == null)
        return this.NotFound();

      this.Group = group;

      var programs = this.groupManagmentService.ListTrainingPrograms();
      var selectedProgram = programs.Where(p => p.MinAge == group.MinAge && p.MaxAge == group.MaxAge).Single();
      this.ProgramList = new SelectList(programs, "Id", "Name");

      var tariffs = new[] { TariffDTO.Free }.Union(this.accountingServiceFacade.ListTariffs());
      var selectedTariff = this.groupManagmentService.ViewGroupTariff(group.Id);
      this.TariffList = new SelectList(tariffs, "AgreementId", "Description");

      var branchId = group.Branch.Id;
      var scheduleSpecification = new ScheduleSpecification(groupId, branchId);
      var schedule = await this.scheduleRepository.GetBySpecAsync(scheduleSpecification);
      if (schedule != null)
      {
        this.ScheduleId = schedule.Id;
        var coach = await this.coachRepository.GetByIdAsync(schedule.CoachId);
        this.CoachName = coach.Person.Name.FullName();
        this.CoachId = coach.Id;
      }

      this.UpdateCommand = new UpdateGroupCommand(group.Id, group.Name, selectedProgram.Id, selectedTariff.AgreementId);
      return Page();
    }

    public IActionResult OnPost()
    {
      if (!ModelState.IsValid)
        return Page();

      this.groupManagmentService.UpdateGroup(this.UpdateCommand);
      return RedirectToPage("./Index");
    }
  }
}
