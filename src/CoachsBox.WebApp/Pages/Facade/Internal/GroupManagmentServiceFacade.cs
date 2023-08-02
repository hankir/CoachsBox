using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoachsBox.Coaching.Accounting.AgreementRegistryEntryModel;
using CoachsBox.Coaching.Accounting.CoachingServiceAgreementModel;
using CoachsBox.Coaching.Accounting.StudentAccountingEventModel;
using CoachsBox.Coaching.Application;
using CoachsBox.Coaching.Application.Accounting.Commands;
using CoachsBox.Coaching.AttendanceLogModel;
using CoachsBox.Coaching.BranchModel;
using CoachsBox.Coaching.GroupModel;
using CoachsBox.Coaching.ScheduleModel;
using CoachsBox.Core.Interfaces;
using CoachsBox.WebApp.AppFacade.Groups.DTO;
using CoachsBox.WebApp.Areas.Admin.Facade.DTO;
using CoachsBox.WebApp.Areas.Admin.Facade.Internal;
using CoachsBox.WebApp.Pages.Facade.DTO;
using CoachsBox.WebApp.Pages.Facade.Internal.Assembler;
using MediatR;

namespace CoachsBox.WebApp.Pages.Facade.Internal
{
  public class GroupManagmentServiceFacade : IGroupManagmentServiceFacade
  {
    private readonly List<TrainingProgramDTO> trainingProgramList = new List<TrainingProgramDTO>()
    {
      new TrainingProgramDTO(1, "Возраст от 4 до 6", 4, 6),
      new TrainingProgramDTO(2, "Возраст от 6 до 9", 6, 9),
      new TrainingProgramDTO(3, "Возраст от 10 до 17", 10, 17),
      new TrainingProgramDTO(4, "Взрослые", 18, 150)
    };
    private readonly IMediator mediator;
    private readonly IUnitOfWork unitOfWork;
    private readonly IGroupManagmentService groupManagmentService;
    private readonly IGroupRepository groupRepository;
    private readonly IBranchRepository branchRepository;
    private readonly IScheduleRepository scheduleRepository;
    private readonly ICoachingServiceAgreementRepository serviceAgreementRepository;
    private readonly IAgreementRegistryEntryRepository agreementRegistryRepository;
    private readonly IStudentAccountingEventRepository coachingAccountingEventRepository;
    private readonly IAttendanceLogRepository attendanceLogRepository;

    public GroupManagmentServiceFacade(
      IMediator mediator,
      IUnitOfWork unitOfWork,
      IGroupManagmentService groupManagmentService,
      IGroupRepository groupRepository,
      IBranchRepository branchRepository,
      IScheduleRepository scheduleRepository,
      ICoachingServiceAgreementRepository serviceAgreementRepository,
      IAgreementRegistryEntryRepository agreementRegistryRepository,
      IStudentAccountingEventRepository coachingAccountingEventRepository,
      IAttendanceLogRepository attendanceLogRepository)
    {
      this.mediator = mediator;
      this.unitOfWork = unitOfWork;
      this.groupManagmentService = groupManagmentService;
      this.groupRepository = groupRepository;
      this.branchRepository = branchRepository;
      this.scheduleRepository = scheduleRepository;
      this.serviceAgreementRepository = serviceAgreementRepository;
      this.agreementRegistryRepository = agreementRegistryRepository;
      this.coachingAccountingEventRepository = coachingAccountingEventRepository;
      this.attendanceLogRepository = attendanceLogRepository;
    }

    public IReadOnlyCollection<GroupDTO> ListGroups()
    {
      var groups = this.groupRepository.ListAllAsync().Result;
      var assembler = new GroupDTOAssembler();
      return assembler.ToDTOList(groups);
    }

    public IReadOnlyCollection<BranchRefDTO> ListBranches()
    {
      var branches = this.branchRepository.ListAllAsync().Result;
      var assembler = new BranchRefDTOAssembler();
      return assembler.ToDTOList(branches);
    }

    public IReadOnlyCollection<TrainingProgramDTO> ListTrainingPrograms()
    {
      // TODO: Заполнение программ нужно вынести в администрирование.
      return this.trainingProgramList;
    }

    public int CreateNewGroup(CreateGroupCommand command)
    {
      var branchId = command.BranchId;
      var groupName = command.Name;
      var minAge = command.Program.MinAge;
      var maxAge = command.Program.MaxAge;
      var groupId = this.groupManagmentService.CreateNewGroup(branchId, groupName, Sport.Taekwondo, minAge, maxAge);

      if (command.TariffId > 0)
        this.mediator.Send(new AssignCoachingServiceAgreementCommand(command.TariffId, groupId)).Wait();

      return groupId;
    }

    public GroupDTO LoadGroup(int id)
    {
      var group = this.groupRepository.GetByIdAsync(id).Result;
      var assembler = new GroupDTOAssembler();
      var branch = this.branchRepository.GetByIdAsync(group.BranchId).Result;
      var branchAssembler = new BranchRefDTOAssembler();
      var branchRef = branchAssembler.ToDTO(branch);
      return assembler.ToDTO(group, branchRef);
    }

