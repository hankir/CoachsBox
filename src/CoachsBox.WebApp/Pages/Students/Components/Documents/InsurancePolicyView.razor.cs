using CoachsBox.Coaching.Application.Commands;
using CoachsBox.WebApp.AppFacade.Documents.DTO;
using MediatR;

namespace CoachsBox.WebApp.Pages.Students.Components.Documents
{
  public partial class InsurancePolicyView : StudentDocumentComponentBase<InsurancePolicyDTO>
  {
    protected override IBaseRequest CreateAddCommand()
    {
      return new AddInsurancePolicyCommand(this.StudentId, this.EditModel.Date.Value, this.EditModel.EndDate.Value, this.EditModel.Number);
    }

    protected override InsurancePolicyDTO CreateEditModel()
    {
      return this.IsTransient() ? new InsurancePolicyDTO() : new InsurancePolicyDTO()
      {
        Id = this.Document.Id,
        Date = this.Document.Date,
        EndDate = this.Document.EndDate,
        Number = this.Document.Number
      };
    }

    protected override IBaseRequest CreateUpdateCommand()
    {
      return new UpdateInsurancePolicyCommand(this.EditModel.Id.Value, this.EditModel.Date.Value, this.EditModel.EndDate.Value, this.EditModel.Number);
    }
  }
}
