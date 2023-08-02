using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoachsBox.WebApp.Pages.Facade.DTO
{
  public class DayOfWeekDTO
  {
    public DayOfWeekDTO(DayOfWeek id, string name)
    {
      this.Id = id;
      this.Name = name;
    }

    public DayOfWeek Id { get; }

    public string Name { get; }
  }
}
