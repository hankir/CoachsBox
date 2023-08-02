using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using CoachsBox.Coaching.Accounting.StudentAccountModel;
using CoachsBox.Coaching.StudentModel;
using CoachsBox.Core;
using CoachsBox.WebApp.AppFacade.Students.DTO;
using CoachsBox.WebApp.Areas.Admin.Facade.Internal;
using CoachsBox.WebApp.Areas.Identity.Data;
using CoachsBox.WebApp.Extensions;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace CoachsBox.WebApp.Pages.Students.Components
{
  public partial class StudentsList : OwningComponentBase<IStudentRepository>
  {
    private IReadOnlyCollection<StudentListItemDTO> studentsList;

    private PaginalSpecification<Student> paginalSpecification;

    private ILogger<StudentsList> logger;

    private int currentPage = 1;

    private int count;

    private StudentsListFilter filter;

    [Inject]
    private IHttpContextAccessor HttpAccessor { get; set; }

    [Parameter]
    public int PortionSize { get; set; }

    [Parameter]
    public StudentsListFilter InitialFilter { get; set; }

    [Parameter]
    public string EmptyListMessage { get; set; }

    protected override async Task OnInitializedAsync()
    {
      this.logger = this.ScopedServices.GetRequiredService<ILogger<StudentsList>>();
      this.filter = this.InitialFilter;
      var studentIdsCachKey = $"StudentsIds_{this.filter.FilterId}";
      var countCachKey = $"StudentsListCount_{this.filter.FilterId}";
      var listCacheKey = $"StudentsList_{this.filter.FilterId}";

      IEnumerable<int> studentsIds;
      if (!this.ScopedServices.TryGetCachedData(studentIdsCachKey, out studentsIds))
      {
        studentsIds = await this.LoadStudentIds();
        this.ScopedServices.SetCacheData(studentIdsCachKey, studentsIds, TimeSpan.FromSeconds(10));
      }

      var specification = this.CreateSpecification(studentsIds);
      this.paginalSpecification = new PaginalSpecification<Student>(specification.AsReadOnly());

      if (!this.ScopedServices.TryGetCachedData(countCachKey, out this.count))
      {
        this.count = await this.LoadCount(specification);
        this.ScopedServices.SetCacheData(countCachKey, this.count, TimeSpan.FromSeconds(10));
      }

      if (!this.ScopedServices.TryGetCachedData(listCacheKey, out this.studentsList))
      {
        await this.ApplyPaging();
        this.ScopedServices.SetCacheData(listCacheKey, this.studentsList, TimeSpan.FromSeconds(10));
      }
    }

    private StudentListSpecification ApplySorting(StudentListSpecification specification)
    {
      var result = this.filter.BirthdayPeriod != null ? specification.SortByBirthday() : specification;
      return result.SortBySurname().SortByName().SortByPatronymic();
    }

    private async Task NextPage()
    {
      if (!this.CanNextPage())
        return;

      this.currentPage++;
      await this.ApplyPaging();
    }

    private bool CanNextPage()
    {
      return this.currentPage < this.CountPages();
    }

    private async Task PreviousPage()
    {
      if (!this.CanPreviousPage())
        return;

      this.currentPage--;
      await this.ApplyPaging();
    }

    private bool CanPreviousPage()
    {
      return this.currentPage > 1;
    }

    private async Task MoveToPage(int page)
    {
      this.currentPage = page;
      await this.ApplyPaging();
    }

    private int CountPages()
    {
      return (int)Math.Ceiling(Decimal.Divide(this.count, this.PortionSize));
    }

    private Task ApplyPaging()
    {
      return this.Measure(Task.Run(async () =>
      {
        this.paginalSpecification.ApplyPaging((this.currentPage - 1) * this.PortionSize, this.PortionSize);
        var assembler = new StudentListItemDTOAssembler();
        var personAssembler = new PersonDTOAssembler();
        var students = await this.Service.ListAsync(this.paginalSpecification);
        var accountQuery = this.ScopedServices.GetRequiredService<IStudentAccountQueryService>();
        var studentsBalances = await accountQuery.GetStudentsBalances(students.Select(s => s.Id));
        this.studentsList = assembler.ToDTOList(students, personAssembler, studentsBalances);
        await this.InvokeAsync(this.StateHasChanged);
      }),
      "Apply students list paging"
      );
    }

    private async Task Measure(Task action, string operationName)
    {
      var sw = Stopwatch.StartNew();
      await action;
      sw.Stop();
      this.logger.LogDebug("{OperationName} time: {Elapsed} ms", operationName, sw.ElapsedMilliseconds);
    }

    private async Task<TResult> Measure<TResult>(Task<TResult> action, string operationName)
    {
      var sw = Stopwatch.StartNew();
      var result = await action;
      sw.Stop();
      this.logger.LogDebug("{OperationName} time: {Elapsed} ms", operationName, sw.ElapsedMilliseconds);
      return result;
    }

    private async Task OnFilterChanged(StudentsListFilter filter)
    {
      this.filter = filter;
      var studentsIds = await this.LoadStudentIds();
      var specification = this.CreateSpecification(studentsIds);
      this.paginalSpecification = new PaginalSpecification<Student>(specification.AsReadOnly());
      this.count = await this.LoadCount(specification);
      this.currentPage = 1;
      await this.ApplyPaging();
    }

    private StudentListSpecification CreateSpecification(IEnumerable<int> studentsIds)
    {
      var specification = (studentsIds == null ?
              filter.CreateSpecification() :
              filter.CreateSpecification(studentsIds)
              )
              .WithPerson();
      return this.ApplySorting(specification);
    }

    private async Task<IEnumerable<int>> LoadStudentIds()
    {
      IReadOnlyList<int> studentsIds = null;
      if (this.filter.IsShowDebtsOnly)
      {
        if (this.HttpAccessor.HttpContext.User.IsInRole(CoachsBoxWebAppRole.Coach))
        {
          var coachId = this.HttpAccessor.HttpContext.User.GetCoachId();
          var studentsAccountQuery = this.ScopedServices.GetRequiredService<IStudentAccountQueryService>();
          studentsIds = await this.Measure(studentsAccountQuery.GetStudentsIdsWithDebtsByCoach(coachId), "Query coachs students ids with debts");
          logger.LogDebug("Coach: {CoachId}, Ids count: {Count}", coachId, studentsIds.Count);
        }
        else
        {
          var studentsAccountQuery = this.ScopedServices.GetRequiredService<IStudentAccountQueryService>();
          studentsIds = await this.Measure(studentsAccountQuery.GetStudentsIdsWithDebts(), "Query all students ids with debts");
          logger.LogDebug("Ids count: {Count}", studentsIds.Count);
        }
      }
      else
      {
        if (this.HttpAccessor.HttpContext.User.IsInRole(CoachsBoxWebAppRole.Coach))
        {
          var studentsQuery = this.ScopedServices.GetRequiredService<IStudentQueryService>();
          var coachId = this.HttpAccessor.HttpContext.User.GetCoachId();
          studentsIds = await this.Measure(studentsQuery.QueryCoachsStudents(coachId), "Query coachs students ids");
          logger.LogDebug("Coach: {CoachId}, Ids count: {Count}", coachId, studentsIds.Count);
        }
      }
      return studentsIds;
    }

    private Task<int> LoadCount(StudentListSpecification specification)
    {
      return this.Measure(this.Service.CountAsync(specification.AsReadOnly()), "Students list count");
    }
  }
}
