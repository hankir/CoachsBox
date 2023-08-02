using System;
using System.Threading;
using System.Threading.Tasks;
using CoachsBox.Coaching.Accounting.SalaryModel;
using MediatR;

namespace CoachsBox.Coaching.Application.Accounting.Commands
{
  public class SalaryPayCommandHandler : IRequestHandler<SalaryPayCommand, bool>
  {
    private readonly ISalaryRepository salaryRepository;

    public SalaryPayCommandHandler(
      ISalaryRepository salaryRepository)
    {
      this.salaryRepository = salaryRepository;
    }

    public async Task<bool> Handle(SalaryPayCommand request, CancellationToken cancellationToken)
    {
      if (request == null)
        throw new ArgumentNullException(nameof(request));

      var salarySpecification = new SalarySpecification(request.SalaryId).WithCalculations();
      var salary = await this.salaryRepository.GetBySpecAsync(salarySpecification);

      salary.Pay();

      await this.salaryRepository.UpdateAsync(salary);
      await this.salaryRepository.SaveAsync();

      return true;
    }
  }
}
