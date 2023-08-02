using CoachsBox.Coaching.PersonModel;
using CoachsBox.Coaching.StudentModel;
using CoachsBox.Core.Primitives;

namespace CoachsBox.Coaching.Application
{
  public interface IAdministrativeService
  {
    void AddStudentRelative(int studentId, PersonName relativeName, Date birthday, PersonalInformation relativeInfo, Relationship relationship);

    void UpdateStudentRelative(int studentId, int personid, PersonName relativeName, Date birthday, PersonalInformation relativeInfo, Relationship relationship);
  }
}
