using System;
using System.Collections.Generic;
using System.Linq;
using CoachsBox.Accounting;
using CoachsBox.Coaching.Accounting.SalaryAccountingEventModel;
using CoachsBox.Coaching.Accounting.SalaryModel.Events;
using CoachsBox.Coaching.Accounting.SalaryPostingRuleModel;
using CoachsBox.Core;
using CoachsBox.Core.Primitives;

namespace CoachsBox.Coaching.Accounting.SalaryModel
{
  public class Salary : BaseEntity
  {
    private List<SalaryCalculation> salaryCalculations = new List<SalaryCalculation>();

    public Salary(int year, Month month)
      : this(year, month, Date.Create(new DateTime(year, month.Number, DateTime.DaysInMonth(year, month.Number))))
    {
    }

    public void CorrectPaymentsPeriodEnding(Date paymentsEndPeriod)
    {
      if (paymentsEndPeriod.ToDateTime() < this.PeriodEnding.ToDateTime())
        throw new InvalidOperationException("Payments period should be greather or equals salary calculation end period.");

      this.PaymentsPeriodEnding = Date.Create(paymentsEndPeriod);
      this.AddDomainEvent(new SalaryPaymentsPeriodEndingChangedEvent(this));
    }

    /// <summary>
    /// Создать экземляр расчета зарплаты за указанный месяц.
    /// </summary>
    /// <param name="year">Год, за который расчитывается зарплата.</param>
    /// <param name="month">Месяц, за который расчитывается зарплата.</param>
    public Salary(int year, Month month, Date paymentsPeriodEnding)
    {
      var periodBegining = new Date(1, Month.Create(month.Number), year);
      var periodEnding = new Date((byte)DateTime.DaysInMonth(year, month.Number), Month.Create(month.Number), year);
      var paymentsPeriod = new Date(paymentsPeriodEnding.Day, paymentsPeriodEnding.Month, paymentsPeriodEnding.Year);

      if (periodBegining.ToDateTime() >= this.PeriodEnding.ToDateTime())
        throw new InvalidOperationException($"Period beginning should be greather than period ending");

      if (paymentsPeriod.ToDateTime() < periodEnding.ToDateTime())
        throw new InvalidOperationException($"Payments period should be greather than period ending");

      if (paymentsPeriod.Year < periodEnding.Year)
        throw new InvalidOperationException($"Years of payments period ending should be greather than period ending or equals");

      this.PeriodBeginning = periodBegining;
      this.PeriodEnding = periodEnding;
      this.PaymentsPeriodEnding = paymentsPeriod;

      this.AddDomainEvent(new SalaryCreatedEvent(this));
    }

    public IReadOnlyList<CoachSalaryCalculation> ListCoachCalculations(int coachId)
    {
      return this.Calculations.OfType<CoachSalaryCalculation>()
        .Where(calculation => calculation.CoachId == coachId)
        .ToList();
    }

    /// <summary>
    /// Получить начала периода расчета зарплаты.
    /// </summary>
    public Date PeriodBeginning { get; private set; }

    /// <summary>
    /// Получить окончание периода расчета зарплаты.
    /// </summary>
    public Date PeriodEnding { get; private set; }

    /// <summary>
    /// Получить окончание периода, до которого брать в расчет поступившие платежи.
    /// </summary>
    public Date PaymentsPeriodEnding { get; private set; }

    /// <summary>
    /// Получить признак того, что зарплата выплачена.
    /// </summary>
    public bool IsPaid { get; private set; }

    /// <summary>
    /// Получить коллекцию расчетов зарплат.
    /// </summary>
    public IReadOnlyCollection<SalaryCalculation> Calculations => this.salaryCalculations;

    public PostingRule GetPostingRule(AccountingEventType eventType)
    {
      if (SalaryAccountingEventType.CoachSalaryPayment.Equals(eventType))
        return new CoachSalaryPaymentPostingRule();
      return null;
    }

    /// <summary>
    /// Выплатить зарплату.
    /// </summary>
    public void Pay()
    {
      if (this.IsPaid)
        throw new InvalidOperationException("Salary already paid");

      this.IsPaid = true;
      this.AddDomainEvent(new SalaryPaidEvent(this));
    }

