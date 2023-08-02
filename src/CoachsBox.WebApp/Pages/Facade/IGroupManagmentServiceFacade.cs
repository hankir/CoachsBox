using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CoachsBox.WebApp.AppFacade.Groups.DTO;
using CoachsBox.WebApp.Areas.Admin.Facade.DTO;
using CoachsBox.WebApp.Pages.Facade.DTO;

namespace CoachsBox.WebApp.Pages.Facade
{
  public interface IGroupManagmentServiceFacade
  {
    IReadOnlyCollection<GroupDTO> ListGroups();

    IReadOnlyCollection<GroupDTO> ListGroupsByCoach(int coachId);

    Task<IReadOnlyCollection<GroupDTO>> ListGroupsByStudent(int studentId);

    IReadOnlyCollection<BranchRefDTO> ListBranches();

    IReadOnlyCollection<TrainingProgramDTO> ListTrainingPrograms();

    int CreateNewGroup(CreateGroupCommand command);

    GroupDTO LoadGroup(int id);

    Task<GroupDetailsDTO> ViewGroup(int groupId, int attendanceYear, int attendanceMonth);

    Task<GroupDetailsDTO> ViewGroup(int groupId, int attendanceYear, int attendanceMonth, DateTime paymentsEndPeriod);

    void UpdateGroup(UpdateGroupCommand groupDTO);

    TariffDTO ViewGroupTariff(int groupId);
  }
}
