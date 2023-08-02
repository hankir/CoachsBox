using System;
using System.Collections.Generic;
using CoachsBox.Coaching.Accounting.SalaryModel;
using CoachsBox.Core;
using CoachsBox.Core.Primitives;
using Xunit;

namespace CoachsBox.UnitTests.AccountingTests
{
  public class SalaryTests
  {
    [Fact]
    public void CreateSalaryTest()
    {
      var expectedPeriodBeginning = new DateTime(2020, 11, 1);
      var expectedPeriodEnding = new DateTime(2020, 11, DateTime.DaysInMonth(2020, 11));
      var salary = new Salary(2020, Month.November);

      Assert.Equal(expectedPeriodBeginning, salary.PeriodBeginning.ToDateTime());
      Assert.Equal(expectedPeriodEnding, salary.PeriodEnding.ToDateTime());
    }

    [Fact]
    public void AddCoachSalaryCalculation()
    {
      var expectedCoachSalary = Money.CreateRuble(decimal.Multiply(5, 150));
      var expectedTotalSalarySum = Money.CreateRuble(decimal.Multiply(5, 150));

      var salary = new Salary(2020, Month.November);
      var salaryCalculation = salary.AddCoachSalaryCalculation(1, 1, 5, Money.CreateRuble(150));

      Assert.Equal(expectedCoachSalary, salaryCalculation.Amount);
      Assert.Equal(expectedTotalSalarySum, salary.TotalAmount());
    }

    [Fact]
    public void AddMultiplyCoachSalaryCalculation()
    {
      var expectedTotalSalarySum = Money.CreateRuble(decimal.Multiply(30, 150));
      var expectedCoachSalary = Money.CreateRuble(decimal.Multiply(15, 150));
      var expectedCoachSalary2 = Money.CreateRuble(decimal.Multiply(15, 150));

      var calculations = new List<SalaryCalculation>();
      var salary = new Salary(2020, Month.November);
      calculations.Add(salary.AddCoachSalaryCalculation(coachId: 1, groupId: 1, trainingCount: 5, Money.CreateRuble(150)));
      calculations.Add(salary.AddCoachSalaryCalculation(coachId: 1, groupId: 2, trainingCount: 10, Money.CreateRuble(150)));
      calculations.Add(salary.AddCoachSalaryCalculation(coachId: 2, groupId: 3, trainingCount: 15, Money.CreateRuble(150)));

      Assert.Equal(expectedTotalSalarySum, salary.TotalAmount());
      Assert.Equal(expectedCoachSalary, salary.TotalCoachAmount(1));
      Assert.Equal(expectedCoachSalary2, salary.TotalCoachAmount(2));
    }

