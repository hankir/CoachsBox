using CoachsBox.Coaching.CoachModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CoachsBox.Coaching.Infrastructure.Configurations
{
  internal class CoachConfiguration : IEntityTypeConfiguration<Coach>
  {
    public void Configure(EntityTypeBuilder<Coach> builder)
    {
      builder.HasKey(coach => coach.Id);

      builder
        .HasOne(coach => coach.Person)
        .WithMany()
        .HasForeignKey(coach => coach.PersonId)
        .IsRequired(true);
    }
  }
}