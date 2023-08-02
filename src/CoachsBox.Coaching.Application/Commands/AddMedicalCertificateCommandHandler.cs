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
  public class AddMedicalCertificateCommandHandler : IRequestHandler<AddMedicalCertificateCommand, bool>
  {
    private readonly IStudentDocumentRepository studentDocumentRepository;

    public AddMedicalCertificateCommandHandler(IStudentDocumentRepository studentDocumentRepository)
    {
      this.studentDocumentRepository = studentDocumentRepository;
    }

    public async Task<bool> Handle(AddMedicalCertificateCommand request, CancellationToken cancellationToken)
    {
      if (request == null)
        return false;

      var studentId = request.StudentId;
      var date = Date.Create(request.Date.Date);
      var endDate = Date.Create(request.Date.Date.AddYears(1));

      var medicalCertificate = request.AllowCompetition ?
        MedicalCertificate.CreateWithAllowCompetition(studentId, date, endDate) :
        MedicalCertificate.CreateWithAllowTraining(studentId, date, endDate);

      await this.studentDocumentRepository.AddAsync(medicalCertificate);
      await this.studentDocumentRepository.SaveAsync();
      return true;
    }
  }
}
