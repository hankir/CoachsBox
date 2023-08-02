using CoachsBox.Coaching.Accounting.CoachingAccountingEventModel;
using CoachsBox.Coaching.Accounting.SalaryAccountingEventModel;
using CoachsBox.Coaching.Accounting.SalaryModel;
using CoachsBox.Coaching.CoachModel;
using CoachsBox.Coaching.GroupModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CoachsBox.Coaching.Infrastructure.Configurations
{
  internal class SalaryAccountingEventConfiguration : BaseCoachingAccountingEventConfiguration<SalaryAccountingEvent>
  {
    public override void Configure(EntityTypeBuilder<SalaryAccountingEvent> builder)
    {
      /*builder.OwnsOne(salaryAccountingEvent => (SalaryAccountingEventType)salaryAccountingEvent.EventType);

      builder.OwnsOne(salaryAccountingEvent => salaryAccountingEvent.ProcessingState, processingStateBuilder =>
      {
        processingStateBuilder.WithOwner(processingState => (SalaryAccountingEvent)processingState.AccountingEvent);
        processingStateBuilder.Property(processingState => processingState.IsProcessed).IsConcurrencyToken();
        processingStateBuilder.Property(processingState => processingState.WhenProcessed).IsConcurrencyToken();
        processingStateBuilder.OwnsMany(processingState => processingState.ResultingEntries, resultingEntryBuilder =>
        {
          resultingEntryBuilder.WithOwner().HasForeignKey("ProcessingStateId");
          resultingEntryBuilder.Property<int>("Id").ValueGeneratedOnAdd();
          resultingEntryBuilder.HasKey("Id");

          resultingEntryBuilder
            .HasOne(resultingEntry => resultingEntry.AccountEntry)
            .WithOne()
            .IsRequired();

          resultingEntryBuilder.ToTable(nameof(AccountingEventResultingEntry));
        })
        .ToTable("AccountingEventProcessLog");
      });

      builder.ToTable($"{nameof(CoachsBoxContext.SalaryAccountingEvents)}");*/

      builder.OwnsOne(accountingEvent => (SalaryAccountingEventType)accountingEvent.EventType);

      this.ConfigureBase(builder, $"{nameof(CoachsBoxContext.SalaryAccountingEvents)}");

      builder
        .HasOne(salaryAccountingEvent => salaryAccountingEvent.Salary)
        .WithMany()
        .IsRequired()
        .OnDelete(DeleteBehavior.Restrict);
    }

    public void ConfigureDescedants(ModelBuilder modelBuilder)
    {
      var salaryPaymentEventBuilder = modelBuilder.Entity<SalaryPaymentAccountingEvent>().HasBaseType<SalaryAccountingEvent>();
      salaryPaymentEventBuilder
        .HasOne<SalaryCalculation>()
        .WithMany()
        .IsRequired()
        .HasForeignKey(nameof(SalaryPaymentAccountingEvent.CalculationId))
        .OnDelete(DeleteBehavior.Restrict);

      var salaryDebtEventBuilder = modelBuilder.Entity<SalaryDebtAccountingEvent>().HasBaseType<SalaryAccountingEvent>();
      salaryDebtEventBuilder.OwnsOne(salaryDebtEvent => salaryDebtEvent.TrainingCost);
      salaryDebtEventBuilder
        .HasOne<Coach>()
        .WithMany()
        .IsRequired()
        .HasForeignKey(nameof(SalaryDebtAccountingEvent.CoachId))
        .OnDelete(DeleteBehavior.Restrict);

      salaryDebtEventBuilder
        .HasOne<Group>()
        .WithMany()
        .IsRequired()
        .HasForeignKey(nameof(SalaryDebtAccountingEvent.GroupId))
        .OnDelete(DeleteBehavior.Restrict);

      salaryDebtEventBuilder
        .HasOne<Salary>()
        .WithMany()
        .IsRequired(false)
        .HasForeignKey(nameof(SalaryDebtAccountingEvent.ProcessedInSalaryId))
        .OnDelete(DeleteBehavior.Restrict);
    }
  }
}
