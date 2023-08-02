using System;
using System.Threading;
using System.Threading.Tasks;
using CoachsBox.Accounting;
using CoachsBox.Coaching.Accounting.AgreementRegistryEntryModel;
using CoachsBox.Coaching.Accounting.CoachingAccountingEventModel;
using CoachsBox.Coaching.Accounting.CoachingServiceAgreementModel;
using CoachsBox.Coaching.Accounting.GroupAccountEntryModel;
using CoachsBox.Coaching.Accounting.GroupAccountModel;
using CoachsBox.Coaching.Accounting.SalaryAccountingEventModel;
using CoachsBox.Coaching.Accounting.SalaryModel;
using CoachsBox.Coaching.Accounting.StudentAccountEntryModel;
using CoachsBox.Coaching.Accounting.StudentAccountingEventModel;
using CoachsBox.Coaching.Accounting.StudentAccountModel;
using CoachsBox.Coaching.Accounting.StudentAccountPostingRuleModel;
using CoachsBox.Coaching.Accounting.StudentContractModel;
using CoachsBox.Coaching.AttendanceLogModel;
using CoachsBox.Coaching.BranchModel;
using CoachsBox.Coaching.CoachModel;
using CoachsBox.Coaching.GroupModel;
using CoachsBox.Coaching.Infrastructure.Configurations;
using CoachsBox.Coaching.PersonModel;
using CoachsBox.Coaching.ScheduleModel;
using CoachsBox.Coaching.StudentDocumentModel;
using CoachsBox.Coaching.StudentModel;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace CoachsBox.Coaching.Infrastructure
{
  public class CoachsBoxContext : DbContext
  {
    private readonly IMediator mediator;

    public DbSet<Person> Persons { get; set; }

    public DbSet<Group> Groups { get; set; }

    public DbSet<Student> Students { get; set; }

    public DbSet<Coach> Coaches { get; set; }

    public DbSet<Schedule> Schedules { get; set; }

    public DbSet<Branch> Branches { get; set; }

    public DbSet<AttendanceLog> AttendanceLogs { get; set; }

    public DbSet<AgreementRegistryEntry> AgreementRegistry { get; set; }

    public DbSet<StudentAccount> StudentAccounts { get; set; }

    public DbSet<StudentDocument> StudentDocuments { get; set; }

    public DbSet<CoachingServiceAgreement> CoachingServiceAgreements { get; set; }

    public DbSet<StudentAccountingEvent> StudentAccountingEvents { get; set; }

    public DbSet<SalaryAccountingEvent> SalaryAccountingEvents { get; set; }

    public DbSet<StudentAccountPostingRule> StudentAccountPostingRules { get; set; }

    public DbSet<StudentAccountEntry> StudentAccountEntries { get; set; }

    public DbSet<StudentContract> StudentContracts { get; set; }

    public DbSet<GroupAccount> GroupAccounts { get; set; }

    public DbSet<GroupAccountEntry> GroupAccountEntries { get; set; }

    public DbSet<Salary> Salaries { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
      base.OnModelCreating(modelBuilder);

      // Конфигурация Coaching domain
      modelBuilder.ApplyConfiguration(new PersonConfiguration());
      modelBuilder.ApplyConfiguration(new GroupConfiguration());
      modelBuilder.ApplyConfiguration(new CoachConfiguration());
      modelBuilder.ApplyConfiguration(new StudentConfiguration());
      modelBuilder.ApplyConfiguration(new BranchConfiguration());
      modelBuilder.ApplyConfiguration(new ScheduleConfiguration());
      modelBuilder.ApplyConfiguration(new AttendanceLogConfiguration());

      var studentDocumentConfiguration = new StudentDocumentConfiguration();
      modelBuilder.ApplyConfiguration(studentDocumentConfiguration);
      studentDocumentConfiguration.ConfigureDescedants(modelBuilder);

      // Конфигурация Accounting domain

      // Для сложной иерархии сущностей игнорируем базовые классы, для которых не нужна таблица.
      modelBuilder
        .Ignore<Account>()
        .Ignore<CoachingAccountingEvent>();

      modelBuilder.ApplyConfiguration(new AgreementRegistryEntryConfiguration());
      modelBuilder.ApplyConfiguration(new StudentAccountConfiguration());
      modelBuilder.ApplyConfiguration(new StudentAccountEntryConfiguration());
      modelBuilder.ApplyConfiguration(new CoachingServiceAgreementConfiguration());
      modelBuilder.ApplyConfiguration(new StudentContractConfiguration());
      modelBuilder.ApplyConfiguration(new GroupAccountConfiguration());
      modelBuilder.ApplyConfiguration(new GroupAccountEntryConfiguration());
      modelBuilder.ApplyConfiguration(new SalaryConfiguration());

      var postingRuleConfiguration = new StudentAccountPostingRuleConfiguration();
      modelBuilder.ApplyConfiguration(postingRuleConfiguration);
      postingRuleConfiguration.ConfigureDescedants(modelBuilder);

      var studentAccountingEventConfiguration = new StudentAccountingEventConfiguration();
      modelBuilder.ApplyConfiguration(studentAccountingEventConfiguration);
      studentAccountingEventConfiguration.ConfigureDescedants(modelBuilder);

      var salaryCalculationConfiguration = new SalaryCalculationConfiguration();
      modelBuilder.ApplyConfiguration(salaryCalculationConfiguration);
      salaryCalculationConfiguration.ConfigureDescedants(modelBuilder);

      var salaryAccountingEventConfiguration = new SalaryAccountingEventConfiguration();
      modelBuilder.ApplyConfiguration(salaryAccountingEventConfiguration);
      salaryAccountingEventConfiguration.ConfigureDescedants(modelBuilder);
    }

    public override int SaveChanges()
    {
      // Dispatch Domain Events collection. 
      // Choices:
      // A) Right BEFORE committing data (EF SaveChanges) into the DB will make a single transaction including  
      // side effects from the domain event handlers which are using the same DbContext with "InstancePerLifetimeScope" or "scoped" lifetime
      // B) Right AFTER committing data (EF SaveChanges) into the DB will make multiple transactions. 
      // You will need to handle eventual consistency and compensatory actions in case of failures in any of the Handlers. 

      IDbContextTransaction transaction = null;
      try
      {
        if (this.Database.CurrentTransaction == null)
          transaction = this.Database.BeginTransaction();

        var entitiesSavedCount = base.SaveChanges();
        this.mediator.DispatchDomainEventsAsync(this).Wait();

        transaction?.Commit();

        return entitiesSavedCount;
      }
      catch
      {
        transaction?.Rollback();
        throw;
      }
      finally
      {
        transaction?.Dispose();
      }
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
      // Dispatch Domain Events collection.
      // Choices:
      // A) Right BEFORE committing data (EF SaveChanges) into the DB will make a single transaction including  
      // side effects from the domain event handlers which are using the same DbContext with "InstancePerLifetimeScope" or "scoped" lifetime
      // B) Right AFTER committing data (EF SaveChanges) into the DB will make multiple transactions. 
      // You will need to handle eventual consistency and compensatory actions in case of failures in any of the Handlers. 

      IDbContextTransaction transaction = null;
      {
        try
        {
          if (this.Database.CurrentTransaction == null)
            transaction = await this.Database.BeginTransactionAsync();

          var entitiesSavedCount = await base.SaveChangesAsync(cancellationToken);
          await this.mediator.DispatchDomainEventsAsync(this);

          if (transaction != null)
            await transaction.CommitAsync(cancellationToken);

          return entitiesSavedCount;
        }
        catch
        {
          if (transaction != null)
            await transaction.RollbackAsync();
          throw;
        }
        finally
        {
          transaction?.Dispose();
        }
      }
    }

    public CoachsBoxContext(DbContextOptions<CoachsBoxContext> options, IMediator mediator)
      : base(options)
    {
      this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
    }

    public CoachsBoxContext()
    {
    }
  }

  public class ReadOnlyCoachsBoxContext : CoachsBoxContext
  {
    public ReadOnlyCoachsBoxContext(DbContextOptions<CoachsBoxContext> options, IMediator mediator)
      : base(options, mediator)
    {
      this.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
    }

    public ReadOnlyCoachsBoxContext()
    {
      this.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
    }
  }
}
