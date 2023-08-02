using CoachsBox.Coaching.Accounting.StudentAccountingEventModel;
using CoachsBox.Coaching.GroupModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CoachsBox.Coaching.Infrastructure.Configurations
{
  internal class StudentAccountingEventConfiguration : BaseCoachingAccountingEventConfiguration<StudentAccountingEvent>
  {
    public override void Configure(EntityTypeBuilder<StudentAccountingEvent> builder)
    {
      /*builder.OwnsOne(studentAccountingEvent => (StudentAccountingEventType)studentAccountingEvent.EventType, studentAccountingEventBuilder =>
      {
        studentAccountingEventBuilder.WithOwner();
        studentAccountingEventBuilder.Property<int>("Id");
        studentAccountingEventBuilder.HasKey("Id");
      });

      builder.OwnsOne(coachingAccountingEvent => coachingAccountingEvent.ProcessingState, processingStateBuilder =>
      {
        processingStateBuilder.WithOwner(processingState => (StudentAccountingEvent)processingState.AccountingEvent);
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

      builder.ToTable($"{nameof(CoachsBoxContext.StudentAccountingEvents)}");*/

      builder.OwnsOne(accountingEvent => (StudentAccountingEventType)accountingEvent.EventType);

      this.ConfigureBase(builder, $"{nameof(CoachsBoxContext.StudentAccountingEvents)}");

      builder
        .HasOne(studentAccountingEvent => studentAccountingEvent.Account)
        .WithMany()
        .IsRequired()
        .OnDelete(DeleteBehavior.Restrict);

      builder
        .HasOne(studentAccountingEvent => studentAccountingEvent.ServiceAgreement)
        .WithMany()
        .IsRequired()
        .OnDelete(DeleteBehavior.Restrict);

      // Связь учетного события с группой.
      builder
        .HasOne(typeof(Group))
        .WithMany()
        .HasForeignKey(nameof(StudentAccountingEvent.GroupId))
        .OnDelete(DeleteBehavior.Cascade);
    }

    public void ConfigureDescedants(ModelBuilder modelBuilder)
    {
      var paymentAccountingEventBuilder = modelBuilder.Entity<PaymentAccountingEvent>().HasBaseType<StudentAccountingEvent>();
      paymentAccountingEventBuilder.OwnsOne(paymentEvent => paymentEvent.Amount);
      paymentAccountingEventBuilder
        .HasOne(paymentAccountingEvent => paymentAccountingEvent.AgreementRegistryEntry)
        .WithMany()
        .IsRequired()
        .OnDelete(DeleteBehavior.Restrict);

      modelBuilder.Entity<MonthlyAccrualAccountingEvent>().HasBaseType<StudentAccountingEvent>().OwnsOne(accuralEvent => accuralEvent.Month);

      var personalTrainingAccuralEventBuilder = modelBuilder.Entity<PersonalTrainingAccrualAccountingEvent>().HasBaseType<StudentAccountingEvent>();
      personalTrainingAccuralEventBuilder.OwnsOne(accuralEvent => accuralEvent.TrainingDate, dateBuilder =>
      {
        dateBuilder.WithOwner();
        dateBuilder.OwnsOne(date => date.Month);
      });
      personalTrainingAccuralEventBuilder.OwnsOne(accuralEvent => accuralEvent.StartOfTraining);
      personalTrainingAccuralEventBuilder.OwnsOne(accuralEvent => accuralEvent.EndOfTraining);
    }
  }
}
