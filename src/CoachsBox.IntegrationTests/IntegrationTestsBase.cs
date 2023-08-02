using System;
using System.Reflection;
using CoachsBox.Coaching.Accounting.AgreementRegistryEntryModel;
using CoachsBox.Coaching.Accounting.StudentAccountingEventModel;
using CoachsBox.Coaching.Accounting.StudentAccountModel;
using CoachsBox.Coaching.Accounting.StudentAccountPostingRuleModel;
using CoachsBox.Coaching.Application;
using CoachsBox.Coaching.Application.DomainEventHandlers;
using CoachsBox.Coaching.Application.Impl;
using CoachsBox.Coaching.AttendanceLogModel;
using CoachsBox.Coaching.Infrastructure;
using CoachsBox.Coaching.ScheduleModel;
using CoachsBox.Coaching.StudentModel;
using CoachsBox.Core.Interfaces;
using CoachsBox.IntegrationTests.Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Xunit.Abstractions;

namespace CoachsBox.IntegrationTests
{
  public class IntegrationTestsBase : IDisposable
  {
    private bool disposed = false;
    private readonly ServiceCollection serviceCollection;
    private readonly ServiceProvider serviceProvider;
    private readonly IServiceScope serviceScope;
    private readonly ITestOutputHelper outputHelper;
    private readonly string storeName;

    protected CoachsBoxContext CreateContext(InMemoryDatabaseRoot inMemoryDatabaseRoot = null)
    {
      var mediator = this.GetMediator();
      var options = this.CreateDbOptions(inMemoryDatabaseRoot);
      return new CoachsBoxContext(options, mediator);
    }

    protected ReadOnlyCoachsBoxContext CreateReadOnlyContext(InMemoryDatabaseRoot inMemoryDatabaseRoot = null)
    {
      var mediator = this.GetMediator();
      var options = this.CreateDbOptions(inMemoryDatabaseRoot);
      return new ReadOnlyCoachsBoxContext(options, mediator);
    }

    protected ILogger CreateLogger()
    {
      return new XunitLogger(this.outputHelper, this.GetType().Name);
    }

    protected ILogger<T> CreateLogger<T>()
    {
      return new XunitLogger<T>(this.outputHelper, this.GetType().Name);
    }

    private ILoggerFactory GetLoggerFactory(ITestOutputHelper output)
    {
      return this.serviceProvider.GetService<ILoggerFactory>();
    }

    protected virtual IMediator GetMediator()
    {
      return this.serviceProvider.GetRequiredService<IMediator>();
    }

    protected virtual void ConfigureServices(ServiceCollection serviceCollection)
    {
      serviceCollection.AddScoped(typeof(DbContextOptions<CoachsBoxContext>), p => this.CreateDbOptions());
      serviceCollection.AddScoped(typeof(CoachsBoxContext), typeof(CoachsBoxContext));
      serviceCollection.AddScoped(typeof(ILogger<CreatedStudentEventHandler>), p => this.CreateLogger<CreatedStudentEventHandler>());
      serviceCollection.AddScoped(typeof(ILogger<StudentMarkedEventHandler>), p => this.CreateLogger<StudentMarkedEventHandler>());
      serviceCollection.AddScoped(typeof(IStudentAccountRepository), typeof(StudentAccountRepository));
      serviceCollection.AddScoped(typeof(IStudentAccountPostingRuleRepository), typeof(StudentAccountPostingRuleRepository));
      serviceCollection.AddScoped(typeof(IAgreementRegistryEntryRepository), typeof(AgreementRegistryEntryRepository));
      serviceCollection.AddScoped(typeof(IAccrualService), typeof(AccrualService));
      serviceCollection.AddScoped(typeof(IStudentAccountingEventRepository), typeof(StudentAccountingEventRepository));
      serviceCollection.AddScoped(typeof(IUnitOfWork), typeof(CoachsBoxUnitOfWork));
      serviceCollection.AddScoped(typeof(IAttendanceLogRepository), typeof(AttendanceLogRepository));
      serviceCollection.AddScoped(typeof(IScheduleRepository), typeof(ScheduleRepository));
      serviceCollection.AddScoped(typeof(ILogger<AccrualService>), p => this.CreateLogger<AccrualService>());
      serviceCollection.AddMediatR(new[] {
          typeof(Student).GetTypeInfo().Assembly,
          typeof(CreatedStudentEventHandler).Assembly,
        });
    }

    public IntegrationTestsBase(ITestOutputHelper output)
    {
      this.serviceCollection = new ServiceCollection();
      this.ConfigureServices(this.serviceCollection);
      this.serviceProvider = this.serviceCollection.BuildServiceProvider();
      this.serviceScope = this.serviceProvider.CreateScope();

      this.outputHelper = output;
      this.storeName = this.GetType().Name + Guid.NewGuid().ToString();
    }

    private DbContextOptions<CoachsBoxContext> CreateDbOptions(InMemoryDatabaseRoot inMemoryRoot = null)
    {
      return new DbContextOptionsBuilder<CoachsBoxContext>()
        .UseLoggerFactory(this.GetLoggerFactory(this.outputHelper))
        .EnableSensitiveDataLogging()
        .UseInMemoryDatabase(this.storeName, inMemoryRoot)
        .ConfigureWarnings(builder => builder.Ignore(InMemoryEventId.TransactionIgnoredWarning))
        .Options;
    }

    public void Dispose()
    {
      if (!this.disposed)
      {
        this.serviceScope?.Dispose();
        this.disposed = true;
      }
    }
  }
}
