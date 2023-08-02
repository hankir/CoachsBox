using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CoachsBox.WebApp.Pages.Facade.DTO
{
  public class RepeatableTrainingTimeDTO
  {
    [Display(Name = "День недели")]
    public DayOfWeek DayOfWeek { get; set; }

    [Display(Name = "Начало")]
    public TimeSpan Start { get; set; }

    [Display(Name = "Конец")]
    public TimeSpan End { get; set; }
  }
}
