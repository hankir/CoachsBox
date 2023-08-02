using System;
using System.Threading.Tasks;
using CoachsBox.Core;
using CoachsBox.WebApp.AppFacade.Groups.DTO;
using CoachsBox.WebApp.Pages.Facade;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CoachsBox.WebApp.Pages.Groups
{
  public class DetailsModel : PageModel
  {
    private IGroupManagmentServiceFacade groupManagmentService;

    public DetailsModel(IGroupManagmentServiceFacade groupManagmentService)
    {
      this.groupManagmentService = groupManagmentService;
    }

    public GroupDetailsDTO Group { get; set; }

    public int? Year { get; set; }

    public int? Month { get; set; }

    public bool ShowAllStudents { get; set; }

    public DateTime? PaymentsEndPeriod { get; set; }

    public async Task<IActionResult> OnGet(int groupId, int? year, int? month, bool? showAllStudents, DateTime? paymentsEndPeriod)
    {
      if (groupId < 1)
        return this.NotFound();

      var now = Watch.Now;
      this.Group = await this.groupManagmentService.ViewGroup(groupId, year ?? now.Year, month ?? now.Month);

      if (this.Group == null)
        return this.NotFound();

      this.Year = year;
      this.Month = month;
      this.ShowAllStudents = showAllStudents != null ? showAllStudents.Value : false;
      this.PaymentsEndPeriod = paymentsEndPeriod;

      return Page();
    }
  }
}
