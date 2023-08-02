using System.Collections.Generic;
using System.Threading.Tasks;
using CoachsBox.Coaching.Application.Commands;
using CoachsBox.WebApp.Areas.Identity.Data;
using CoachsBox.WebApp.Pages.Facade;
using CoachsBox.WebApp.Pages.Facade.DTO;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CoachsBox.WebApp.Pages.Groups
{
  public class IndexModel : PageModel
  {
    private readonly IGroupManagmentServiceFacade groupManagmentServiceFacade;
    private readonly IMediator mediator;

    public IndexModel(IGroupManagmentServiceFacade groupManagmentServiceFacade, IMediator mediator)
    {
      this.groupManagmentServiceFacade = groupManagmentServiceFacade;
      this.mediator = mediator;
    }

    public IReadOnlyCollection<GroupDTO> Groups { get; set; }

    public async Task OnGetAsync()
    {
      await this.LoadGroups();
    }

    public async Task<IActionResult> OnPostDeleteAsync(int groupId)
    {
      await this.mediator.Send(new DeleteGroupCommand(groupId));
      return this.RedirectToPage("Index");
    }

    private async Task LoadGroups()
    {
      if (this.User.IsInRole(CoachsBoxWebAppRole.Coach))
        this.Groups = await Task.FromResult(this.groupManagmentServiceFacade.ListGroupsByCoach(this.User.GetCoachId()));
      else
        this.Groups = await Task.FromResult(this.groupManagmentServiceFacade.ListGroups());
    }
  }
}
