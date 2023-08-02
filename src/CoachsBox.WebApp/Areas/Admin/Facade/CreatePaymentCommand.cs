using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoachsBox.WebApp.Areas.Admin.Facade
{
  public class CreatePaymentCommand
  {
    public int StudentId { get; set; }

    public string StudentName { get; set; }

    public int GroupId { get; set; }

    public string GroupName { get; set; }

    public DateTime WhenOccured { get; set; }

    public decimal Amount { get; set; }
  }
}
