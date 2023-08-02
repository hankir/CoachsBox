using System;
using MediatR;

namespace CoachsBox.Coaching.AttendanceLogModel
{
  public class StudentMarkClearedEvent : INotification
  {
    public StudentMarkClearedEvent(int groupId, int studentId, int coachId, DateTime trainingStart, DateTime trainingEnd, bool isTrialTraining, string absence, bool haveAttendanceInMonth)
    {
      this.GroupId = groupId;
      this.StudentId = studentId;
      this.CoachId = coachId;
      this.TrainingStart = trainingStart;
      this.TrainingEnd = trainingEnd;
      this.IsTrialTraining = isTrialTraining;
      this.Absence = absence;
      this.HaveAttendanceInMonth = haveAttendanceInMonth;
    }

    public int GroupId { get; }

    public int StudentId { get; }

    public int CoachId { get; }

    public DateTime TrainingStart { get; }

    public DateTime TrainingEnd { get; }

    public bool IsTrialTraining { get; }

    public string Absence { get; }

    public bool HaveAttendanceInMonth { get; }
  }
}
