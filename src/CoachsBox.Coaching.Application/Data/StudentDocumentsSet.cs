using CoachsBox.Coaching.Accounting.StudentContractModel;
using CoachsBox.Coaching.StudentDocumentModel;

namespace CoachsBox.Coaching.Application.Data
{
  /// <summary>
  /// Комплект документов студента.
  /// </summary>
  public class StudentDocumentsSet
  {
    public StudentDocumentsSet(
      StudentDocumentModel.Application application,
      StudentContract contract,
      MedicalCertificate medicalCertificate,
      InsurancePolicy insurancePolicy)
    {
      this.Application = application;
      this.Contract = contract;
      this.MedicalCertificate = medicalCertificate;
      this.InsurancePolicy = insurancePolicy;
    }

    /// <summary>
    /// Получить заявление студента.
    /// </summary>
    public StudentDocumentModel.Application Application { get; private set; }

    /// <summary>
    /// Получить договор студента.
    /// </summary>
    public StudentContract Contract { get; private set; }

    /// <summary>
    /// Получить договор студента.
    /// </summary>
    public MedicalCertificate MedicalCertificate { get; private set; }

    /// <summary>
    /// Получить договор студента.
    /// </summary>
    public InsurancePolicy InsurancePolicy { get; private set; }
  }
}