    /// <summary>
    /// Расчитать зарплату тренера.
    /// </summary>
    /// <param name="coachId">Идентификатор тренера.</param>
    /// <param name="groupId">Идентификатор группы.</param>
    /// <param name="trainingCount">Количество тренировок.</param>
    /// <param name="trainingCost">Стоимость тренировки.</param>
    /// <returns>Расчет зарплаты тренера.</returns>
    public CoachSalaryCalculation AddCoachSalaryCalculation(int coachId, int groupId, int trainingCount, Money trainingCost)
    {
      if (this.salaryCalculations.Any(calculation =>
        calculation.GetType() == typeof(CoachSalaryCalculation)
        && ((CoachSalaryCalculation)calculation).GroupId == groupId
        && ((CoachSalaryCalculation)calculation).CoachId == coachId))
        throw new InvalidOperationException("Multiply coach calculation by one group not supported yet");

      var result = new CoachSalaryCalculation(coachId, groupId, trainingCount, trainingCost);
      this.salaryCalculations.Add(result);
      return result;
    }

    public CoachSalaryCalculation AddCoachSalaryAllocatedDebtCalculation(int coachId, int groupId, int debtTrainingCount, Money trainingCost)
    {
      if (this.salaryCalculations.Any(calculation =>
        calculation.GetType() == typeof(CoachSalaryAllocatedDebtCalculation)
        && ((CoachSalaryAllocatedDebtCalculation)calculation).GroupId == groupId
        && ((CoachSalaryAllocatedDebtCalculation)calculation).CoachId == coachId))
        throw new InvalidOperationException("Multiply coach allocated debts calculation by one group not supported yet");

      var result = new CoachSalaryAllocatedDebtCalculation(coachId, groupId, debtTrainingCount, trainingCost);
      this.salaryCalculations.Add(result);
      return result;
    }

    /// <summary>
    /// Расчитать долг по зарплате тренера.
    /// </summary>
    /// <param name="salaryDebtEvent">Событие долга по зарплате.</param>
    /// <param name="coachId">Идентификатор тренера.</param>
    /// <param name="groupId">Идентификатор группы.</param>
    /// <returns>Расчет зарплаты тренера.</returns>
    public CoachSalaryCalculation AddCoachSalaryDebtCalculation(SalaryDebtAccountingEvent salaryDebtEvent)
    {
      var result = new CoachSalaryDebtCalculation(salaryDebtEvent);
      this.salaryCalculations.Add(result);
      return result;
    }

    /// <summary>
    /// Сбросить расчеты зарплат тренеров.
    /// </summary>
    public void ResetCoachCalculation()
    {
      this.ResetCalculationExact<CoachSalaryCalculation>();
    }

    /// <summary>
    /// Сбросить расчеты долгов зарплат тренеров.
    /// </summary>
    public void ResetCoachDebtCalculation()
    {
      this.ResetCalculationExact<CoachSalaryDebtCalculation>();
    }

    /// <summary>
    /// Сбросить расчеты зарплат тренеров.
    /// </summary>
    public void ResetCoachAllocatedDebtCalculation()
    {
      this.ResetCalculationExact<CoachSalaryAllocatedDebtCalculation>();
    }

    private void ResetCalculationExact<T>() where T : SalaryCalculation
    {
      var calculations = this.Calculations.Where(calculation => calculation.GetType() == typeof(T)).ToList();
      foreach (var calculation in calculations)
        this.salaryCalculations.Remove(calculation);
    }

    /// <summary>
    /// Применить расчет зарплаты.
    /// </summary>
    /// <param name="salaryFund">Фонд зарплаты.</param>
    /// <param name="calculation">Расчеты зарплат.</param>
    /// <returns>Распределенная зарплата.</returns>
    public SalaryFund PropagateSalary(SalaryFund salaryFund, IEnumerable<SalaryCalculation> calculations)
    {
      var requiredSalaryFund = new SalaryFund();
      foreach (var salaryCalculation in calculations)
      {
        if (salaryCalculation is CoachSalaryCalculation coachSalaryCalculation)
          requiredSalaryFund.Add(coachSalaryCalculation.GroupId, Money.CreateRuble(salaryCalculation.AmountToIssued.Quantity));
      }

      var estimatedSalaryFund = salaryFund.Minus(requiredSalaryFund);
      if (estimatedSalaryFund.HasNegativeSharesBalance())
        throw new InvalidOperationException($"Salary fund not enough. Required amount: {estimatedSalaryFund.Total().Quantity}");

      this.salaryCalculations.Clear();
      this.salaryCalculations.AddRange(calculations);

      return requiredSalaryFund;
    }

