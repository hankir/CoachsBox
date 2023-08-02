using System.Threading;
using System.Threading.Tasks;
using CoachsBox.Coaching.StudentDocumentModel;
using CoachsBox.Core.Primitives;
using MediatR;

namespace CoachsBox.Coaching.Application.Commands
{
  public class UpdateInsurancePolicyCommandHandler : IRequestHandler<UpdateInsurancePolicyCommand, bool>
  {
    private readonly IStudentDocumentRepository studentDocumentRepository;

    public UpdateInsurancePolicyCommandHandler(IStudentDocumentRepository studentDocumentRepository)
    {
      this.studentDocumentRepository = studentDocumentRepository;
    }

    public async Task<bool> Handle(UpdateInsurancePolicyCommand request, CancellationToken cancellationToken)
    {
      if (request == null)
        return false;

      var date = Date.Create(request.Date);
      var endDate = Date.Create(request.EndDate);

      var insurancePolicy = await this.studentDocumentRepository.GetByIdAsync(request.InsurancePolicyId) as InsurancePolicy;

      insurancePolicy.CorrectDates(date, endDate);
      insurancePolicy.CorrectNumber(request.Number);

      await this.studentDocumentRepository.UpdateAsync(insurancePolicy);
      await this.studentDocumentRepository.SaveAsync();
      return true;
    }
  }
}
