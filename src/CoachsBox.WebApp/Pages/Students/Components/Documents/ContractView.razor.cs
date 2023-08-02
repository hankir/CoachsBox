using CoachsBox.Coaching.Application.Commands;
using CoachsBox.WebApp.AppFacade.Documents.DTO;
using MediatR;

namespace CoachsBox.WebApp.Pages.Students.Components.Documents
{
  public partial class ContractView : StudentDocumentComponentBase<ContractDTO>
  {
    protected override IBaseRequest CreateAddCommand()
    {
      return new AddContractCommand(this.StudentId, this.EditModel.Date.Value, this.EditModel.Number);
    }

    protected override IBaseRequest CreateUpdateCommand()
    {
      return new UpdateContractCommand(this.EditModel.Id.Value, this.EditModel.Date.Value, this.EditModel.Number);
    }

    protected override ContractDTO CreateEditModel()
    {
      return this.IsTransient() ? new ContractDTO() : new ContractDTO()
      {
        Id = this.Document.Id,
        Date = this.Document.Date,
        Number = this.Document.Number
      };
    }
  }
}
