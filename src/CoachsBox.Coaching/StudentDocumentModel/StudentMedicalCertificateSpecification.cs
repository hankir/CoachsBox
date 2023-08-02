using CoachsBox.Core;

namespace CoachsBox.Coaching.StudentDocumentModel
{
  public class StudentMedicalCertificateSpecification : BaseSpecification<MedicalCertificate>
  {
    public StudentMedicalCertificateSpecification(int studentId)
      : base(application => application.StudentId == studentId)
    {
    }

    /// <summary>
    /// Получить документы студента с получением ссылки на студента.
    /// </summary>
    /// <returns>Этот экземпляр спецификации на документы студента.</returns>
    public StudentMedicalCertificateSpecification WithStudent()
    {
      this.Includes.Add(application => application.Student);
      return this;
    }
  }
}
