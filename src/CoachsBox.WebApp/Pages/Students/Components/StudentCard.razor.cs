using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoachsBox.WebApp.AppFacade.Students.DTO;
using Microsoft.AspNetCore.Components;

namespace CoachsBox.WebApp.Pages.Students.Components
{
  public partial class StudentCard : OwningComponentBase
  {
    [Parameter]
    public StudentDetailsDTO Student { get; set; }
  }
}
