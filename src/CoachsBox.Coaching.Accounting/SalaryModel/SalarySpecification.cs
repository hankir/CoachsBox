using CoachsBox.Core;

namespace CoachsBox.Coaching.Accounting.SalaryModel
{
  public class SalarySpecification : BaseSpecification<Salary>
  {
    public SalarySpecification(int salaryId)
      : base(salary => salary.Id == salaryId)
    {
    }

    public SalarySpecification WithCalculations()
    {
      this.AddInclude(salary => salary.Calculations);
      return this;
    }
  }
}
