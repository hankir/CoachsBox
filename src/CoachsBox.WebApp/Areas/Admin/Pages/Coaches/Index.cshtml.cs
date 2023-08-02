using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using CoachsBox.Coaching.CoachModel;
using CoachsBox.Coaching.Infrastructure;
using CoachsBox.WebApp.Areas.Admin.Facade.DTO;
using CoachsBox.WebApp.Areas.Admin.Facade.Internal;

namespace CoachsBox.WebApp.Areas.Admin.Pages.Coaches
{
  public class IndexModel : PageModel
  {
    private readonly ICoachRepository coachRepository;

    public IndexModel(ICoachRepository context)
    {
      coachRepository = context;
    }

    public IList<CoachDTO> Coaches { get; set; }

    public async Task OnGetAsync()
    {
      var coaches = await coachRepository.ListAllAsync();
      var assembler = new CoachDTOAssembler();
      var orderedCoasches = coaches.OrderBy(c => c.Person.Name.FullName());
      this.Coaches = assembler.ToDTOList(orderedCoasches);
    }
  }
}
