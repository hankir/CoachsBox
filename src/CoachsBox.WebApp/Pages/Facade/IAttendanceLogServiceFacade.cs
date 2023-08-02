using System;
using System.Threading.Tasks;
using CoachsBox.WebApp.AppFacade.Attendance.DTO;
using CoachsBox.WebApp.Pages.Facade.DTO;

namespace CoachsBox.WebApp.Pages.Facade
{
  public interface IAttendanceLogServiceFacade
  {
    /// <summary>
    /// Посмотреть журнал посещаемости группы.
    /// </summary>
    /// <param name="groupId">Идентификатор группы.</param>
    /// <param name="month">Месяц, за который нужен журнал.</param>
    /// <param name="year">Год, за который нужен журнал.</param>
    /// <returns>Журнал посещаемости группы.</returns>
    Task<AttendanceLogViewDTO> ViewAttendanceLog(int groupId, int month, int year);

    /// <summary>
    /// Посмотреть журнал посещаемости группы.
    /// </summary>
    /// <param name="groupId">Идентификатор группы.</param>
    /// <param name="month">Месяц, за который нужен журнал.</param>
    /// <param name="year">Год, за который нужен журнал.</param>
    /// <param name="showExcludedStudents">Показывать студентов, которые уже не в группе.</param>
    /// <returns>Журнал посещаемости группы.</returns>
    Task<AttendanceLogViewDTO> ViewAttendanceLog(int groupId, int month, int year, bool showExcludedStudents);

    /// <summary>
    /// Посмотреть посещаемость тренировки группы.
    /// </summary>
    /// <param name="groupId">Идентификатор группы.</param>
    /// <param name="trainingDate">Дата тренировки.</param>
    /// <param name="trainingStartTime">Начало тренировки.</param>
    /// <param name="trainingEndTime">Конец тренировки.</param>
    /// <param name="showMarkedStudents">Получить всех отмеченых студентов, в том числе исключенных.</param>
    /// <returns>Журнал посещаемости в разрезе тренировки.</returns>
    Task<AttendanceLogDTO> ViewTrainingAttendance(int groupId, DateTime trainingDate, TimeSpan trainingStartTime, TimeSpan trainingEndTime, bool showMarkedStudents);

    Task MarkAllStudents(int groupId, MarkAttendanceCommand markCommand);

    Task ClearMarkStudent(int studentId, MarkAttendanceCommand markCommand);

    Task MarkStudent(int studentId, MarkAttendanceCommand markCommand);

    Task ClearAllMark(int groupId, MarkAttendanceCommand markCommand);
  }
}
