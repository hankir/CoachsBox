using System;
using System.Collections.Generic;

namespace CoachsBox.Coaching.Accounting.SalaryModel
{
  public class SalaryCalculationComparer : IComparer<SalaryCalculation>
  {
    private static readonly Lazy<SalaryCalculationComparer> instance = new Lazy<SalaryCalculationComparer>();

    public static SalaryCalculationComparer Instance => instance.Value;

    public int Compare(SalaryCalculation x, SalaryCalculation y)
    {
      var typeOfX = x.GetType();
      var typeOfY = y.GetType();

      if (typeOfX == typeOfY)
        return 0;

      if (typeOfX == typeof(CoachSalaryDebtCalculation))
        return -1;

      return 1;
    }
  }
}
