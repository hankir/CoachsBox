using System;
using System.Linq;
using CoachsBox.Coaching.Accounting.AgreementRegistryEntryModel;
using CoachsBox.Coaching.Accounting.CoachingServiceAgreementModel;
using CoachsBox.Coaching.Accounting.GroupAccountModel;
using CoachsBox.Coaching.Accounting.StudentAccountEntryModel;
using CoachsBox.Coaching.Accounting.StudentAccountingEventModel;
using CoachsBox.Coaching.Accounting.StudentAccountModel;
using CoachsBox.Coaching.Infrastructure;
using CoachsBox.Core.Primitives;
using Xunit;
using Xunit.Abstractions;

namespace CoachsBox.IntegrationTests.Accounting
{
  public class AddNew : IntegrationTestsBase
  {
    [Fact]
    public void InitialStudentAccount()
    {
      int studentAccountId = 0;
      using (var context = this.CreateContext())
      {
        var studentAccountRepository = new StudentAccountRepository(context);
        var studentAccount = new StudentAccount(1);
        studentAccountRepository.AddAsync(studentAccount).Wait();
        studentAccountId = studentAccount.Id;
        studentAccountRepository.SaveAsync().Wait();
      }

      using (var context = this.CreateContext())
      {
        var studentAccountRepository = new StudentAccountRepository(context);
        var studentAccount = studentAccountRepository.GetByIdAsync(studentAccountId).Result;

        Assert.Equal(1, studentAccount.StudentId);
        Assert.Empty(studentAccount.Entries);
      }
    }

    [Fact]
    public void StudentAccountWithEntry()
    {
      int studentAccountId = 0;
      using (var context = this.CreateContext())
      {
        var studentAccountRepository = new StudentAccountRepository(context);
        var studentAccount = new StudentAccount(1);
        var accountEntry = new StudentAccountEntry(StudentAccountEntryType.Payment, Money.CreateRuble(1200), DateTime.Now, "Платеж за тренировки");
        studentAccount.AddEntry(accountEntry);

        studentAccountRepository.AddAsync(studentAccount).Wait();
        studentAccountId = studentAccount.Id;
        studentAccountRepository.SaveAsync().Wait();
      }

      using (var context = this.CreateContext())
      {
        var studentAccountRepository = new StudentAccountRepository(context);
        var studentAccount = studentAccountRepository.GetByIdAsync(studentAccountId).Result;

        Assert.Equal(1, studentAccount.StudentId);
        Assert.NotEmpty(studentAccount.Entries);

        var accountEntry = (StudentAccountEntry)studentAccount.Entries.First();
        Assert.Equal(accountEntry.EntryType, StudentAccountEntryType.Payment);
        Assert.Equal(1200, accountEntry.Amount.Quantity);
        Assert.Equal("Платеж за тренировки", accountEntry.Description);
      }
    }

    [Fact]
    public void InitialCoachingServiceAgreement()
    {
      int serviceAgreementId = 0;
      using (var context = this.CreateContext())
      {
        var serviceAgreementRepository = new CoachingServiceAgreementRepository(context);
        var serviceAgreement = new CoachingServiceAgreement(Money.CreateRuble(150), StudentAccountingEventType.Accrual, "Общие тренировки");
        serviceAgreementRepository.AddAsync(serviceAgreement).Wait();
        serviceAgreementId = serviceAgreement.Id;
        serviceAgreementRepository.SaveAsync().Wait();
      }

      using (var context = this.CreateContext())
      {
        var serviceAgreementRepository = new CoachingServiceAgreementRepository(context);
        var serviceAgreement = serviceAgreementRepository.GetByIdAsync(serviceAgreementId).Result;

        Assert.Equal(150, serviceAgreement.Rate.Quantity);
        Assert.NotEmpty(serviceAgreement.PostingRules);
        Assert.Equal(2, serviceAgreement.PostingRules.Count);
        Assert.Collection(serviceAgreement.PostingRules,
          rule => rule.EventType.Equals(StudentAccountingEventType.Accrual),
          rule => rule.EventType.Equals(StudentAccountingEventType.Payment));
      }
    }

