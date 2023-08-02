using System;
using System.Collections.Generic;
using System.Text;
using CoachsBox.Coaching.Accounting.CoachingAccountingEventModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CoachsBox.Coaching.Infrastructure.Configurations
{
  internal abstract class BaseCoachingAccountingEventConfiguration<T> : IEntityTypeConfiguration<T> where T : CoachingAccountingEvent
  {
    public abstract void Configure(EntityTypeBuilder<T> builder);

    protected void ConfigureBase(EntityTypeBuilder<T> builder, string tableName)
    {
      builder.OwnsOne(accountingEvent => accountingEvent.ProcessingState, processingStateBuilder =>
      {
        processingStateBuilder.WithOwner(processingState => (T)processingState.AccountingEvent);
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

          resultingEntryBuilder.ToTable($"{typeof(T).Name}{nameof(AccountingEventProcessingState.ResultingEntries)}");
        })
        .ToTable($"{typeof(T).Name}{nameof(CoachingAccountingEvent.ProcessingState)}");
      });

      builder.ToTable(tableName);
    }
  }
}
