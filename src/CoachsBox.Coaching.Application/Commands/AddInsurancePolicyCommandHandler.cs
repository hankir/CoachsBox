using System.Threading;
using System.Threading.Tasks;
using CoachsBox.Coaching.StudentDocumentModel;
using CoachsBox.Core.Primitives;
using MediatR;

namespace CoachsBox.Coaching.Application.Commands
{
  /// <summary>
  /// Обработчик команды добавления заявления студента.
  /// </summary>
  public class AddInsurancePolicyCommandHandler : IRequestHandler<AddInsurancePolicyCommand, bool>
  {
    private readonly IStudentDocumentRepository studentDocumentRepository;

    public AddInsurancePolicyCommandHandler(IStudentDocumentRepository studentDocumentRepository)
    {
      this.studentDocumentRepository = studentDocumentRepository;
    }

    public async Task<bool> Handle(AddInsurancePolicyCommand request, CancellationToken cancellationToken)
    {
      if (request == null)
        return false;

      var studentId = request.StudentId;
      var date = Date.Create(request.Date.Date);
      var endDate = Date.Create(request.EndDate.Date);
      var number = request.Number;

      var medicalCertificate = new InsurancePolicy(studentId, date, endDate, number);

      await this.studentDocumentRepository.AddAsync(medicalCertificate);
      await this.studentDocumentRepository.SaveAsync();
      return true;
    }
  }
}