    [Fact]
    public void CoachingServiceAgreementWithAccrualPostingRule()
    {
      int serviceAgreementId = 0;
      using (var context = this.CreateContext())
      {
        var serviceAgreementRepository = new CoachingServiceAgreementRepository(context);
        var serviceAgreement = new CoachingServiceAgreement(Money.CreateRuble(150), StudentAccountingEventType.Accrual, "Общие тренировки");

        serviceAgreementRepository.AddAsync(serviceAgreement).Wait();
        serviceAgreementId = serviceAgreement.Id;
        serviceAgreementRepository.SaveAsync().Wait();
      }

      using (var context = this.CreateContext())
      {
        var serviceAgreementRepository = new CoachingServiceAgreementRepository(context);
        var serviceAgreement = serviceAgreementRepository.GetByIdAsync(serviceAgreementId).Result;

        Assert.Equal(150, serviceAgreement.Rate.Quantity);
        Assert.NotEmpty(serviceAgreement.PostingRules);

        var accrualPostingRule = serviceAgreement.GetPostingRule(StudentAccountingEventType.Accrual);
        Assert.Equal(StudentAccountEntryType.Accrual, accrualPostingRule.EntryType);

        var paymentPostingRule = serviceAgreement.GetPostingRule(StudentAccountingEventType.Payment);
        Assert.Equal(StudentAccountEntryType.Payment, paymentPostingRule.EntryType);
      }
    }

    [Fact]
    public void CoachingServiceAgreementWithPersonalTrainingAccrualPostingRule()
    {
      int serviceAgreementId = 0;
      using (var context = this.CreateContext())
      {
        var serviceAgreementRepository = new CoachingServiceAgreementRepository(context);
        var serviceAgreement = new CoachingServiceAgreement(Money.CreateRuble(500), StudentAccountingEventType.PersonalTrainingAccrual, "Персональные тренировки");

        serviceAgreementRepository.AddAsync(serviceAgreement).Wait();
        serviceAgreementId = serviceAgreement.Id;
        serviceAgreementRepository.SaveAsync().Wait();
      }

      using (var context = this.CreateContext())
      {
        var serviceAgreementRepository = new CoachingServiceAgreementRepository(context);
        var serviceAgreement = serviceAgreementRepository.GetByIdAsync(serviceAgreementId).Result;

        Assert.Equal(500, serviceAgreement.Rate.Quantity);
        Assert.NotEmpty(serviceAgreement.PostingRules);

        var accrualPostingRule = serviceAgreement.GetPostingRule(StudentAccountingEventType.PersonalTrainingAccrual);
        Assert.Equal(StudentAccountEntryType.Accrual, accrualPostingRule.EntryType);

        var paymentPostingRule = serviceAgreement.GetPostingRule(StudentAccountingEventType.Payment);
        Assert.Equal(StudentAccountEntryType.Payment, paymentPostingRule.EntryType);
      }
    }

    [Fact]
    public void MonthlyAccrualAccountingEvent()
    {
      int accountingEventId = 0;
      using (var context = this.CreateContext())
      {
        var serviceAgreement = new CoachingServiceAgreement(Money.CreateRuble(150), StudentAccountingEventType.Accrual, "Общие тренировки");
        var studentAccount = new StudentAccount(1);

        var accountingEventRepository = new StudentAccountingEventRepository(context);
        var accountingEvent = new MonthlyAccrualAccountingEvent(10, Month.February, DateTime.Now.Year, DateTime.Now, DateTime.Now, 1, studentAccount, serviceAgreement);
        accountingEventRepository.AddAsync(accountingEvent).Wait();
        accountingEventId = accountingEvent.Id;
        accountingEventRepository.SaveAsync().Wait();
      }

      using (var context = this.CreateContext())
      {
        var accountingEventRepository = new StudentAccountingEventRepository(context);
        var accountingEvent = accountingEventRepository.GetByIdAsync(accountingEventId).Result as MonthlyAccrualAccountingEvent;

        Assert.NotNull(accountingEvent);
        Assert.NotNull(accountingEvent.Account);
        Assert.NotNull(accountingEvent.ServiceAgreement);

        Assert.Equal(StudentAccountingEventType.Accrual, accountingEvent.EventType);
        Assert.Equal(1, accountingEvent.GroupId);
        Assert.Equal(10, accountingEvent.TrainingsQuantity);
      }
    }

    [Fact]
    public void PersonalTrainingAccrualAccountingEvent()
    {
      int accountingEventId = 0;
      using (var context = this.CreateContext())
      {
        var serviceAgreement = new CoachingServiceAgreement(Money.CreateRuble(150), StudentAccountingEventType.Accrual, "Общие тренировки");
        var studentAccount = new StudentAccount(1);

        var accountingEventRepository = new StudentAccountingEventRepository(context);
        var now = DateTime.Now;
        var accountingEvent = new PersonalTrainingAccrualAccountingEvent(
          Date.Create(now), TimeOfDay.Create(now.TimeOfDay), TimeOfDay.Create(now.AddHours(1).TimeOfDay),
          DateTime.Now, DateTime.Now, 1, studentAccount, serviceAgreement);
        accountingEventRepository.AddAsync(accountingEvent).Wait();
        accountingEventId = accountingEvent.Id;
        accountingEventRepository.SaveAsync().Wait();
      }

      using (var context = this.CreateContext())
      {
        var accountingEventRepository = new StudentAccountingEventRepository(context);
        var accountingEvent = accountingEventRepository.GetByIdAsync(accountingEventId).Result as PersonalTrainingAccrualAccountingEvent;

        Assert.NotNull(accountingEvent);
        Assert.NotNull(accountingEvent.Account);
        Assert.NotNull(accountingEvent.ServiceAgreement);

        Assert.Equal(StudentAccountingEventType.PersonalTrainingAccrual, accountingEvent.EventType);
        Assert.Equal(1, accountingEvent.GroupId);
      }
    }

