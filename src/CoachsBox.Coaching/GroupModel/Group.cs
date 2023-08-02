using System;
using System.Collections.Generic;
using System.Linq;
using CoachsBox.Coaching.BranchModel;
using CoachsBox.Coaching.StudentModel;
using CoachsBox.Core;

namespace CoachsBox.Coaching.GroupModel
{
  /// <summary>
  /// Модель группы учащихся.
  /// </summary>
  public class Group : BaseEntity
  {
    private List<EnrolledStudent> enrolledStudents = new List<EnrolledStudent>();

    /// <summary>
    /// Создать тренировочную группу.
    /// </summary>
    /// <param name="name">Имя группы.</param>
    /// <param name="sport">Спортивное направление группы.</param>
    /// <param name="schedule">Расписание группы.</param>
    public Group(int branchId, string name, Sport sport, TrainingProgramSpecification trainingProgram)
    {
      if (branchId <= 0)
        throw new ArgumentException("Id is transient", nameof(branchId));

      this.BranchId = branchId;
      this.Name = name;
      this.Sport = sport;
      this.TrainingProgramm = trainingProgram;
    }

    /// <summary>
    /// Создать тренировочную группу.
    /// </summary>
    /// <param name="name">Имя группы.</param>
    /// <param name="sport">Спортивное направление группы.</param>
    /// <param name="schedule">Расписание группы.</param>
    public Group(Branch branch, string name, Sport sport, TrainingProgramSpecification trainingProgram)
    {
      this.Branch = branch;
      this.Name = name;
      this.Sport = sport;
      this.TrainingProgramm = trainingProgram;
    }

    /// <summary>
    /// Получить идентификатор филиала.
    /// </summary>
    public int BranchId { get; private set; }

    /// <summary>
    /// Получить филиал.
    /// </summary>
    public Branch Branch { get; private set; }

    /// <summary>
    /// Получить тренировочную программу группы.
    /// </summary>
    public TrainingProgramSpecification TrainingProgramm { get; private set; }

    /// <summary>
    /// Получить имя группы.
    /// </summary>
    public string Name { get; private set; }

    /// <summary>
    /// Получить вид спорта.
    /// </summary>
    public Sport Sport { get; private set; }

    /// <summary>
    /// Получить зачисленных в группу студентов.
    /// </summary>
    public IReadOnlyCollection<EnrolledStudent> EnrolledStudents => this.enrolledStudents;

    /// <summary>
    /// Проверить включен ли студент в группу.
    /// </summary>
    /// <param name="studentId">Идентификатор студента.</param>
    /// <returns>True если студент зарегистрирован в группе, иначе false.</returns>
    public bool IsEnrolled(int studentId)
    {
      return this.enrolledStudents.Exists(e => e.StudentId == studentId);
    }

    /// <summary>
    /// Зачислить студента в группу.
    /// </summary>
    /// <param name="studentId">Идентификатор студента.</param>
    public void EnrollStudent(int studentId)
    {
      if (!this.IsEnrolled(studentId))
        this.enrolledStudents.Add(new EnrolledStudent(studentId, Watch.Now.DateTime));
    }

    /// <summary>
    /// Зачислить студента в группу.
    /// </summary>
    /// <param name="student">Студента.</param>
    public void EnrollStudent(Student student)
    {
      if (student.IsTransient() || !this.IsEnrolled(student.Id))
        this.enrolledStudents.Add(new EnrolledStudent(student, Watch.Now.DateTime));
    }

    /// <summary>
    /// Переименовать группу.
    /// </summary>
    /// <param name="newName">Новое имя группы.</param>
    public void Rename(string newName)
    {
      this.Name = newName;
    }

    /// <summary>
    /// Изменить программу тренировок группы.
    /// </summary>
    /// <param name="trainingProgramSpecification">Спецификация программы тренировок группы.</param>
    public void ChangeTrainingProgram(TrainingProgramSpecification trainingProgramSpecification)
    {
      this.TrainingProgramm = trainingProgramSpecification;
    }

    /// <summary>
    /// Исключить студента из группы.
    /// </summary>
    /// <param name="studentId">Идентификатор студента.</param>
    public void ExcludeStudent(int studentId)
    {
      if (!this.IsEnrolled(studentId))
        throw new ArgumentException($"Student ({studentId}) not enrolled in group ({this.Id})");

      var enrolledStudent = this.enrolledStudents.Single(enrolled => enrolled.StudentId == studentId);
      this.enrolledStudents.Remove(enrolledStudent);
    }

    /// <summary>
    /// Использовать пробную тренировку студента.
    /// </summary>
    /// <param name="studentId">Идентификатор студента.</param>
    public void UseTrialTraining(int studentId)
    {
      if (!this.IsEnrolled(studentId))
        throw new ArgumentException($"Student ({studentId}) not enrolled in group ({this.Id})");

      if (!this.HasTrialTrainings(studentId))
        throw new InvalidOperationException($"Student ({studentId}) have not trial training in group ({this.Id})");

      var enrolledStudentIndex = this.enrolledStudents.FindIndex(enrollment => enrollment.StudentId == studentId);
      var enrolledStudent = this.enrolledStudents[enrolledStudentIndex];
      this.enrolledStudents[enrolledStudentIndex] = enrolledStudent.UseTrialTraining();
    }

    public void ReuseTrialTraining(int studentId)
    {
      if (!this.IsEnrolled(studentId))
        throw new ArgumentException($"Student ({studentId}) not enrolled in group ({this.Id})");

      var enrolledStudentIndex = this.enrolledStudents.FindIndex(enrollment => enrollment.StudentId == studentId);
      var enrolledStudent = this.enrolledStudents[enrolledStudentIndex];
      this.enrolledStudents[enrolledStudentIndex] = enrolledStudent.ReuseTrialTraining();
    }

    public void EnableTrialPeriod(int studentId, byte trainingCount)
    {
      if (!this.IsEnrolled(studentId))
        throw new ArgumentException($"Student ({studentId}) not enrolled in group ({this.Id})");

      var enrolledStudentIndex = this.enrolledStudents.FindIndex(enrollment => enrollment.StudentId == studentId);
      var enrolledStudent = this.enrolledStudents[enrolledStudentIndex];
      this.enrolledStudents[enrolledStudentIndex] = enrolledStudent.EnableTrialPeriod(trainingCount);
    }

    public void DisableTrialPeriod(int studentId)
    {
      if (!this.IsEnrolled(studentId))
        throw new ArgumentException($"Student ({studentId}) not enrolled in group ({this.Id})");

      var enrolledStudentIndex = this.enrolledStudents.FindIndex(enrollment => enrollment.StudentId == studentId);
      var enrolledStudent = this.enrolledStudents[enrolledStudentIndex];
      this.enrolledStudents[enrolledStudentIndex] = enrolledStudent.DisableTrialPeriod();
    }

    /// <summary>
    /// Проверить, есть ли пробные тренировки у студента.
    /// </summary>
    /// <param name="studentId">Идентификатор студента.</param>
    /// <returns>True, если у студента есть пробные тренировки, иначе - false.</returns>
    public bool HasTrialTrainings(int studentId)
    {
      if (!this.IsEnrolled(studentId))
        return false;

      var enrolledStudent = this.EnrolledStudents.Single(enrolled => enrolled.StudentId == studentId);
      return enrolledStudent.HasTrialTrainings();
    }

    private Group()
    {
      // Требует Entity framework core
    }
  }
}
