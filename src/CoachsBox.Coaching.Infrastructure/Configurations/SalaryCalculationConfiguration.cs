using CoachsBox.Coaching.Accounting.SalaryAccountingEventModel;
using CoachsBox.Coaching.Accounting.SalaryModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CoachsBox.Coaching.Infrastructure.Configurations
{
  internal class SalaryCalculationConfiguration : IEntityTypeConfiguration<SalaryCalculation>
  {
    public void Configure(EntityTypeBuilder<SalaryCalculation> builder)
    {
      builder.OwnsOne(salaryCalculation => salaryCalculation.Amount);
      builder.OwnsOne(salaryCalculation => salaryCalculation.AmountToIssued);
    }

    public void ConfigureDescedants(ModelBuilder builder)
    {
      builder
        .Entity<CoachSalaryCalculation>()
        .HasBaseType<SalaryCalculation>()
        .OwnsOne(coachSalaryCalculation => coachSalaryCalculation.TrainingCost);

      var coachSalaryDebtCalculationBuilder = builder
        .Entity<CoachSalaryDebtCalculation>()
        .HasBaseType<CoachSalaryCalculation>();

      coachSalaryDebtCalculationBuilder
        .HasOne(typeof(SalaryDebtAccountingEvent))
        .WithMany()
        .IsRequired()
        .HasForeignKey(nameof(CoachSalaryDebtCalculation.SalaryDebtAccountingEventId))
        .OnDelete(DeleteBehavior.Restrict);

      builder
        .Entity<CoachSalaryAllocatedDebtCalculation>()
        .HasBaseType<CoachSalaryCalculation>();
    }
  }
}
