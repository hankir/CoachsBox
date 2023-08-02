using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using CoachsBox.Coaching.BranchModel;
using CoachsBox.Coaching.CoachModel;
using CoachsBox.Core;
using CoachsBox.WebApp.AppFacade.Accounting.DTO;
using CoachsBox.WebApp.Areas.Admin.Facade;
using CoachsBox.WebApp.Areas.Admin.Facade.DTO;
using CoachsBox.WebApp.Areas.Admin.Facade.Internal;
using CoachsBox.WebApp.Extensions;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace CoachsBox.WebApp.Areas.Admin.Pages.Payments
{
  public partial class AccountingView : OwningComponentBase
  {
    #region Поля и свойства

    private static readonly TimeSpan groupBalancesCacheLifetime = TimeSpan.FromSeconds(10);

    private static readonly TimeSpan filterDataCacheLifetime = TimeSpan.FromMinutes(1);

    private List<GroupBalanceDTO> groupBalances = new List<GroupBalanceDTO>();

    private bool isLoading = false;

    private decimal totalIncome = decimal.Zero;

    private decimal totalDebts = decimal.Zero;

    private DateTime? from;

    private DateTime? to;

    private string tariff;

    private List<string> allowedTariffs = new List<string>();

    private List<CoachDTO> allowedCoaches = new List<CoachDTO>();

    private HashSet<int> selectedCoaches = new HashSet<int>();

    private List<BranchDTO> allowedBranches = new List<BranchDTO>();

    private HashSet<int> selectedBranches = new HashSet<int>();

    #endregion

    #region Жизненный цикл

    protected override async Task OnInitializedAsync()
    {
      var now = Watch.Now.Date;
      this.from = new DateTime(now.Year, now.Month, 1);
      this.to = this.from.Value.AddMonths(1);

      var loadDataTasks = new[]
      {
        this.LoadGroupBalance(this.StartOfDayFrom().Value, this.EndOfDayTo().Value),
        this.LoadCoaches(),
        this.LoadBranches()
      };

      await Task.WhenAll(loadDataTasks);
    }

    #endregion

    #region Методы

    public async Task OnShowGroupBalanceClick()
    {
      if (this.from.HasValue && this.to.HasValue && this.from < this.to)
      {
        try
        {
          this.isLoading = true;
          await this.LoadGroupBalance(this.StartOfDayFrom().Value, this.EndOfDayTo().Value);
        }
        finally
        {
          this.isLoading = false;
        }
      }
    }

    public void OnAllTariffClick()
    {
      this.OnTariffClick(null);
    }

    public void OnTariffClick(string selectedTariff)
    {
      this.tariff = selectedTariff;
      this.RecalculateIncome();
    }

    public void OnChangeCoach(CoachDTO coach)
    {
      if (coach == null)
        return;

      if (this.selectedCoaches.Contains(coach.Id))
        this.selectedCoaches.Remove(coach.Id);
      else
        this.selectedCoaches.Add(coach.Id);
      this.RecalculateIncome();
    }

    public void OnChangeBranch(BranchDTO branch)
    {
      if (branch == null)
        return;

      if (this.selectedBranches.Contains(branch.Id))
        this.selectedBranches.Remove(branch.Id);
      else
        this.selectedBranches.Add(branch.Id);
      this.RecalculateIncome();
    }

    public void ClearCoachFilter()
    {
      this.selectedCoaches.Clear();
      this.RecalculateIncome();
    }

    public void ClearBranchFilter()
    {
      this.selectedBranches.Clear();
      this.RecalculateIncome();
    }

    public List<GroupBalanceDTO> FilteredGroupBalances()
    {
      var visibleGroupBalances = this.groupBalances.AsEnumerable();
      if (this.tariff != null)
        visibleGroupBalances = visibleGroupBalances.Where(b => b.AgreementName == this.tariff);

      if (this.selectedCoaches.Any())
        visibleGroupBalances = visibleGroupBalances.Where(b => this.selectedCoaches.Contains(b.CoachId));

      if (this.selectedBranches.Any())
        visibleGroupBalances = visibleGroupBalances.Where(b => this.selectedBranches.Contains(b.BranchId));

      return visibleGroupBalances.ToList();
    }

    private async Task LoadGroupBalance(DateTime from, DateTime to)
    {
      var logger = this.ScopedServices.GetRequiredService<ILogger<AccountingView>>();
      var sw = Stopwatch.StartNew();
      var cacheKey = $"{this.from}-{this.to}-groupBalances";
      IReadOnlyCollection<GroupBalanceDTO> cachedGroupBalances;
      if (!this.ScopedServices.TryGetCachedData(cacheKey, out cachedGroupBalances))
      {
        var accountingService = this.ScopedServices.GetRequiredService<IAccountingServiceFacade>();
        cachedGroupBalances = await accountingService.ListGroupsBalance(from, to);
        this.ScopedServices.SetCacheData(cacheKey, cachedGroupBalances, groupBalancesCacheLifetime);
      }

      this.groupBalances = cachedGroupBalances.OrderBy(group => group.GroupName).ToList();
      this.allowedTariffs = this.groupBalances.Select(g => g.AgreementName).Distinct().OrderBy(t => t).ToList();
      this.RecalculateIncome();
      var groupIds = this.groupBalances.Select(b => b.GroupId);
      sw.Stop();
      logger.LogDebug("LoadGroupBalance from {From} to {To} at {Elapsed}", from.ToLongDateString(), to.ToLongDateString(), sw.Elapsed);
    }

    private async Task LoadBranches()
    {
      using (var scope = this.ScopedServices.CreateScope())
      {
        const string cachePostfix = "branches";
        IReadOnlyList<Branch> branches;
        if (!scope.ServiceProvider.TryGetCachedData(cachePostfix, out branches))
        {
          var branchRepository = scope.ServiceProvider.GetRequiredService<IBranchRepository>();
          branches = await branchRepository.ListAllAsync();
          scope.ServiceProvider.SetCacheData(cachePostfix, branches, filterDataCacheLifetime);
        }

        var branchAssembler = new BranchDTOAssembler();
        this.allowedBranches = branchAssembler.ToDTOList(branches);
      }
    }

    private async Task LoadCoaches()
    {
      using (var scope = this.ScopedServices.CreateScope())
      {
        const string cachePostfix = "coaches";
        IReadOnlyList<Coach> coaches;
        if (!scope.ServiceProvider.TryGetCachedData(cachePostfix, out coaches))
        {
          var coachRepository = scope.ServiceProvider.GetRequiredService<ICoachRepository>();
          coaches = await coachRepository.ListAllAsync();
          scope.ServiceProvider.SetCacheData(cachePostfix, coaches, filterDataCacheLifetime);
        }

        var assembler = new CoachDTOAssembler();
        var orderedCoasches = coaches.OrderBy(c => c.Person.Name.FullName());
        this.allowedCoaches = assembler.ToDTOList(orderedCoasches);
      }
    }

    private DateTime? StartOfDayFrom()
    {
      return this.from?.Date;
    }

    private DateTime? EndOfDayTo()
    {
      return this.to?.Date.Add(TimeSpan.FromDays(1));
    }

    private void RecalculateIncome()
    {
      var visibleGroupBalances = this.FilteredGroupBalances();
      this.totalIncome = visibleGroupBalances.Sum(g => g.Income);
      this.totalDebts = visibleGroupBalances.Sum(g => g.Depts);
    }

    #endregion
  }
}
