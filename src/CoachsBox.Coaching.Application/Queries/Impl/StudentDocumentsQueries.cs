using System.Threading.Tasks;
using CoachsBox.Coaching.Accounting.StudentContractModel;
using CoachsBox.Coaching.Application.Data;
using CoachsBox.Coaching.StudentDocumentModel;

namespace CoachsBox.Coaching.Application.Queries.Impl
{
  public class StudentDocumentsQueries : IStudentDocumentsQueries
  {
    private readonly IStudentDocumentRepository studentDocumentRepository;

    private readonly IStudentContractRepository studentContractRepository;

    public StudentDocumentsQueries(
      IStudentDocumentRepository studentDocumentRepository,
      IStudentContractRepository studentContractRepository)
    {
      this.studentDocumentRepository = studentDocumentRepository;
      this.studentContractRepository = studentContractRepository;
    }

    public Task<StudentDocumentModel.Application> GetApplication(int studentId)
    {
      var applicationSpecification = new StudentApplicationSpecification(studentId);
      return this.studentDocumentRepository.GetBySpecAsync(applicationSpecification.AsReadOnly());
    }

    public Task<StudentContract> GetContract(int studentId)
    {
      var contractSpecification = new StudentContractSpecification(studentId);
      return this.studentContractRepository.GetBySpecAsync(contractSpecification.AsReadOnly());
    }

    public Task<InsurancePolicy> GetInsurancePolicy(int studentId)
    {
      var insurancePolicySpecification = new StudentInsurancePolicySpecification(studentId);
      return this.studentDocumentRepository.GetBySpecAsync(insurancePolicySpecification.AsReadOnly());
    }

    public Task<MedicalCertificate> GetMedicalCertificate(int studentId)
    {
      var medicalCertificateSpecification = new StudentMedicalCertificateSpecification(studentId);
      return this.studentDocumentRepository.GetBySpecAsync(medicalCertificateSpecification.AsReadOnly());
    }

    public async Task<StudentDocumentsSet> GetStudentDocumentsSetAsync(int studentId)
    {
      var studentDocumentsSpecification = new StudentDocumentsSpecification(studentId);
      var allStudendDocuments = await this.studentDocumentRepository.ListAsync(studentDocumentsSpecification.AsReadOnly());
      StudentDocumentModel.Application application = null;
      MedicalCertificate medicalCertificate = null;
      InsurancePolicy insurancePolicy = null;
      foreach (var document in allStudendDocuments)
      {
        if (document is StudentDocumentModel.Application)
          application = (StudentDocumentModel.Application)document;

        if (document is MedicalCertificate)
          medicalCertificate = (MedicalCertificate)document;

        if (document is InsurancePolicy)
          insurancePolicy = (InsurancePolicy)document;
      }

      var contractSpecification = new StudentContractSpecification(studentId);
      var contract = await this.studentContractRepository.GetBySpecAsync(contractSpecification.AsReadOnly());

      return new StudentDocumentsSet(application, contract, medicalCertificate, insurancePolicy);
    }
  }
}