    [Fact]
    public void PropagateCoachSalaryForNotEnoughFund()
    {
      var expectedTotalSalarySum = Money.CreateRuble(decimal.Multiply(30, 150));
      var expectedCoachSalary = Money.CreateRuble(decimal.Multiply(15, 150));
      var expectedCoachSalary2 = Money.CreateRuble(decimal.Multiply(15, 150));

      var expectedCoachDebtTrainings = 3;
      var expectedCoachDebtTrainings2 = 5;

      var payments = Money.CreateRuble(decimal.Multiply(5, 150));
      var payments2 = Money.CreateRuble(decimal.Multiply(7, 150));
      var payments3 = Money.CreateRuble(decimal.Multiply(10, 150));

      var expectedTotalSalarySumToIssued = Money.CreateRuble(decimal.Multiply(22, 150));
      var expectedEstimatedSalaryTotal = Money.CreateRuble(0);
      var expectedCoachSalaryToIssued = Money.CreateRuble(decimal.Multiply(12, 150));
      var expectedCoachSalaryToIssued2 = Money.CreateRuble(decimal.Multiply(10, 150));

      // Расчет зарплаты.
      var salary = new Salary(2020, Month.November);
      salary.AddCoachSalaryCalculation(coachId: 1, groupId: 1, trainingCount: 5, Money.CreateRuble(150));
      salary.AddCoachSalaryCalculation(coachId: 1, groupId: 2, trainingCount: 10, Money.CreateRuble(150));
      salary.AddCoachSalaryCalculation(coachId: 2, groupId: 3, trainingCount: 15, Money.CreateRuble(150));

      // Фонд зарплаты с недостаточной суммой к выдаче.
      var notEnoughSalaryFund = new SalaryFund();
      notEnoughSalaryFund.Add(1, payments);
      notEnoughSalaryFund.Add(2, payments2);
      notEnoughSalaryFund.Add(3, payments3);

      // Рассчеты требуемой зарплаты тренерам.
      var salaryPropagation = salary.CalculateSalaryPropagation(notEnoughSalaryFund);

      // Распределенная сумма зарплаты.
      var propagatedSalaryFund = salary.PropagateSalary(notEnoughSalaryFund, salaryPropagation);

      Assert.Equal(expectedTotalSalarySum, salary.TotalAmount());
      Assert.Equal(expectedCoachSalary, salary.TotalCoachAmount(1));
      Assert.Equal(expectedCoachSalary2, salary.TotalCoachAmount(2));
      Assert.Equal(expectedTotalSalarySumToIssued, propagatedSalaryFund.Total());
      Assert.Equal(expectedCoachDebtTrainings, salary.TotalCoachDebtTrainingCount(1));
      Assert.Equal(expectedCoachDebtTrainings2, salary.TotalCoachDebtTrainingCount(2));
      // Так как фонда зарплаты не хватает для выплаты полной зарплаты, то выплачивается все, в остатке ноль.
      Assert.Equal(expectedEstimatedSalaryTotal, notEnoughSalaryFund.Minus(propagatedSalaryFund).Total());
      Assert.Equal(expectedTotalSalarySumToIssued, salary.TotalAmountToIssued());
      Assert.Equal(expectedCoachSalaryToIssued, salary.TotalCoachAmountToIssued(1));
      Assert.Equal(expectedCoachSalaryToIssued2, salary.TotalCoachAmountToIssued(2));
    }

    [Fact]
    public void PropagateCoachSalaryForNegativeFund()
    {
      var expectedTotalSalarySum = Money.CreateRuble(decimal.Multiply(30, 150));
      var expectedCoachSalary = Money.CreateRuble(decimal.Multiply(15, 150));
      var expectedCoachSalary2 = Money.CreateRuble(decimal.Multiply(15, 150));

      var expectedCoachDebtTrainings = 8;
      var expectedCoachDebtTrainings2 = 15;

      var payments = Money.CreateRuble(decimal.Multiply(5, 150)).Negate();
      var payments2 = Money.CreateRuble(decimal.Multiply(7, 150));
      var payments3 = Money.CreateRuble(decimal.Multiply(10, 150)).Negate();

      var expectedTotalSalarySumToIssued = Money.CreateRuble(decimal.Multiply(7, 150));
      var expectedEstimatedSalaryTotal = Money.CreateRuble(0);
      var expectedCoachSalaryToIssued = Money.CreateRuble(decimal.Multiply(7, 150));
      var expectedCoachSalaryToIssued2 = Money.CreateRuble(0);

      // Расчет зарплаты.
      var salary = new Salary(2020, Month.November);
      salary.AddCoachSalaryCalculation(coachId: 1, groupId: 1, trainingCount: 5, Money.CreateRuble(150));
      salary.AddCoachSalaryCalculation(coachId: 1, groupId: 2, trainingCount: 10, Money.CreateRuble(150));
      salary.AddCoachSalaryCalculation(coachId: 2, groupId: 3, trainingCount: 15, Money.CreateRuble(150));

      // Фонд зарплаты с недостаточной суммой к выдаче.
      var notEnoughSalaryFund = new SalaryFund();
      notEnoughSalaryFund.Add(1, payments);
      notEnoughSalaryFund.Add(2, payments2);
      notEnoughSalaryFund.Add(3, payments3);

      // Рассчеты требуемой зарплаты тренерам.
      var salaryPropagation = salary.CalculateSalaryPropagation(notEnoughSalaryFund);

      // Распределенная сумма зарплаты.
      var propagatedSalaryFund = salary.PropagateSalary(notEnoughSalaryFund, salaryPropagation);

      Assert.Equal(expectedTotalSalarySum, salary.TotalAmount());
      Assert.Equal(expectedCoachSalary, salary.TotalCoachAmount(1));
      Assert.Equal(expectedCoachSalary2, salary.TotalCoachAmount(2));
      Assert.Equal(expectedTotalSalarySumToIssued, propagatedSalaryFund.Total());
      Assert.Equal(expectedCoachDebtTrainings, salary.TotalCoachDebtTrainingCount(1));
      Assert.Equal(expectedCoachDebtTrainings2, salary.TotalCoachDebtTrainingCount(2));
      // Так как фонда зарплаты не хватает для выплаты полной зарплаты, то выплачивается все, в остатке ноль.
      Assert.Equal(expectedEstimatedSalaryTotal, notEnoughSalaryFund.Minus(propagatedSalaryFund).Total());
      Assert.Equal(expectedTotalSalarySumToIssued, salary.TotalAmountToIssued());
      Assert.Equal(expectedCoachSalaryToIssued, salary.TotalCoachAmountToIssued(1));
      Assert.Equal(expectedCoachSalaryToIssued2, salary.TotalCoachAmountToIssued(2));
    }

