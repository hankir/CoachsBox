using CoachsBox.Coaching.Application.Commands;
using CoachsBox.WebApp.AppFacade.Documents.DTO;
using MediatR;

namespace CoachsBox.WebApp.Pages.Students.Components.Documents
{
  public partial class MedicalCertificateView : StudentDocumentComponentBase<MedicalCertificateDTO>
  {
    protected override IBaseRequest CreateAddCommand()
    {
      return new AddMedicalCertificateCommand(this.StudentId, this.EditModel.Date.Value, this.EditModel.AllowTraining, this.EditModel.AllowCompetition);
    }

    protected override MedicalCertificateDTO CreateEditModel()
    {
      return this.IsTransient() ? new MedicalCertificateDTO() : new MedicalCertificateDTO()
      {
        Id = this.Document.Id,
        Date = this.Document.Date,
        EndDate = this.Document.EndDate,
        AllowTraining = this.Document.AllowTraining,
        AllowCompetition = this.Document.AllowCompetition
      };
    }

    protected override IBaseRequest CreateUpdateCommand()
    {
      return new UpdateMedicalCertificateCommand(this.EditModel.Id.Value, this.EditModel.Date.Value, this.EditModel.AllowTraining, this.EditModel.AllowCompetition);
    }
  }
}
