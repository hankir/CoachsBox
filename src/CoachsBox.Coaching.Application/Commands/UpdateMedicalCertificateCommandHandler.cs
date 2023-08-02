using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using CoachsBox.Coaching.StudentDocumentModel;
using CoachsBox.Core.Primitives;
using MediatR;

namespace CoachsBox.Coaching.Application.Commands
{
  public class UpdateMedicalCertificateCommandHandler : IRequestHandler<UpdateMedicalCertificateCommand, bool>
  {
    private IStudentDocumentRepository studentDocumentRepository;

    public UpdateMedicalCertificateCommandHandler(IStudentDocumentRepository studentDocumentRepository)
    {
      this.studentDocumentRepository = studentDocumentRepository;
    }

    public async Task<bool> Handle(UpdateMedicalCertificateCommand request, CancellationToken cancellationToken)
    {
      if (request == null)
        return false;

      var date = Date.Create(request.Date.Date);
      var endDate = Date.Create(request.Date.Date.AddYears(1));

      var medicalCertificate = await this.studentDocumentRepository.GetByIdAsync(request.MedicalCertificateId) as MedicalCertificate;
      medicalCertificate.CorrectDates(date, endDate);
      medicalCertificate.CorrectAllows(request.AllowTraining, request.AllowCompetition);

      await this.studentDocumentRepository.UpdateAsync(medicalCertificate);
      await this.studentDocumentRepository.SaveAsync();
      return true;
    }
  }
}
