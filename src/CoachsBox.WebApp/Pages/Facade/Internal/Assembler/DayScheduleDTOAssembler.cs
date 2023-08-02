using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoachsBox.Coaching.GroupModel;
using CoachsBox.Coaching.ScheduleModel;
using CoachsBox.WebApp.Pages.Facade.DTO;

namespace CoachsBox.WebApp.Pages.Facade.Internal.Assembler
{
  public class DayScheduleDTOAssembler
  {
    public DayScheduleDTO ToDto(Group group, TrainingTime trainingTime)
    {
      var startTime = trainingTime.Start;
      var endTime = trainingTime.End;
      return new DayScheduleDTO(
        groupId: group.Id,
        groupName: group.Name,
        studentsCount: group.EnrolledStudents.Count,
        startTraining: new TimeSpan(startTime.Hours, startTime.Minutes, 0),
        endTraining: new TimeSpan(endTime.Hours, endTime.Minutes, 0));
    }
  }
}
