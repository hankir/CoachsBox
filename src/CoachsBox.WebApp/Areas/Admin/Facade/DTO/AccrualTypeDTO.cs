using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoachsBox.WebApp.Areas.Admin.Facade.DTO
{
  public class AccrualTypeDTO
  {
    public string AccrualType { get; set; }

    public string AccrualTypeName { get; set; }

    public override string ToString()
    {
      return this.AccrualTypeName;
    }
  }
}