    [Fact]
    public void PaymentAccountingEvent()
    {
      int accountingEventId = 0;
      using (var context = this.CreateContext())
      {
        var serviceAgreement = new CoachingServiceAgreement(Money.CreateRuble(150), StudentAccountingEventType.Accrual, "Общие тренировки");
        var studentAccount = new StudentAccount(1);
        var groupAccount = new GroupAccount(1);
        var agreementRegistryEntry = new AgreementRegistryEntry(serviceAgreement, groupAccount);
        var accountingEvent = new PaymentAccountingEvent(Money.CreateRuble(500), DateTime.Now, DateTime.Now, studentAccount, agreementRegistryEntry);

        var accountingEventRepository = new StudentAccountingEventRepository(context);
        accountingEventRepository.AddAsync(accountingEvent).Wait();
        accountingEventRepository.SaveAsync().Wait();

        accountingEventId = accountingEvent.Id;
      }

      using (var context = this.CreateContext())
      {
        var accountingEventRepository = new StudentAccountingEventRepository(context);
        var accountingEvent = accountingEventRepository.GetByIdAsync(accountingEventId).Result as PaymentAccountingEvent;

        Assert.NotNull(accountingEvent);
        Assert.NotNull(accountingEvent.Account);
        Assert.NotNull(accountingEvent.ServiceAgreement);

        Assert.Equal(StudentAccountingEventType.Payment, accountingEvent.EventType);
        Assert.Equal(1, accountingEvent.GroupId);
        Assert.Equal(Money.CreateRuble(500), accountingEvent.Amount);
      }
    }

    [Fact]
    public void CoachingServiceAgreementWithAccrualPostingRuleChangeToPersonalTrainingAccrual()
    {
      int serviceAgreementId = 0;
      using (var context = this.CreateContext())
      {
        var serviceAgreementRepository = new CoachingServiceAgreementRepository(context);
        var serviceAgreement = new CoachingServiceAgreement(Money.CreateRuble(150), StudentAccountingEventType.Accrual, "Общие тренировки");

        serviceAgreementRepository.AddAsync(serviceAgreement).Wait();
        serviceAgreementId = serviceAgreement.Id;
        serviceAgreementRepository.SaveAsync().Wait();
      }

      int oldAccrualPostingRuleId = 0;
      using (var context = this.CreateContext())
      {
        var serviceAgreementRepository = new CoachingServiceAgreementRepository(context);
        var serviceAgreement = serviceAgreementRepository.GetByIdAsync(serviceAgreementId).Result;

        oldAccrualPostingRuleId = serviceAgreement.GetPostingRule(StudentAccountingEventType.Accrual).Id;
        serviceAgreement.ChangeAccrualType(StudentAccountingEventType.PersonalTrainingAccrual);
        serviceAgreementRepository.SaveAsync().Wait();
      }

      using (var context = this.CreateContext())
      {
        var serviceAgreementRepository = new CoachingServiceAgreementRepository(context);
        var serviceAgreement = serviceAgreementRepository.GetByIdAsync(serviceAgreementId).Result;
        var oldAccrualPostingRule = context.StudentAccountPostingRules.Find(oldAccrualPostingRuleId);

        // Старое правило проводки, которое сменили должно быть удалено.
        Assert.Null(oldAccrualPostingRule);

        Assert.Equal(150, serviceAgreement.Rate.Quantity);
        Assert.NotEmpty(serviceAgreement.PostingRules);
        Assert.Equal(2, serviceAgreement.PostingRules.Count);

        var accrualPostingRule = serviceAgreement.GetPostingRule(StudentAccountingEventType.Accrual);
        Assert.Null(accrualPostingRule);

        var personalAccrualPostingRule = serviceAgreement.GetPostingRule(StudentAccountingEventType.PersonalTrainingAccrual);
        Assert.Equal(StudentAccountEntryType.Accrual, personalAccrualPostingRule.EntryType);

        var paymentPostingRule = serviceAgreement.GetPostingRule(StudentAccountingEventType.Payment);
        Assert.Equal(StudentAccountEntryType.Payment, paymentPostingRule.EntryType);
      }
    }

    public AddNew(ITestOutputHelper output)
      : base(output)
    {
    }
  }
}
