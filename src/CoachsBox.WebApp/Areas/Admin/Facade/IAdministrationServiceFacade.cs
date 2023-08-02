using System.Collections.Generic;
using CoachsBox.WebApp.AppFacade.Coaches.Commands;
using CoachsBox.WebApp.AppFacade.Primitives.Commands;
using CoachsBox.WebApp.AppFacade.Primitives.DTO;
using CoachsBox.WebApp.AppFacade.Students.Commands;
using CoachsBox.WebApp.AppFacade.Students.DTO;
using CoachsBox.WebApp.Areas.Admin.Facade.DTO;

namespace CoachsBox.WebApp.Areas.Admin.Facade
{
  public interface IAdministrationServiceFacade
  {
    IReadOnlyCollection<CoachDTO> ListCoachesForBranch(int branchId);

    IReadOnlyCollection<PersonDTO> ListContactPersonsForBranch();

    IReadOnlyCollection<CoachDTO> FindExistCoach(CreatePersonCommandBase command);

    IReadOnlyCollection<StudentDTO> FindExistStudent(CreatePersonCommandBase command);

    int CreateCoach(CreateCoachCommand command);

    int CreateStudent(CreateStudentCommand command);

    int CreateBranch(CreateBranchCommand command);

    int CreatePerson(CreatePersonCommandBase command);

    void UpdateCoach(InputCoachCommand command);

    void UpdateStudent(UpdateStudentCommand command);

    AddOrUpdateStudentRelativeCommand EditStudentRelative(int studentId, int personId);

    CreateBranchCommand EditBranch(int branchId);

    void UpdateBranch(int branchId, CreateBranchCommand command);

    void AddStudentRelative(AddOrUpdateStudentRelativeCommand command);

    void UpdateStudentRelative(int personId, AddOrUpdateStudentRelativeCommand command);

    StudentDetailsDTO StudentDetails(int studentId);
  }
}
