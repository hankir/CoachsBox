using System;
using System.Threading.Tasks;
using CoachsBox.Core;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Http;

namespace CoachsBox.WebApp.Pages.Students.Components
{
  public partial class StudentsListFilterPanel
  {
    private string information;

    private StudentsListFilter filter;

    [Inject]
    private IHttpContextAccessor HttpAccessor { get; set; }

    [Parameter]
    public StudentsListFilter InitialFilter { get; set; }

    [Parameter]
    public EventCallback<StudentsListFilter> FilterChanged { get; set; }

    protected override void OnInitialized()
    {
      base.OnInitialized();
      this.filter = this.InitialFilter ?? new StudentsListFilter();
      this.filter.BirthdayFrom = Watch.Now.DateTime.Date;
      this.filter.BirthdayTo = Watch.Now.DateTime.Date.AddDays(1);
    }

    public async Task OnKeyDown(KeyboardEventArgs e)
    {
      if (e.Code == "Enter" || e.Code == "NumpadEnter")
        await this.OnFindByNameClick();
    }

    private void OnBirthdayFromChanged(ChangeEventArgs changeEvent)
    {
      if (DateTime.TryParse(changeEvent.Value as string, out var from))
        this.filter.BirthdayFrom = from;
      else
        this.filter.BirthdayFrom = null;
    }

    private void OnBirthdayToChanged(ChangeEventArgs changeEvent)
    {
      if (DateTime.TryParse(changeEvent.Value as string, out var to))
        this.filter.BirthdayTo = to;
      else
        this.filter.BirthdayTo = null;
    }

    private async Task OnFindByNameClick()
    {
      this.filter.BirthdayPeriod = null;
      await this.FilterChanged.InvokeAsync(this.filter);
      this.UpdateInformation();
    }

    private async Task OnBirthdayChildsByPeriodClick(BirthdayPeriod period)
    {
      this.filter.BirthdayPeriod = period;
      await this.FilterChanged.InvokeAsync(this.filter);
      this.UpdateInformation();
    }

    private async Task OnBirthdayChildsClick()
    {
      this.filter.BirthdayPeriod = BirthdayPeriod.Custom;
      await this.FilterChanged.InvokeAsync(this.filter);
      this.UpdateInformation();
    }

    private Task OnShowStudentsWithDebts(ChangeEventArgs e)
    {
      this.filter.IsShowDebtsOnly = !this.filter.IsShowDebtsOnly;
      return this.FilterChanged.InvokeAsync(this.filter);
    }

    private void UpdateInformation()
    {
      if (this.filter.BirthdayPeriod != null)
      {
        switch (this.filter.BirthdayPeriod.Value)
        {
          default:
          case BirthdayPeriod.Today:
            this.information = "Показаны ученики чей день рождения сегодня. Спешите поздравить!";
            break;
          case BirthdayPeriod.Tomorrow:
            this.information = "Показаны ученики чей день рождения завтра. Не забудьте поздравить!";
            break;
          case BirthdayPeriod.ThisWeek:
            this.information = "Показаны ученики чей день рождения на этой неделе.";
            break;
          case BirthdayPeriod.ThisMonth:
            this.information = "Показаны ученики чей день рождения в этом месяце.";
            break;
          case BirthdayPeriod.Custom:
            this.information = $"Показаны ученики чей день рожденя в период с {this.filter.BirthdayFrom?.ToShortDateString()} по {this.filter.BirthdayTo?.ToShortDateString()}.";
            break;
        }
      }
      else
      {
        this.information = string.Empty;
      }
    }
  }
}
