using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CoachsBox.Coaching.Accounting.AgreementRegistryEntryModel;
using CoachsBox.Coaching.Accounting.SalaryAccountingEventModel;
using CoachsBox.Coaching.Accounting.SalaryModel;
using CoachsBox.Coaching.Accounting.StudentAccountModel;
using CoachsBox.Coaching.AttendanceLogModel;
using CoachsBox.Core;
using CoachsBox.Core.Interfaces;
using CoachsBox.Core.Primitives;
using MediatR;

namespace CoachsBox.Coaching.Application.Accounting.Commands
{
  public class AllocateDebtsForUnpaidTrainingCountsCommandHandler : IRequestHandler<AllocateDebtsForUnpaidTrainingCountsCommand, bool>
  {
    private readonly ISalaryRepository salaryRepository;
    private readonly IStudentAccountQueryService studentAccountQueryService;
    private readonly IAttendanceQueryService attendanceQueryService;
    private readonly IAccrualService accrualService;
    private readonly IAgreementRegistryEntryRepository agreementRegistryEntryRepository;
    private readonly ISalaryAccountingEventRepository accountingEventRepository;
    private readonly IUnitOfWork unitOfWork;

    public AllocateDebtsForUnpaidTrainingCountsCommandHandler(
      ISalaryRepository salaryRepository,
      IStudentAccountQueryService paymentsQueryService,
      IAttendanceQueryService attendanceQueryService,
      IAccrualService accrualService,
      IAgreementRegistryEntryRepository agreementRegistryEntryRepository,
      ISalaryAccountingEventRepository accountingEventRepository,
      IUnitOfWork unitOfWork)
    {
      this.salaryRepository = salaryRepository;
      this.studentAccountQueryService = paymentsQueryService;
      this.attendanceQueryService = attendanceQueryService;
      this.accrualService = accrualService;
      this.agreementRegistryEntryRepository = agreementRegistryEntryRepository;
      this.accountingEventRepository = accountingEventRepository;
      this.unitOfWork = unitOfWork;
    }

    public async Task<bool> Handle(AllocateDebtsForUnpaidTrainingCountsCommand request, CancellationToken cancellationToken)
    {
      if (request == null)
        throw new ArgumentNullException(nameof(request));

      var salarySpecification = new SalarySpecification(request.SalaryId).WithCalculations();
      var salary = await this.salaryRepository.GetBySpecAsync(salarySpecification);
      var from = salary.PeriodBeginning.ToDateTime().Value;
      var to = salary.PeriodEnding.ToDateTime().Value;
      var accrualsEndPeriod = to.AddDays(1);
      var paymentsEndPeriod = salary.PaymentsPeriodEnding.ToDateTime().Value;

      salary.ResetCoachAllocatedDebtCalculation();

      // TODO: Это все перенести в Salary, потому что это бизнес логика и тут ей не место.
      var calculationList = salary.Calculations.ToList();
      foreach (var calculation in calculationList)
      {
        if (calculation.GetType() == typeof(CoachSalaryCalculation))
        {
          var coachSalaryCalculation = (CoachSalaryCalculation)calculation;
          var groupId = coachSalaryCalculation.GroupId;

          var groupAggrementSpec = new AgreementRegistryEntryByGroupSpecification(groupId).AsReadOnly();
          var groupAggrement = await this.agreementRegistryEntryRepository.GetBySpecAsync(groupAggrementSpec);

          if (groupAggrement == null)
            continue;

          var trainingCost = Money.CreateRuble(groupAggrement.Agreement.Rate.Quantity);

          var allGroupMarksMap = await this.attendanceQueryService.GetStudentAttendanceMarksByCoachAndGroupAsync(coachSalaryCalculation.CoachId, coachSalaryCalculation.GroupId, from, to);
          var groupStudentsBalances = await this.studentAccountQueryService.GetStudentsBalancesByGroup(groupId, accrualsEndPeriod, paymentsEndPeriod);

          foreach (var groupMarksByStudents in allGroupMarksMap)
          {
            var groupDebts = 0;
            foreach (var studentMarks in groupMarksByStudents.Value)
            {
              var trainingCounts = this.accrualService.AccountableTrainingCount(studentMarks.Value);
              var studentId = studentMarks.Key;
              if (groupStudentsBalances.TryGetValue(studentId, out var studentBalance))
              {
                // Если баланс отрицательный.
                if (studentBalance.IsNegative())
                {
                  var studentDebts = (int)decimal.Ceiling(decimal.Divide(Math.Abs(studentBalance.Quantity), trainingCost.Quantity));
                  if (studentDebts > trainingCounts)
                    studentDebts = trainingCounts;

                  groupDebts += studentDebts;
                }
              }
              else
              {
                groupDebts += trainingCounts;
              }
            }

            if (groupDebts > 0)
            {
              coachSalaryCalculation.AllocateDebt(salary, groupDebts, Watch.UtcNow);

              salary.AddCoachSalaryAllocatedDebtCalculation(
                coachSalaryCalculation.CoachId,
                coachSalaryCalculation.GroupId,
                groupDebts,
                trainingCost)
                .Describe("Неоплаченные тренировки");
            }
          }
        }
      }

      await this.salaryRepository.UpdateAsync(salary);
      await this.salaryRepository.SaveAsync();

      return true;
    }
  }
}
