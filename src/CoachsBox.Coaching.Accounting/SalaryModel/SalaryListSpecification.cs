using CoachsBox.Core;

namespace CoachsBox.Coaching.Accounting.SalaryModel
{
  public class SalaryListSpecification : BaseSpecification<Salary>
  {
    public SalaryListSpecification()
      : base(salary => true)
    {

    }

    public SalaryListSpecification(int year)
      : base(salary => salary.PeriodBeginning.Year == year)
    {
    }

    public SalaryListSpecification WithCalculations()
    {
      this.AddInclude(salary => salary.Calculations);
      return this;
    }
  }
}