    public void UpdateGroup(UpdateGroupCommand command)
    {
      var groupId = command.GroupId;
      var groupName = command.Name;
      var program = this.trainingProgramList.Single(p => p.Id == command.ProgramId);
      var minAge = program.MinAge;
      var maxAge = program.MaxAge;

      var group = this.groupRepository.GetByIdAsync(groupId).Result;
      group.Rename(groupName);
      group.ChangeTrainingProgram(new TrainingProgramSpecification(minAge, maxAge));
      this.groupRepository.UpdateAsync(group).Wait();

      var agreementByGroupSpecification = new AgreementRegistryEntryByGroupSpecification(groupId);
      var groupAgreement = this.agreementRegistryRepository.GetBySpecAsync(agreementByGroupSpecification).Result;
      if (groupAgreement != null)
      {
        if (command.TariffId != groupAgreement.AgreementId)
        {
          var newAgreement = this.serviceAgreementRepository.GetByIdAsync(command.TariffId).Result;
          if (newAgreement != null)
            groupAgreement.ChangeAgreement(newAgreement);
        }
      }
      else if (command.TariffId > 0)
        this.mediator.Send(new AssignCoachingServiceAgreementCommand(command.TariffId, groupId)).Wait();

      this.unitOfWork.Save();
    }

    public async Task<GroupDetailsDTO> ViewGroup(int groupId, int attendanceYear, int attendanceMonth)
    {
      var paymentsStartPeriod = new DateTime(attendanceYear, attendanceMonth, 1);
      var paymentsEndPeriod = paymentsStartPeriod.AddMonths(1);
      return await this.ViewGroup(groupId, paymentsStartPeriod, paymentsEndPeriod);
    }

    public async Task<GroupDetailsDTO> ViewGroup(int groupId, int attendanceYear, int attendanceMonth, DateTime paymentsEndPeriod)
    {
      var paymentsStartPeriod = new DateTime(attendanceYear, attendanceMonth, 1);
      return await this.ViewGroup(groupId, paymentsStartPeriod, paymentsEndPeriod);
    }

    public async Task<GroupDetailsDTO> ViewGroup(int groupId, DateTime paymentsStartPeriod, DateTime paymentsEndPeriod)
    {
      var groupWithStudentsSpecification = new GroupByIdSpecification(groupId).WithStudents();
      var group = await this.groupRepository.GetBySpecAsync(groupWithStudentsSpecification.AsReadOnly());
      if (group == null)
      {
        // TODO: Вернуть NotFound
        return null;
      }

      var branchId = group.BranchId;
      var scheduleSpecification = new ScheduleSpecification(groupId, branchId).WithCoach();
      var schedule = await this.scheduleRepository.GetBySpecAsync(scheduleSpecification.AsReadOnly());

      var groupAccountingEvents = await this.coachingAccountingEventRepository.ListEventsByGroupId(groupId);
      var groupBalanceReports = new GroupBalanceReports(groupAccountingEvents);
      var groupPaymentsByPeriod = groupBalanceReports.CalculatePaymentsByPeriod(paymentsStartPeriod, paymentsEndPeriod).Quantity;

      var assembler = new GroupDetailsDTOAssembler();
      var hasTrialTrainings = this.attendanceLogRepository.HasTrialTrainingMarks(groupId);
      var result = assembler.ToDTO(group, schedule, groupBalanceReports, null, hasTrialTrainings);
      result.CurrentPayments = groupPaymentsByPeriod;

      var groupTariffSpecification = new AgreementRegistryEntryByGroupSpecification(groupId);
      var groupTariff = await this.agreementRegistryRepository.GetBySpecAsync(groupTariffSpecification.AsReadOnly());
      if (groupTariff != null)
        result.Tariff = groupTariff.Agreement.Rate;

      return result;
    }

    public IReadOnlyCollection<GroupDTO> ListGroupsByCoach(int coachId)
    {
      var groups = this.groupManagmentService.ListCoachGroups(coachId);
      var assembler = new GroupDTOAssembler();
      return assembler.ToDTOList(groups);
    }

    public TariffDTO ViewGroupTariff(int groupId)
    {
      var assembler = new TariffDTOAssembler();
      var agreementByGroupSpecification = new AgreementRegistryEntryByGroupSpecification(groupId);
      var groupAgreement = this.agreementRegistryRepository.GetBySpecAsync(agreementByGroupSpecification.AsReadOnly()).Result;
      return groupAgreement != null ? assembler.ToDTO(groupAgreement.Agreement) : TariffDTO.Free;
    }

    public async Task<IReadOnlyCollection<GroupDTO>> ListGroupsByStudent(int studentId)
    {
      var groupByStudentSpec = new GroupByStudentsIdsSpecification(new int[] { studentId });
      var groups = await this.groupRepository.ListAsync(groupByStudentSpec.AsReadOnly());

      var groupIds = groups.Select(g => g.Id);
      var scheduleSpecification = new ScheduleByGroupSpecification(groupIds).WithCoach();
      var schedules = await this.scheduleRepository.ListAsync(scheduleSpecification.AsReadOnly());

      var result = new List<GroupDTO>();
      var groupAssembler = new GroupDTOAssembler();
      var coachAssembler = new CoachDTOAssembler();
      foreach (var group in groups)
      {
        foreach (var schedule in schedules.Where(g => g.GroupId == group.Id))
        {
          var coachDTO = coachAssembler.ToDTO(schedule.Coach);
          result.Add(groupAssembler.ToDTO(group, coachDTO));
        }
      }

      return result;
    }
  }
}
