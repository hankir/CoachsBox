using System;
using System.Collections.Generic;
using System.Text;

namespace CoachsBox.Coaching.Application.Data
{
  public sealed class GroupBalanceInfo
  {
    public int GroupId { get; set; }

    public decimal Balance { get; set; }

    public decimal Income { get; set; }

    public decimal Debts { get; set; }
  }
}