    [Fact]
    public void PropagateCoachSalaryForSufficientFund()
    {
      var expectedTotalSalarySum = Money.CreateRuble(decimal.Multiply(30, 150));
      var expectedCoachSalary = Money.CreateRuble(decimal.Multiply(15, 150));
      var expectedCoachSalary2 = Money.CreateRuble(decimal.Multiply(15, 150));

      var expectedCoachDebtTrainings = 0;
      var expectedCoachDebtTrainings2 = 0;

      var payments = Money.CreateRuble(decimal.Multiply(10, 150) + 50);
      var payments2 = Money.CreateRuble(decimal.Multiply(20, 150) + 100);
      var payments3 = Money.CreateRuble(decimal.Multiply(30, 150) + 200);

      var expectedTotalSalarySumToIssued = Money.CreateRuble(expectedTotalSalarySum.Quantity);
      var expectedEstimatedSalaryTotal = Money.CreateRuble(decimal.Multiply(60, 150) + 350 - expectedTotalSalarySum.Quantity);
      var expectedCoachSalaryToIssued = Money.CreateRuble(expectedCoachSalary.Quantity);
      var expectedCoachSalaryToIssued2 = Money.CreateRuble(expectedCoachSalary2.Quantity);

      // Расчет зарплаты.
      var salary = new Salary(2020, Month.November);
      salary.AddCoachSalaryCalculation(coachId: 1, groupId: 1, trainingCount: 5, Money.CreateRuble(150));
      salary.AddCoachSalaryCalculation(coachId: 1, groupId: 2, trainingCount: 10, Money.CreateRuble(150));
      salary.AddCoachSalaryCalculation(coachId: 2, groupId: 3, trainingCount: 15, Money.CreateRuble(150));

      // Фонд зарплаты с недостаточной суммой к выдаче.
      var sufficientSalaryFund = new SalaryFund();
      sufficientSalaryFund.Add(1, payments);
      sufficientSalaryFund.Add(2, payments2);
      sufficientSalaryFund.Add(3, payments3);

      // Рассчеты требуемой зарплаты тренерам.
      var salaryPropagation = salary.CalculateSalaryPropagation(sufficientSalaryFund);

      // Распределенная сумма зарплаты.
      var propagatedSalaryFund = salary.PropagateSalary(sufficientSalaryFund, salaryPropagation);

      Assert.Equal(expectedTotalSalarySum, salary.TotalAmount());
      Assert.Equal(expectedCoachSalary, salary.TotalCoachAmount(1));
      Assert.Equal(expectedCoachSalary2, salary.TotalCoachAmount(2));
      Assert.Equal(expectedTotalSalarySumToIssued, propagatedSalaryFund.Total());
      Assert.Equal(expectedCoachDebtTrainings, salary.TotalCoachDebtTrainingCount(1));
      Assert.Equal(expectedCoachDebtTrainings2, salary.TotalCoachDebtTrainingCount(2));
      // Так как фонда зарплаты больше чем требуется для выплаты полной зарплаты, то выплачивается все, в остатке разница.
      Assert.Equal(expectedEstimatedSalaryTotal, sufficientSalaryFund.Minus(propagatedSalaryFund).Total());
      Assert.Equal(expectedTotalSalarySumToIssued, salary.TotalAmountToIssued());
      Assert.Equal(expectedCoachSalaryToIssued, salary.TotalCoachAmountToIssued(1));
      Assert.Equal(expectedCoachSalaryToIssued2, salary.TotalCoachAmountToIssued(2));
    }

