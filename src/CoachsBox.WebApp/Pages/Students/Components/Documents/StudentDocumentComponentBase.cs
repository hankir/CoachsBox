using System.Threading.Tasks;
using CoachsBox.WebApp.AppFacade.Documents.DTO;
using MediatR;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.JSInterop;

namespace CoachsBox.WebApp.Pages.Students.Components.Documents
{
  public abstract partial class StudentDocumentComponentBase<T> : OwningComponentBase<IMediator> where T : BaseStudentDocumentDTO
  {
    [Parameter]
    public int StudentId { get; set; }

    [Parameter]
    public T Document { get; set; }

    [Parameter]
    public RenderFragment<T> DisplayTemplate { get; set; }

    [Parameter]
    public RenderFragment<T> EditBodyTemplate { get; set; }

    [Parameter]
    public EventCallback<T> Changed { get; set; }

    protected T EditModel { get; set; }

    protected EditContext EditContext { get; set; }

    protected abstract IBaseRequest CreateAddCommand();

    protected abstract IBaseRequest CreateUpdateCommand();

    protected abstract T CreateEditModel();

    protected bool IsTransient()
    {
      return this.Document?.Id == null;
    }

    protected bool IsEditModelCreated()
    {
      return this.EditContext != null && this.EditModel != null;
    }

    protected bool IsSubmitDisabled()
    {
      return !this.EditContext.IsModified() || !this.EditContext.Validate();
    }

    protected void OnEditClick()
    {
      this.InitializeEditContext();
    }

    protected async Task OnSubmitClick(MouseEventArgs e)
    {
      if (this.IsSubmitDisabled())
        return;

      if (this.Document.Id != null)
        await this.Service.Send(this.CreateUpdateCommand());
      else
        await this.Service.Send(this.CreateAddCommand());

      await this.Changed.InvokeAsync(this.Document);
    }

    private void InitializeEditContext()
    {
      this.EditModel = this.CreateEditModel();
      this.EditContext = new EditContext(this.EditModel);
    }
  }
}
