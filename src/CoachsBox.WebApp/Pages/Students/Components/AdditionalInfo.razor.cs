using System;
using System.Threading.Tasks;
using CoachsBox.Coaching.Application.Data;
using CoachsBox.Coaching.Application.Queries;
using CoachsBox.WebApp.AppFacade.Documents.DTO;
using CoachsBox.WebApp.AppFacade.Students.DTO;
using CoachsBox.WebApp.Extensions;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;

namespace CoachsBox.WebApp.Pages.Students.Components
{
  public partial class AdditionalInfo : OwningComponentBase
  {
    /// <summary>
    /// Заявление студента.
    /// </summary>
    private ApplicationDTO application;

    private ContractDTO contract;

    private MedicalCertificateDTO medicalCertificate;

    private InsurancePolicyDTO insurancePolicy;

    [Parameter]
    public StudentDetailsDTO Student { get; set; }

    protected override async Task OnInitializedAsync()
    {
      await this.LoadDocumentsSet();
    }

    private async Task LoadDocumentsSet()
    {
      string cacheKey = $"{this.Student.StudentId}-documentsSet";
      StudentDocumentsSet documentsSet;
      if (!this.ScopedServices.TryGetCachedData(cacheKey, out documentsSet))
      {
        var documentsQueries = this.ScopedServices.GetRequiredService<IStudentDocumentsQueries>();
        documentsSet = await documentsQueries.GetStudentDocumentsSetAsync(this.Student.StudentId);
        this.ScopedServices.SetCacheData(cacheKey, documentsSet, TimeSpan.FromSeconds(5));
      }

      var documentsAssembler = new StudentDocumentDTOAssembler();
      this.application = documentsAssembler.ToDTO(documentsSet.Application);
      this.contract = documentsAssembler.ToDTO(documentsSet.Contract);
      this.medicalCertificate = documentsAssembler.ToDTO(documentsSet.MedicalCertificate);
      this.insurancePolicy = documentsAssembler.ToDTO(documentsSet.InsurancePolicy);
    }

    private async Task RefreshApplication()
    {
      var documentsQueries = this.ScopedServices.GetRequiredService<IStudentDocumentsQueries>();
      var documentsAssembler = new StudentDocumentDTOAssembler();
      this.application = documentsAssembler.ToDTO(await documentsQueries.GetApplication(this.Student.StudentId));
    }

    private async Task RefreshContract()
    {
      var documentsQueries = this.ScopedServices.GetRequiredService<IStudentDocumentsQueries>();
      var documentsAssembler = new StudentDocumentDTOAssembler();
      this.contract = documentsAssembler.ToDTO(await documentsQueries.GetContract(this.Student.StudentId));
    }

    private async Task RefreshMedicalCertificate()
    {
      var documentsQueries = this.ScopedServices.GetRequiredService<IStudentDocumentsQueries>();
      var documentsAssembler = new StudentDocumentDTOAssembler();
      this.medicalCertificate = documentsAssembler.ToDTO(await documentsQueries.GetMedicalCertificate(this.Student.StudentId));
    }

    private async Task RefreshInsurancePolicy()
    {
      var documentsQueries = this.ScopedServices.GetRequiredService<IStudentDocumentsQueries>();
      var documentsAssembler = new StudentDocumentDTOAssembler();
      this.insurancePolicy = documentsAssembler.ToDTO(await documentsQueries.GetInsurancePolicy(this.Student.StudentId));
    }
  }
}