    [Fact]
    public void PropagateCoachDebtsSalary()
    {
      var expectedTotalSalarySum = Money.CreateRuble(decimal.Multiply(30, 150));
      var expectedCoachSalary = Money.CreateRuble(decimal.Multiply(15, 150));
      var expectedCoachSalary2 = Money.CreateRuble(decimal.Multiply(15, 150));

      var expectedCoachDebtTrainings = 3;
      var expectedCoachDebtTrainings2 = 5;

      var payments = Money.CreateRuble(decimal.Multiply(5, 150));
      var payments2 = Money.CreateRuble(decimal.Multiply(7, 150));
      var payments3 = Money.CreateRuble(decimal.Multiply(10, 150));

      var expectedTotalSalarySumToIssued = Money.CreateRuble(decimal.Multiply(22, 150));
      var expectedEstimatedSalaryTotal = Money.CreateRuble(0);
      var expectedCoachSalaryToIssued = Money.CreateRuble(decimal.Multiply(12, 150));
      var expectedCoachSalaryToIssued2 = Money.CreateRuble(decimal.Multiply(10, 150));

      // Расчет зарплаты.
      var salary = new Salary(2020, Month.October);
      salary.AddCoachSalaryCalculation(coachId: 1, groupId: 1, trainingCount: 5, Money.CreateRuble(150));
      salary.AddCoachSalaryCalculation(coachId: 1, groupId: 2, trainingCount: 10, Money.CreateRuble(150));
      salary.AddCoachSalaryCalculation(coachId: 2, groupId: 3, trainingCount: 15, Money.CreateRuble(150));

      // Фонд зарплаты с недостаточной суммой к выдаче.
      var notEnoughSalaryFund = new SalaryFund();
      notEnoughSalaryFund.Add(1, payments);
      notEnoughSalaryFund.Add(2, payments2);
      notEnoughSalaryFund.Add(3, payments3);

      // Рассчеты требуемой зарплаты тренерам.
      var salaryPropagation = salary.CalculateSalaryPropagation(notEnoughSalaryFund);

      // Распределенная сумма зарплаты.
      var propagatedSalaryFund = salary.PropagateSalary(notEnoughSalaryFund, salaryPropagation);

      Assert.Equal(expectedTotalSalarySum, salary.TotalAmount());
      Assert.Equal(expectedCoachSalary, salary.TotalCoachAmount(1));
      Assert.Equal(expectedCoachSalary2, salary.TotalCoachAmount(2));
      Assert.Equal(expectedTotalSalarySumToIssued, propagatedSalaryFund.Total());
      Assert.Equal(expectedCoachDebtTrainings, salary.TotalCoachDebtTrainingCount(1));
      Assert.Equal(expectedCoachDebtTrainings2, salary.TotalCoachDebtTrainingCount(2));
      // Так как фонда зарплаты не хватает для выплаты полной зарплаты, то выплачивается все, в остатке ноль.
      Assert.Equal(expectedEstimatedSalaryTotal, notEnoughSalaryFund.Minus(propagatedSalaryFund).Total());
      Assert.Equal(expectedTotalSalarySumToIssued, salary.TotalAmountToIssued());
      Assert.Equal(expectedCoachSalaryToIssued, salary.TotalCoachAmountToIssued(1));
      Assert.Equal(expectedCoachSalaryToIssued2, salary.TotalCoachAmountToIssued(2));

      // Выплата в следующем периоде долгов.
      var nextSalary = new Salary(2020, Month.November);
      foreach (var coachId in new int[] { 1, 2 })
      {
        var coachSummaryCalculation = salary.ListCoachCalculations(coachId);
        foreach (var calculation in coachSummaryCalculation)
        {
          if (calculation.HasDebt())
          {
            var debt = calculation.MakeSalaryDebtEvent(salary, Watch.Now.DateTime);
            nextSalary.AddCoachSalaryDebtCalculation(debt);
          }
        }
      }

      var nextSalaryFund = new SalaryFund();
      nextSalaryFund.Add(2, Money.CreateRuble(decimal.Multiply(expectedCoachDebtTrainings, 150)));
      nextSalaryFund.Add(3, Money.CreateRuble(decimal.Multiply(expectedCoachDebtTrainings2, 150)));

      // Рассчеты требуемой зарплаты тренерам.
      var salaryDebtsPropagation = nextSalary.CalculateSalaryPropagation(nextSalaryFund);

      // Распределенная сумма зарплаты.
      var propagatedSalaryDebtsFund = nextSalary.PropagateSalary(nextSalaryFund, salaryDebtsPropagation);

      expectedTotalSalarySum = Money.CreateRuble(decimal.Multiply(8, 150));
      expectedCoachSalary = Money.CreateRuble(decimal.Multiply(3, 150));
      expectedCoachSalary2 = Money.CreateRuble(decimal.Multiply(5, 150));

      expectedCoachDebtTrainings = 0;
      expectedCoachDebtTrainings2 = 0;

      expectedTotalSalarySumToIssued = Money.CreateRuble(expectedTotalSalarySum.Quantity);
      expectedEstimatedSalaryTotal = Money.CreateRuble(0);
      expectedCoachSalaryToIssued = Money.CreateRuble(expectedCoachSalary.Quantity);
      expectedCoachSalaryToIssued2 = Money.CreateRuble(expectedCoachSalary2.Quantity);

      Assert.Equal(expectedTotalSalarySum, nextSalary.TotalAmount());
      Assert.Equal(expectedCoachSalary, nextSalary.TotalCoachAmount(1));
      Assert.Equal(expectedCoachSalary2, nextSalary.TotalCoachAmount(2));
      Assert.Equal(expectedTotalSalarySumToIssued, propagatedSalaryDebtsFund.Total());
      Assert.Equal(expectedCoachDebtTrainings, nextSalary.TotalCoachDebtTrainingCount(1));
      Assert.Equal(expectedCoachDebtTrainings2, nextSalary.TotalCoachDebtTrainingCount(2));
      // Долги выплачены, в остатке ноль.
      Assert.Equal(expectedEstimatedSalaryTotal, nextSalaryFund.Minus(propagatedSalaryDebtsFund).Total());
      Assert.Equal(expectedTotalSalarySumToIssued, nextSalary.TotalAmountToIssued());
      Assert.Equal(expectedCoachSalaryToIssued, nextSalary.TotalCoachAmountToIssued(1));
      Assert.Equal(expectedCoachSalaryToIssued2, nextSalary.TotalCoachAmountToIssued(2));
    }
  }
}
