using CoachsBox.Coaching.Accounting.StudentContractModel;
using CoachsBox.Coaching.StudentDocumentModel;
using CoachsBox.Core;

namespace CoachsBox.WebApp.AppFacade.Documents.DTO
{
  public class StudentDocumentDTOAssembler
  {
    public ApplicationDTO ToDTO(Application application)
    {
      if (application == null)
        return new ApplicationDTO();

      return new ApplicationDTO()
      {
        Id = application.Id,
        Date = application.Date.ToDateTime().Value
      };
    }

    public ContractDTO ToDTO(StudentContract contract)
    {
      if (contract == null)
        return new ContractDTO();

      return new ContractDTO()
      {
        Id = contract.Id,
        Date = contract.Date.ToDateTime().Value,
        Number = contract.Number
      };
    }

    public InsurancePolicyDTO ToDTO(InsurancePolicy insurancePolicy)
    {
      if (insurancePolicy == null)
        return new InsurancePolicyDTO();

      return new InsurancePolicyDTO()
      {
        Id = insurancePolicy.Id,
        Date = insurancePolicy.Date.ToDateTime().Value,
        EndDate = insurancePolicy.EndDate.ToDateTime().Value,
        Number = insurancePolicy.Number
      };
    }

    public MedicalCertificateDTO ToDTO(MedicalCertificate medicalCertificate)
    {
      if (medicalCertificate == null)
        return new MedicalCertificateDTO();

      return new MedicalCertificateDTO()
      {
        Id = medicalCertificate.Id,
        AllowCompetition = medicalCertificate.AllowCompetition,
        AllowTraining = medicalCertificate.AllowTraining,
        Date = medicalCertificate.Date.ToDateTime().Value,
        EndDate = medicalCertificate.Date.ToDateTime().Value
      };
    }
  }
}
