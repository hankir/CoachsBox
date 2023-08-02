using System;
using System.Collections.Generic;
using CoachsBox.Coaching.StudentModel;
using CoachsBox.Core;

namespace CoachsBox.Coaching.GroupModel
{
  /// <summary>
  /// Зачисленный студент.
  /// </summary>
  public class EnrolledStudent : ValueObject
  {
    public EnrolledStudent(int studentId, DateTime whenEnrolled)
    {
      if (studentId <= 0)
        throw new ArgumentException("Id is transient", nameof(StudentId));

      this.StudentId = studentId;
      this.WhenEnrolled = whenEnrolled;
    }

    public EnrolledStudent(int studentId, DateTime whenEnrolled, byte trialTrainingCount)
    {
      if (studentId <= 0)
        throw new ArgumentException("Id is transient", nameof(StudentId));

      this.StudentId = studentId;
      this.WhenEnrolled = whenEnrolled;
      this.TrialTrainingCount = trialTrainingCount;
    }

    public EnrolledStudent(Student student, DateTime whenEnrolled)
    {
      this.Student = student;
      this.WhenEnrolled = whenEnrolled;
    }

    public EnrolledStudent(Student student, DateTime whenEnrolled, byte trialTrainingCount)
    {
      this.Student = student;
      this.WhenEnrolled = whenEnrolled;
      this.TrialTrainingCount = trialTrainingCount;
    }

    /// <summary>
    /// Получить студента.
    /// </summary>
    public Student Student { get; private set; }

    /// <summary>
    /// Получить идентификатор студента.
    /// </summary>
    public int StudentId { get; private set; }

    /// <summary>
    /// Получить дату, когда студент зачислен.
    /// </summary>
    public DateTime WhenEnrolled { get; private set; }

    /// <summary>
    /// Получить количество пробных тренировок.
    /// </summary>
    public byte TrialTrainingCount { get; private set; }

    /// <summary>
    /// Проверить, есть ли у зачисленного студента пробные тренировки.
    /// </summary>
    /// <returns>True, если для ученика есть пробные тренировки, иначе - false.</returns>
    public bool HasTrialTrainings()
    {
      return this.TrialTrainingCount > 0;
    }

    public EnrolledStudent UseTrialTraining()
    {
      if (!this.HasTrialTrainings())
        throw new InvalidOperationException($"Student with id {this.StudentId} have not trial trainings");

      return new EnrolledStudent(this.StudentId, this.WhenEnrolled)
      {
        Student = this.Student,
        TrialTrainingCount = (byte)(this.TrialTrainingCount - 1)
      };
    }

    public EnrolledStudent ReuseTrialTraining()
    {
      return new EnrolledStudent(this.StudentId, this.WhenEnrolled)
      {
        Student = this.Student,
        TrialTrainingCount = (byte)(this.TrialTrainingCount + 1)
      };
    }

    public EnrolledStudent EnableTrialPeriod(byte trainingCount)
    {
      return new EnrolledStudent(this.StudentId, this.WhenEnrolled)
      {
        Student = this.Student,
        TrialTrainingCount = trainingCount
      };
    }

    public EnrolledStudent DisableTrialPeriod()
    {
      return new EnrolledStudent(this.StudentId, this.WhenEnrolled)
      {
        Student = this.Student,
        TrialTrainingCount = 0
      };
    }

    protected override IEnumerable<object> GetAtomicValues()
    {
      yield return this.StudentId;
      yield return this.WhenEnrolled;
      yield return this.TrialTrainingCount;
    }

    private EnrolledStudent()
    {
      // Требует Entity framework core
    }
  }
}