    /// <summary>
    /// Распределить сумму фонда зарплаты к выдаче.
    /// </summary>
    /// <param name="salaryFund">Фонд зарплаты.</param>
    /// <returns>Расчеты требуемой к выдаче зарплаты.</returns>
    public IReadOnlyList<SalaryCalculation> CalculateSalaryPropagation(SalaryFund salaryFund)
    {
      var shares = new HashSet<SalaryFundShare>(salaryFund.Shares.Select(s => new SalaryFundShare(s.GroupId, s.Balance)));
      var estimatedSalaryFund = new SalaryFund(shares);
      var result = new List<SalaryCalculation>();
      var orderedCalculations = this.OrderedCalculationsForPropagation();
      foreach (var salaryCalculation in orderedCalculations)
      {
        if (salaryCalculation is CoachSalaryAllocatedDebtCalculation)
        {
          result.Add(salaryCalculation);
          continue;
        }

        if (salaryCalculation is CoachSalaryCalculation coachSalaryCalculation)
        {
          var estimatedShare = estimatedSalaryFund.GetShare(coachSalaryCalculation.GroupId);
          if (estimatedShare != null)
          {
            var propagatedSalaryCalculated = salaryCalculation.PropagateSalary(estimatedShare.Balance);
            estimatedSalaryFund.Minus(coachSalaryCalculation.GroupId, propagatedSalaryCalculated.AmountToIssued);
            CopyDescription(salaryCalculation, propagatedSalaryCalculated);
            result.Add(propagatedSalaryCalculated);
          }
          else
          {
            // В фонде зарплаты нет доли по этой группе, поэтому распределяем сумму 0;
            var propagatedSalaryCalculated = salaryCalculation.PropagateSalary(Money.CreateRuble(0));
            CopyDescription(salaryCalculation, propagatedSalaryCalculated);
            result.Add(propagatedSalaryCalculated);
          }
        }
      }
      return result;
    }

    /// <summary>
    /// Отсортировать расчеты для распределения фонда.
    /// </summary>
    /// <returns>Отсортированный список расчетов.</returns>
    private IReadOnlyList<SalaryCalculation> OrderedCalculationsForPropagation()
    {
      return this.Calculations.OrderBy(calculation => calculation, SalaryCalculationComparer.Instance).ToList();
    }

    public int TotalCoachDebtTrainingCount(int coachId)
    {
      var result = 0;
      foreach (var calculation in this.Calculations)
      {
        if (calculation is CoachSalaryCalculation coachSalaryCalculation && coachSalaryCalculation.CoachId == coachId)
          result += coachSalaryCalculation.CountDebtTrainings();
      }
      return result;
    }

    public Money AmountToIssued(int calculationId)
    {
      foreach (var calculation in this.Calculations)
      {
        if (calculation.Id == calculationId)
          return Money.CreateRuble(calculation.AmountToIssued.Quantity);
      }

      throw new InvalidOperationException($"Salary calculation not fount. SalaryId: {this.Id}, CalculationId: {calculationId}");
    }

    /// <summary>
    /// Получить итоговую сумму расчетов зарплат.
    /// </summary>
    /// <returns>Сумма расчетов зарплат.</returns>
    public Money TotalAmount()
    {
      var result = Money.CreateRuble(0);
      foreach (var calculation in this.salaryCalculations)
        result = result.Add(calculation.Amount);
      return result;
    }

    /// <summary>
    /// Получить итоговую сумму расчетов зарплаты тренера.
    /// </summary>
    /// <returns>Сумма расчетов зарплат.</returns>
    public Money TotalCoachAmount(int coachId)
    {
      var result = Money.CreateRuble(0);
      foreach (var calculation in this.salaryCalculations)
      {
        if (calculation is CoachSalaryCalculation coachSalaryCalculation && coachSalaryCalculation.CoachId == coachId)
          result = result.Add(calculation.Amount);
      }
      return result;
    }

    /// <summary>
    /// Получить итоговую сумму зарплаты к выдаче.
    /// </summary>
    /// <returns>Сумма к выдаче.</returns>
    public Money TotalAmountToIssued()
    {
      var result = Money.CreateRuble(0);
      foreach (var calculation in this.salaryCalculations)
        result = result.Add(calculation.AmountToIssued);
      return result;
    }

    /// <summary>
    /// Получить итоговую сумму зарплаты к выдаче тренеру.
    /// </summary>
    /// <returns>Сумма зарплаты к выдаче тренеру.</returns>
    public Money TotalCoachAmountToIssued(int coachId)
    {
      var result = Money.CreateRuble(0);
      foreach (var calculation in this.salaryCalculations)
      {
        if (calculation is CoachSalaryCalculation coachSalaryCalculation && coachSalaryCalculation.CoachId == coachId)
          result = result.Add(calculation.AmountToIssued);
      }
      return result;
    }

    private static void CopyDescription(SalaryCalculation fromCalculation, SalaryCalculation toCalculation)
    {
      var description = fromCalculation.Description;
      if (!string.IsNullOrWhiteSpace(description))
        toCalculation.Describe(description);
    }

    private Salary()
    {
      // Требует Entity framework core
    }
  }
}
