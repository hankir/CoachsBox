using CoachsBox.Coaching.Application.Commands;
using CoachsBox.WebApp.AppFacade.Documents.DTO;
using MediatR;

namespace CoachsBox.WebApp.Pages.Students.Components.Documents
{
  public partial class ApplicationView : StudentDocumentComponentBase<ApplicationDTO>
  {
    protected override IBaseRequest CreateAddCommand()
    {
      return new AddApplicationCommand(this.StudentId, this.EditModel.Date.Value);
    }

    protected override ApplicationDTO CreateEditModel()
    {
      return this.IsTransient() ? new ApplicationDTO() : new ApplicationDTO()
      {
        Id = this.Document.Id,
        Date = this.Document.Date
      };
    }

    protected override IBaseRequest CreateUpdateCommand()
    {
      return new UpdateApplicationCommand(this.EditModel.Id.Value, this.EditModel.Date.Value);
    }
  }
}
