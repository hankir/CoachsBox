using System;
using System.Threading;
using System.Threading.Tasks;
using CoachsBox.Coaching.Accounting.GroupAccountModel;
using CoachsBox.Coaching.Accounting.SalaryAccountingEventModel;
using CoachsBox.Coaching.Accounting.SalaryModel;
using CoachsBox.Coaching.Accounting.SalaryModel.Events;
using CoachsBox.Coaching.CoachModel;
using CoachsBox.Core;
using MediatR;

namespace CoachsBox.Coaching.Application.Accounting.DomainEventHandlers
{
  public class SalaryPaidEventHandler : INotificationHandler<SalaryPaidEvent>
  {
    private readonly ICoachQueryService coachQueryService;
    private readonly ISalaryAccountingEventRepository accountingEventRepository;
    private readonly IGroupAccountRepository groupAccountRepository;

    public SalaryPaidEventHandler(
      ICoachQueryService coachQueryService,
      ISalaryAccountingEventRepository accountingEventRepository,
      IGroupAccountRepository groupAccountRepository)
    {
      this.coachQueryService = coachQueryService;
      this.accountingEventRepository = accountingEventRepository;
      this.groupAccountRepository = groupAccountRepository;
    }

    public async Task Handle(SalaryPaidEvent notification, CancellationToken cancellationToken)
    {
      if (notification == null)
        throw new ArgumentNullException(nameof(notification));

      var salary = notification.Salary;
      var coachIds = await this.coachQueryService.ListAllCoachIdsAsync();

      foreach (var coachId in coachIds)
      {
        var coachSummaryCalculation = salary.ListCoachCalculations(coachId);
        foreach (var calculation in coachSummaryCalculation)
        {
          var groupId = calculation.GroupId;
          var groupAccount = await this.GetGroupAccount(groupId);

          // Формируем событие на выплату по зарплате.
          if (calculation.AmountToIssued.Quantity > 0)
          {
            var salaryPaymentEvent = calculation.MakeSalaryPaymentEvent(salary, groupAccount, Watch.UtcNow);
            salaryPaymentEvent.Process();
            await this.accountingEventRepository.AddAsync(salaryPaymentEvent);
          }

          // Формируем событие долга.
          if (calculation.HasDebt())
          {
            // Событие долга не обрабатываем, если такая функция понадобится,
            // то это будет кредит и при обработке платежей такой кредит должен будет гасится.
            // Вариант с кредитом сложнее и пока не требуется.
            var salaryDebtEvent = calculation.MakeSalaryDebtEvent(salary, Watch.UtcNow);
            await this.accountingEventRepository.AddAsync(salaryDebtEvent);
          }

          if (calculation is CoachSalaryDebtCalculation debtCalculation)
          {
            var debtCalculationEvent = (SalaryDebtAccountingEvent)await this.accountingEventRepository.GetByIdAsync(debtCalculation.SalaryDebtAccountingEventId);
            debtCalculationEvent.Process(salary.Id);
          }
        }
      }
      await this.accountingEventRepository.SaveAsync();
    }

    private async Task<GroupAccount> GetGroupAccount(int groupId)
    {
      var groupAccountSpecification = new GroupAccountSpecification(groupId);
      return await this.groupAccountRepository.GetBySpecAsync(groupAccountSpecification) ??
        throw new InvalidOperationException($"Group account not found for group. GroupId: {groupId}");
    }
  }
}
