using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoachsBox.Coaching.Accounting.AgreementRegistryEntryModel;
using CoachsBox.Coaching.Accounting.CoachingServiceAgreementModel;
using CoachsBox.Coaching.Accounting.StudentAccountingEventModel;
using CoachsBox.Coaching.Application;
using CoachsBox.Coaching.GroupModel;
using CoachsBox.Coaching.PersonModel;
using CoachsBox.Coaching.ScheduleModel;
using CoachsBox.Coaching.StudentModel;
using CoachsBox.Core;
using CoachsBox.Core.Primitives;
using CoachsBox.WebApp.AppFacade.Accounting.DTO;
using CoachsBox.WebApp.Areas.Admin.Facade.DTO;

namespace CoachsBox.WebApp.Areas.Admin.Facade.Internal
{
  public class AccountingServiceFacade : IAccountingServiceFacade
  {
    private readonly IAccountingService accountingService;
    private readonly IScheduleRepository scheduleRepository;
    private readonly IAgreementRegistryEntryRepository agreementRegistryEntryRepository;
    private readonly ICoachingServiceAgreementRepository coachingServiceAgreementRepository;
    private readonly IStudentAccountingEventRepository eventRepository;
    private readonly IGroupRepository groupRepository;
    private readonly IStudentRepository studentRepository;

    public AccountingServiceFacade(
      ICoachingServiceAgreementRepository coachingServiceAgreementRepository,
      IStudentAccountingEventRepository eventRepository,
      IGroupRepository groupRepository,
      IStudentRepository studentRepository,
      IAccountingService accountingService,
      IScheduleRepository scheduleRepository,
      IAgreementRegistryEntryRepository agreementRegistryEntryRepository)
    {
      this.coachingServiceAgreementRepository = coachingServiceAgreementRepository;
      this.eventRepository = eventRepository;
      this.groupRepository = groupRepository;
      this.studentRepository = studentRepository;
      this.accountingService = accountingService;
      this.scheduleRepository = scheduleRepository;
      this.agreementRegistryEntryRepository = agreementRegistryEntryRepository;
    }

    public void CreateTariff(TariffDTO tariff)
    {
      var accrualType = ValueObject.GetAll<StudentAccountingEventType>().SingleOrDefault(e => e.Name == tariff.AccrualType.AccrualType);
      if (accrualType == null)
        throw new InvalidOperationException($"Accrual type not found by name {tariff.AccrualType.AccrualType}");

      var rate = Money.CreateRuble(tariff.TrainingRate);
      var newAgreement = new CoachingServiceAgreement(rate, accrualType, tariff.Description);
      this.coachingServiceAgreementRepository.AddAsync(newAgreement).Wait();
      this.coachingServiceAgreementRepository.SaveAsync().Wait();
    }

    public IReadOnlyCollection<PaymentDTO> FindPaymentsByStudentName(int count, string studentName)
    {
      if (!string.IsNullOrWhiteSpace(studentName))
      {
        var nameParts = studentName.Trim().Split(' ', 3);
        string surname = nameParts.Length >= 1 ? nameParts[0] : null;
        string name = nameParts.Length >= 2 ? nameParts[1] : null;
        string patronymic = nameParts.Length >= 3 ? nameParts[2] : null;

        var findByNameSpec = new FindStudentsByNameSpecification(new PersonName(surname, name, patronymic)).WithPerson();
        var students = this.studentRepository.ListAsync(findByNameSpec.AsReadOnly()).Result;
        var studentIds = students.Select(student => student.Id);
        return this.ListRecent(count, DateTime.MinValue, studentIds).Result;
      }

      return Enumerable.Empty<PaymentDTO>().ToList();
    }

    public async Task<IReadOnlyCollection<PaymentDTO>> ListStudentPayments(int studentId, int count, DateTime from)
    {
      if (studentId > 0)
      {
        return await this.ListRecent(count, from, new int[] { studentId });
      }

      return await Task.FromResult(new List<PaymentDTO>());
    }

