using CoachsBox.Coaching.Accounting.StudentAccountEntryModel;
using CoachsBox.Coaching.Accounting.StudentAccountPostingRuleModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CoachsBox.Coaching.Infrastructure.Configurations
{
  internal class StudentAccountPostingRuleConfiguration : IEntityTypeConfiguration<StudentAccountPostingRule>
  {
    public void Configure(EntityTypeBuilder<StudentAccountPostingRule> builder)
    {
      builder.OwnsOne(postingRule => (StudentAccountEntryType)postingRule.EntryType);
    }

    public void ConfigureDescedants(ModelBuilder modelBuilder)
    {
      modelBuilder.Entity<MonthlyAccrualPostingRule>().HasBaseType<StudentAccountPostingRule>();
      modelBuilder.Entity<PaymentPostingRule>().HasBaseType<StudentAccountPostingRule>();
      modelBuilder.Entity<PersonalTrainingAccrualPostingRule>().HasBaseType<StudentAccountPostingRule>();
    }
  }
}
