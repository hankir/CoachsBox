using System;
using CoachsBox.Coaching.Accounting.AgreementRegistryEntryModel;
using CoachsBox.Coaching.Accounting.CoachingServiceAgreementModel;
using CoachsBox.Coaching.Accounting.GroupAccountModel;
using CoachsBox.Coaching.Accounting.StudentAccountEntryModel;
using CoachsBox.Coaching.Accounting.StudentAccountingEventModel;
using CoachsBox.Coaching.Accounting.StudentAccountModel;
using CoachsBox.Core.Primitives;
using Xunit;

namespace CoachsBox.UnitTests.AccountingTests
{
  public class GeneralTrainingTests
  {
    [Fact]
    public void AccrualPerMonth()
    {
      const int groupId = 1;
      const int studentId = 1;
      var agreement = new CoachingServiceAgreement(Money.CreateRuble(150), StudentAccountingEventType.Accrual, "Общая тренировка");
      var account = new StudentAccount(studentId);
      var accrualEvent = new MonthlyAccrualAccountingEvent(8, Month.February, DateTime.Now.Year, DateTime.Now, DateTime.Now, groupId, account, agreement);
      accrualEvent.Process();

      Assert.Equal(-1200, account.GetBalance().Quantity);
    }

    [Fact]
    public void PaymentPerMonth()
    {
      const int groupId = 1;
      const int studentId = 1;
      var agreement = new CoachingServiceAgreement(Money.CreateRuble(150), StudentAccountingEventType.Accrual, "Общая тренировка");

      var account = new StudentAccount(studentId);
      account.AddEntry(new StudentAccountEntry(StudentAccountEntryType.Accrual, Money.CreateRuble(-1200), DateTime.Now, string.Empty));

      var groupAccount = new GroupAccount(groupId);
      var agreementRegistryEntry = new AgreementRegistryEntry(agreement, groupAccount);

      var accrualEvent = new PaymentAccountingEvent(Money.CreateRuble(600), DateTime.Now, DateTime.Now, account, agreementRegistryEntry);
      accrualEvent.Process();

      Assert.Equal(-600, account.GetBalance().Quantity);
      Assert.Equal(600, groupAccount.GetBalance().Quantity);
    }

    [Fact(Skip = "Пока не определен метод вычисленя дублей. В StudentAccount не хватает информации об этом.")]
    public void DoubleAccrualPerMonthThrows()
    {
      const int groupId = 1;
      const int studentId = 1;
      var agreement = new CoachingServiceAgreement(Money.CreateRuble(150), StudentAccountingEventType.Accrual, "Общая тренировка");
      var account = new StudentAccount(studentId);
      var accrualEvent = new MonthlyAccrualAccountingEvent(8, Month.February, DateTime.Now.Year, DateTime.Now, DateTime.Now, groupId, account, agreement);
      accrualEvent.Process();

      Assert.Equal(-1200, account.GetBalance().Quantity);

      Assert.Throws<InvalidOperationException>(() => accrualEvent.Process());
    }

    [Fact]
    public void PersonalTrainingAccrual()
    {
      const int groupId = 1;
      const int studentId = 1;
      var agreement = new CoachingServiceAgreement(Money.CreateRuble(500), StudentAccountingEventType.PersonalTrainingAccrual, "Персональная тренировка");
      var account = new StudentAccount(studentId);
      var now = DateTime.Now;
      var accrualEvent = new PersonalTrainingAccrualAccountingEvent(
        Date.Create(now), TimeOfDay.Create(now.TimeOfDay), TimeOfDay.Create(now.AddHours(1).TimeOfDay),
        DateTime.Now, DateTime.Now, groupId, account, agreement);
      accrualEvent.Process();

      Assert.Equal(-500, account.GetBalance().Quantity);
    }

    [Fact]
    public void ChangeAgreementAccrualType()
    {
      const int groupId = 1;
      const int studentId = 1;
      var agreement = new CoachingServiceAgreement(Money.CreateRuble(150), StudentAccountingEventType.Accrual, "Общие тренировки");
      agreement.ChangeAccrualType(StudentAccountingEventType.PersonalTrainingAccrual);

      var account = new StudentAccount(studentId);
      var now = DateTime.Now;
      var accrualEvent = new PersonalTrainingAccrualAccountingEvent(
        Date.Create(now), TimeOfDay.Create(now.TimeOfDay), TimeOfDay.Create(now.AddHours(1).TimeOfDay),
        DateTime.Now, DateTime.Now, groupId, account, agreement);
      accrualEvent.Process();

      Assert.Equal(-150, account.GetBalance().Quantity);
      Assert.Equal(2, agreement.PostingRules.Count);
      Assert.Collection(agreement.PostingRules,
        rule => rule.EventType.Equals(StudentAccountingEventType.Payment),
        rule => rule.EventType.Equals(StudentAccountingEventType.PersonalTrainingAccrual));
    }
  }
}
