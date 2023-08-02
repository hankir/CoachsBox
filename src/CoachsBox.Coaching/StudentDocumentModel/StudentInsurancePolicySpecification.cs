using CoachsBox.Core;

namespace CoachsBox.Coaching.StudentDocumentModel
{
  public class StudentInsurancePolicySpecification : BaseSpecification<InsurancePolicy>
  {
    public StudentInsurancePolicySpecification(int studentId)
      : base(application => application.StudentId == studentId)
    {
    }

    /// <summary>
    /// Получить документы студента с получением ссылки на студента.
    /// </summary>
    /// <returns>Этот экземпляр спецификации на документы студента.</returns>
    public StudentInsurancePolicySpecification WithStudent()
    {
      this.Includes.Add(application => application.Student);
      return this;
    }
  }
}
