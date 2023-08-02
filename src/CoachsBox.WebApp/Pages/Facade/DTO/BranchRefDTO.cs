using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoachsBox.WebApp.Pages.Facade.DTO
{
  public class BranchRefDTO
  {
    public BranchRefDTO(int id, string name)
    {
      this.Id = id;
      this.Name = name;
    }

    public int Id { get; }
    public string Name { get; }
  }
}
