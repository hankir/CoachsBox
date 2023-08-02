using CoachsBox.Coaching.GroupModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CoachsBox.Coaching.Infrastructure.Configurations
{
  internal class GroupConfiguration : IEntityTypeConfiguration<Group>
  {
    public void Configure(EntityTypeBuilder<Group> builder)
    {
      builder.HasKey(group => group.Id);
      builder.OwnsOne(group => group.Sport);

      builder.OwnsOne(g => g.TrainingProgramm);

      builder.OwnsMany(g => g.EnrolledStudents, enrolledStudentBuilder =>
      {
        enrolledStudentBuilder.WithOwner().HasForeignKey("GroupId");
        enrolledStudentBuilder.Property<int>("Id").ValueGeneratedOnAdd();
        enrolledStudentBuilder.HasKey("Id");

        enrolledStudentBuilder.Property(enrolledStudent => enrolledStudent.WhenEnrolled).IsRequired();
        enrolledStudentBuilder.Property(enrolledStudent => enrolledStudent.TrialTrainingCount).HasDefaultValue(0);

        enrolledStudentBuilder
          .HasOne(enrolledStudent => enrolledStudent.Student)
          .WithMany()
          .IsRequired()
          .OnDelete(DeleteBehavior.Restrict);
      });

      builder
        .HasOne(group => group.Branch)
        .WithMany()
        .HasForeignKey(group => group.BranchId)
        .IsRequired(true);
    }
  }
}
