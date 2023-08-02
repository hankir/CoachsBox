using System;
using System.Threading.Tasks;
using CoachsBox.Coaching.Accounting.GroupAccountEntryModel;
using CoachsBox.Coaching.Accounting.GroupAccountModel;
using CoachsBox.Coaching.Accounting.SalaryModel;
using CoachsBox.Coaching.Infrastructure.Accounting;
using CoachsBox.Core.Primitives;
using Microsoft.EntityFrameworkCore.Storage;
using Xunit;
using Xunit.Abstractions;

namespace CoachsBox.IntegrationTests.Accounting
{
  public class SalaryQueryServiceTests : IntegrationTestsBase
  {
    private readonly InMemoryDatabaseRoot dbRoot = new InMemoryDatabaseRoot();

    [Fact]
    public async Task GetSalaryFundByPeriod()
    {
      using (var context = this.CreateContext(dbRoot))
      {
        var groupAccount = new GroupAccount(1);
        var groupAccount2 = new GroupAccount(2);

        groupAccount.AddEntry(new GroupAccountEntry(GroupAccountEntryType.Deposit, Money.CreateRuble(100), new DateTime(2020, 11, 18)));
        groupAccount.AddEntry(new GroupAccountEntry(GroupAccountEntryType.Deposit, Money.CreateRuble(100), new DateTime(2020, 11, 19)));
        groupAccount.AddEntry(new GroupAccountEntry(GroupAccountEntryType.Deposit, Money.CreateRuble(100), new DateTime(2020, 11, 20)));

        groupAccount2.AddEntry(new GroupAccountEntry(GroupAccountEntryType.Deposit, Money.CreateRuble(100), new DateTime(2020, 11, 15)));
        groupAccount2.AddEntry(new GroupAccountEntry(GroupAccountEntryType.Deposit, Money.CreateRuble(100), new DateTime(2020, 11, 16)));
        groupAccount2.AddEntry(new GroupAccountEntry(GroupAccountEntryType.Deposit, Money.CreateRuble(100), new DateTime(2020, 11, 17)));

        await context.AddAsync(groupAccount);
        await context.AddAsync(groupAccount2);
        await context.SaveChangesAsync();
      }

      SalaryFund salaryFund;
      using (var readOnlyContext = this.CreateReadOnlyContext(dbRoot))
      {
        var salaryFundService = new SalaryQueryService(readOnlyContext);
        salaryFund = await salaryFundService.GetSalaryFundAsync(new DateTime(2020, 11, 20));
      }

      var expectedFundTotal = Money.CreateRuble(600);
      Assert.Equal(expectedFundTotal, salaryFund.Total());
    }

    public SalaryQueryServiceTests(ITestOutputHelper output)
      : base(output)
    {
    }
  }
}
