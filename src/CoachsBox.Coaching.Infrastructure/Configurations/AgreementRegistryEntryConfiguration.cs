using CoachsBox.Coaching.Accounting.AgreementRegistryEntryModel;
using CoachsBox.Coaching.GroupModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CoachsBox.Coaching.Infrastructure.Configurations
{
  public class AgreementRegistryEntryConfiguration : IEntityTypeConfiguration<AgreementRegistryEntry>
  {
    public void Configure(EntityTypeBuilder<AgreementRegistryEntry> builder)
    {
      builder
        .HasOne(agreementRegistryEntry => agreementRegistryEntry.Agreement)
        .WithMany()
        .IsRequired()
        .OnDelete(DeleteBehavior.Restrict);

      builder
        .HasOne(agreementRegistryEntry => agreementRegistryEntry.GroupAccount)
        .WithMany()
        .IsRequired(false)
        .OnDelete(DeleteBehavior.Restrict);

      // Связь соглашения о стоимости тренировки с группой.
      builder
        .HasOne(typeof(Group))
        .WithOne()
        .HasPrincipalKey(typeof(Group), nameof(Group.Id))
        .HasForeignKey(typeof(AgreementRegistryEntry), nameof(AgreementRegistryEntry.GroupId))
        .IsRequired()
        .OnDelete(DeleteBehavior.Cascade);
    }
  }
}
