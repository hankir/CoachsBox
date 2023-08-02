using System;
using System.Collections.Generic;
using System.Linq;
using CoachsBox.Coaching.Application.Data;
using CoachsBox.Coaching.AttendanceLogModel;
using CoachsBox.Coaching.ScheduleModel;
using CoachsBox.Core;
using CoachsBox.Core.Primitives;

namespace CoachsBox.Coaching.Application
{
  public class AttendanceReportCardBuilder
  {
    public AttendanceReportCardBuilder(int month, Schedule schedule, AttendanceLog attendanceLog)
    {
      if (schedule == null)
        throw new ArgumentNullException(nameof(schedule));

      if (attendanceLog == null)
        throw new ArgumentNullException(nameof(attendanceLog));

      this.Month = month;
      this.Schedule = schedule;
      this.AttendanceLog = attendanceLog;
    }

    public int Month { get; }

    public Schedule Schedule { get; }

    public AttendanceLog AttendanceLog { get; }

    public IReadOnlyDictionary<TrainingTimeInfo, IReadOnlyList<AttendanceLogEntry>> Build()
    {
      var year = this.AttendanceLog.Year;
      var startDay = 1;
      var endDay = DateTime.DaysInMonth(year, this.Month);
      var trainingDays = new Dictionary<TrainingTimeInfo, List<AttendanceLogEntry>>();
      for (int i = startDay; i <= endDay; i++)
      {
        var trainingDate = Date.Create(new DateTime(year, this.Month, i));
        var scheduledTrainings = this.Schedule.TrainingList.Where(t => t.IsMatch(trainingDate));
        foreach (var scheduledTrainingTime in scheduledTrainings)
        {
          var trainingTime = CreateRegularTimeInfo(trainingDate, scheduledTrainingTime);
          var existedAttendance = new List<AttendanceLogEntry>();
          if (this.AttendanceLog != null)
          {
            existedAttendance = this.AttendanceLog.Entries
              .Where(e => e.Date.Equals(trainingDate) && e.Start.Equals(scheduledTrainingTime.Start) && e.End.Equals(scheduledTrainingTime.End))
              .ToList();
          }
          if (!trainingDays.ContainsKey(trainingTime))
            trainingDays.Add(trainingTime, existedAttendance);
          else
          {
            // TODO: Добавить логирование этой ситуации, когда одинаковое время.
          }
        }
      }

      // Получение отметок по дате тренировки, которой нет в расписании.
      // Такое может быть если расписание поменяли, а отметка осталась.
      if (this.AttendanceLog != null)
      {
        var existedEntries = trainingDays.SelectMany(t => t.Value);
        var ghostsEntries = this.AttendanceLog.Entries
          .Where(e => e.Date.Month.Number == this.Month)
          .Except(existedEntries)
          .ToList();

        foreach (var ghostEntry in ghostsEntries)
        {
          var trainingTime = CreateGhostTimeInfo(ghostEntry);
          if (trainingDays.ContainsKey(trainingTime))
            trainingDays[trainingTime].Add(ghostEntry);
          else
            trainingDays[trainingTime] = new List<AttendanceLogEntry>() { ghostEntry };
        }
      }

      return trainingDays
        .OrderBy(kv => StartToDateTime(kv.Key))
        .ToDictionary(kv => kv.Key, kv => (IReadOnlyList<AttendanceLogEntry>)kv.Value);
    }

    private static DateTime StartToDateTime(TrainingTimeInfo time)
    {
      return time.Date.Date + time.Start;
    }

    private static TrainingTimeInfo CreateRegularTimeInfo(Date date, TrainingTime time)
    {
      return new TrainingTimeInfo(date.ToDateTime().Value, time.Start.ToTimeSpan(), time.End.ToTimeSpan(), false, time.IsRegular());
    }

    private static TrainingTimeInfo CreateGhostTimeInfo(AttendanceLogEntry ghostLogEntry)
    {
      return new TrainingTimeInfo(ghostLogEntry.Date.ToDateTime().Value, ghostLogEntry.Start.ToTimeSpan(), ghostLogEntry.End.ToTimeSpan(), true, false);
    }
  }
}
