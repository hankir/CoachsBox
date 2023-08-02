using CoachsBox.Coaching.BranchModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CoachsBox.Coaching.Infrastructure.Configurations
{
  internal class BranchConfiguration : IEntityTypeConfiguration<Branch>
  {
    public void Configure(EntityTypeBuilder<Branch> builder)
    {
      builder.OwnsOne(branch => branch.Address);
      builder.OwnsMany(branch => branch.CoachingStaff, coachingStaffMemberBuilder =>
      {
        coachingStaffMemberBuilder.WithOwner().HasForeignKey("BranchId");
        coachingStaffMemberBuilder.Property<int>("Id").ValueGeneratedOnAdd();
        coachingStaffMemberBuilder.HasKey("Id");

        coachingStaffMemberBuilder
          .HasOne(coachingStaffMember => coachingStaffMember.Coach)
          .WithOne()
          .IsRequired()
          .OnDelete(DeleteBehavior.Restrict);
      });
      builder
        .HasOne(branch => branch.ContactPerson)
        .WithMany()
        .HasForeignKey("ContactPersonId")
        .IsRequired()
        .OnDelete(DeleteBehavior.Restrict);
    }
  }
}
