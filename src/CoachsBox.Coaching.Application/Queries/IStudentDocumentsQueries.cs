using System.Threading.Tasks;
using CoachsBox.Coaching.Accounting.StudentContractModel;
using CoachsBox.Coaching.Application.Data;
using CoachsBox.Coaching.StudentDocumentModel;

namespace CoachsBox.Coaching.Application.Queries
{
  public interface IStudentDocumentsQueries
  {
    Task<StudentDocumentsSet> GetStudentDocumentsSetAsync(int studentId);

    Task<StudentDocumentModel.Application> GetApplication(int studentId);

    Task<StudentContract> GetContract(int studentId);

    Task<MedicalCertificate> GetMedicalCertificate(int studentId);

    Task<InsurancePolicy> GetInsurancePolicy(int studentId);
  }
}
