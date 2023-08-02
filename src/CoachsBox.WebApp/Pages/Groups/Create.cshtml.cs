using System.Collections.Generic;
using System.Linq;
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
  public class CreateModel : PageModel
  {
    private readonly IGroupManagmentServiceFacade groupManagmentService;
    private readonly ISchedulingServiceFacade schedulingService;
    private readonly IAccountingServiceFacade accountingServiceFacade;

    public CreateModel(
      IGroupManagmentServiceFacade groupManagmentService,
      ISchedulingServiceFacade schedulingService,
      IAccountingServiceFacade accountingServiceFacade)
    {
      this.groupManagmentService = groupManagmentService;
      this.schedulingService = schedulingService;
      this.accountingServiceFacade = accountingServiceFacade;
    }

    public IActionResult OnGet()
    {
      this.FillLists();
      return Page();
    }

    private void FillLists()
    {
      var branches = this.groupManagmentService.ListBranches();
      var programs = this.groupManagmentService.ListTrainingPrograms();
      var tariffs = new[] { TariffDTO.Free }.Union(this.accountingServiceFacade.ListTariffs());
      ViewData["BranchId"] = new SelectList(branches, "Id", "Name");
      ViewData["ProgramId"] = new SelectList(programs, "Id", "Name");
      ViewData["TariffId"] = new SelectList(tariffs, "AgreementId", "Description");
    }

    public IReadOnlyCollection<TrainingProgramDTO> ProgramList { get; set; }

    [BindProperty]
    public CreateGroupCommand CreateCommand { get; set; }

    [BindProperty]
    public int ProgramId { get; set; }

    [BindProperty]
    public int TariffId { get; set; }

    public IActionResult OnPost()
    {
      if (!ModelState.IsValid)
      {
        this.FillLists();
        return Page();
      }

      var program = this.groupManagmentService.ListTrainingPrograms().Single(p => p.Id == this.ProgramId);
      this.CreateCommand.Program = program;
      var groupId = this.groupManagmentService.CreateNewGroup(this.CreateCommand);

      if (this.User.IsInRole(CoachsBoxWebAppRole.Coach))
      {
        var coachId = this.User.GetCoachId();
        var createCommand = new CreateScheduleCommand(groupId, this.CreateCommand.BranchId)
        {
          CoachId = coachId
        };
        this.schedulingService.CreateSchedule(createCommand, ScheduleTemplateDTO.CreateEmpty());
      }

      return this.RedirectToPage("./Edit", new { groupId });
    }
  }
}