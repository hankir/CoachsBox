using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoachsBox.WebApp.Pages.Facade.DTO
{
  public class TrainingProgramDTO
  {
    public TrainingProgramDTO(int id, string name, int minAge, int maxAge)
    {
      this.Id = id;
      this.Name = name;
      this.MinAge = minAge;
      this.MaxAge = maxAge;
    }

    public int Id { get; }

    public string Name { get; }

    public int MinAge { get; }

    public int MaxAge { get; }
  }
}
