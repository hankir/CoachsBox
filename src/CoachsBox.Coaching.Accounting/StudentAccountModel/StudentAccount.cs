using System;
using CoachsBox.Accounting;

namespace CoachsBox.Coaching.Accounting.StudentAccountModel
{
  /// <summary>
  /// Счет студента.
  /// </summary>
  public class StudentAccount : Account
  {
    public StudentAccount(int studentId)
    {
      if (studentId <= 0)
        throw new ArgumentException("Student id should be greater than zero", nameof(studentId));
      this.StudentId = studentId;
    }

    /// <summary>
    /// Получить идентификатор студента, для которого создан счет.
    /// </summary>
    public int StudentId { get; private set; }

    private StudentAccount()
    {
      // Требует Entity framework core
    }
  }
}
