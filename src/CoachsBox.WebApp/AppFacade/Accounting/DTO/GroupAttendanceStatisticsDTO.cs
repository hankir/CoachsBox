using CoachsBox.Coaching.AttendanceLogModel;

namespace CoachsBox.WebApp.AppFacade.Accounting.DTO
{
  public class GroupAttendanceStatisticsDTO
  {
    public int GroupId { get; set; }

    public AttendanceStatisticsInfo StatisticsInfo { get; set; }
  }
}
