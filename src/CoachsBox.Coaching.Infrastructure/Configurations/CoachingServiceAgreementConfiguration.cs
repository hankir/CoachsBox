using CoachsBox.Coaching.Accounting.CoachingServiceAgreementModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CoachsBox.Coaching.Infrastructure.Configurations
{
  internal class CoachingServiceAgreementConfiguration : IEntityTypeConfiguration<CoachingServiceAgreement>
  {
    public void Configure(EntityTypeBuilder<CoachingServiceAgreement> builder)
    {
      builder.OwnsOne(coachingServiceAgreement => coachingServiceAgreement.AccrualEventType);
      builder.OwnsOne(coachingServiceAgreement => coachingServiceAgreement.Rate);
      builder.OwnsMany(coachingServiceAgreement => coachingServiceAgreement.PostingRules, agreedPostingRuleBuilder =>
      {
        agreedPostingRuleBuilder.WithOwner().HasForeignKey("CoachingServiceAgreementId");
        agreedPostingRuleBuilder.Property<int>("Id").ValueGeneratedOnAdd();
        agreedPostingRuleBuilder.HasKey("Id");

        agreedPostingRuleBuilder.OwnsOne(agreedPostingRule => agreedPostingRule.EventType);
        agreedPostingRuleBuilder
          .HasOne(agreedPostingRule => agreedPostingRule.PostingRule)
          .WithMany()
          .IsRequired()
          .OnDelete(DeleteBehavior.Cascade);
      });
    }
  }
}