    public async Task<IReadOnlyCollection<GroupBalanceDTO>> ListGroupsBalance(DateTime from, DateTime to)
    {
      var groupBalances = await this.accountingService.ListGroupsBalance(from, to);
      var groupIds = groupBalances.Select(group => group.GroupId);

      var groupsSpecification = new GroupByIdSpecification(groupIds).WithBranch();
      var groups = await this.groupRepository.ListAsync(groupsSpecification.AsReadOnly());

      var groupsAgreementSpecification = new AgreementRegistryEntryByGroupSpecification(groupIds);
      var groupsAgreements = await this.agreementRegistryEntryRepository.ListAsync(groupsAgreementSpecification.AsReadOnly());

      var schedulesSpecification = new ScheduleByGroupSpecification(groupIds);
      var schedules = await this.scheduleRepository.ListAsync(schedulesSpecification.AsReadOnly());

      var result = new List<GroupBalanceDTO>();
      foreach (var balance in groupBalances)
      {
        var group = groups.SingleOrDefault(group => group.Id == balance.GroupId);
        if (group != null)
        {
          var groupAgreement = groupsAgreements.SingleOrDefault(agreement => agreement.GroupId == balance.GroupId);
          var groupSchedule = schedules.SingleOrDefault(schedule => schedule.GroupId == group.Id);
          result.Add(new GroupBalanceDTO()
          {
            GroupId = balance.GroupId,
            Balance = balance.Balance,
            Income = balance.Income,
            Depts = balance.Debts < 0 ? balance.Debts : decimal.Zero,
            GroupName = group.Name,
            AgreementId = groupAgreement?.AgreementId ?? default,
            AgreementName = groupAgreement?.Agreement.Description,
            CoachId = groupSchedule?.CoachId ?? default,
            BranchId = group.BranchId,
            BranchCity = group.Branch.Address.City
          });
        }
      }
      return result;
    }

    public IReadOnlyCollection<PaymentDTO> ListRecent(int count)
    {
      return this.ListRecent(count, DateTime.MinValue, null).Result;
    }

    public IReadOnlyCollection<PaymentDTO> ListRecent(int count, DateTime from)
    {
      return this.ListRecent(count, from, null).Result;
    }

    private async Task<IReadOnlyCollection<PaymentDTO>> ListRecent(int count, DateTime from, IEnumerable<int> studentIds)
    {
      var to = Watch.Now.DateTime;
      var paymentDTOAssembler = new PaymentDTOAssembler();

      var paymentHistorySpecification = studentIds != null ?
        new PaymentHistorySpecification(count, from, to, studentIds) :
        new PaymentHistorySpecification(count, from, to);
      var payments = await this.eventRepository.ListAsync(paymentHistorySpecification.AsReadOnly());

      var studentIdsWithPayments = payments.Select(payment => payment.Account.StudentId).Distinct().ToList();
      var studentsByIdsSpecification = new StudentByIdSpecification(studentIdsWithPayments);
      var students = await this.studentRepository.ListAsync(studentsByIdsSpecification.AsReadOnly());

      var groupIds = payments.Select(payment => payment.GroupId).Distinct().ToList();
      var groupByIdsSpecification = new GroupByIdSpecification(groupIds);
      var groups = await this.groupRepository.ListAsync(groupByIdsSpecification.AsReadOnly());

      var paymentDTOList = new List<PaymentDTO>();
      foreach (var payment in payments)
      {
        var group = groups.Single(g => g.Id == payment.GroupId);
        var student = students.Single(s => s.Id == payment.Account.StudentId);
        paymentDTOList.Add(paymentDTOAssembler.ToDTO(payment, group, student));
      }
      return paymentDTOList;
    }

    public IReadOnlyCollection<TariffDTO> ListTariffs()
    {
      var serviceAgreements = this.coachingServiceAgreementRepository.ListAllAsync().Result;
      var tariffsDTOAssembler = new TariffDTOAssembler();
      return tariffsDTOAssembler.ToDTOList(serviceAgreements);
    }

    public void UpdateTariff(TariffDTO tariff)
    {
      var agreement = this.coachingServiceAgreementRepository.GetByIdAsync(tariff.AgreementId).Result;
      if (agreement == null)
        throw new InvalidOperationException($"Coaching service agreement not found by id {tariff.AgreementId}");

      var accrualType = ValueObject.GetAll<StudentAccountingEventType>().SingleOrDefault(e => e.Name == tariff.AccrualType.AccrualType);
      if (accrualType == null)
        throw new InvalidOperationException($"Accrual type not found by name {tariff.AccrualType.AccrualType}");

      agreement.ChangeDescription(tariff.Description);

      var newTrainingRate = Money.CreateRuble(tariff.TrainingRate);
      if (!tariff.TrainingRate.Equals(newTrainingRate))
        agreement.ChangeRate(newTrainingRate);

      if (!agreement.AccrualEventType.Equals(accrualType))
        agreement.ChangeAccrualType(accrualType);

      this.coachingServiceAgreementRepository.SaveAsync().Wait();
    }

    public TariffDTO ViewTariff(int tariffId)
    {
      var agreement = this.coachingServiceAgreementRepository.GetByIdAsync(tariffId).Result;
      var assembler = new TariffDTOAssembler();
      return assembler.ToDTO(agreement);
    }
  }
}
