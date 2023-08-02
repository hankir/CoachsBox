using CoachsBox.Coaching.Accounting.SalaryModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CoachsBox.Coaching.Infrastructure.Configurations
{
  internal class SalaryConfiguration : IEntityTypeConfiguration<Salary>
  {
    public void Configure(EntityTypeBuilder<Salary> builder)
    {
      builder.OwnsOne(salary => salary.PeriodBeginning, periodBeginningBuilder =>
      {
        periodBeginningBuilder.WithOwner();
        periodBeginningBuilder.OwnsOne(dateBuilder => dateBuilder.Month);
      });

      builder.OwnsOne(salary => salary.PeriodEnding, periodEndingBuilder =>
      {
        periodEndingBuilder.WithOwner();
        periodEndingBuilder.OwnsOne(dateBuilder => dateBuilder.Month);
      });

      builder.OwnsOne(salary => salary.PaymentsPeriodEnding, paymentsPeriodEndingBuilder =>
      {
        paymentsPeriodEndingBuilder.WithOwner();
        paymentsPeriodEndingBuilder.OwnsOne(dateBuilder => dateBuilder.Month);
      });

      builder
        .HasMany(salary => salary.Calculations)
        .WithOne()
        .OnDelete(DeleteBehavior.Cascade);
    }
  }
}
