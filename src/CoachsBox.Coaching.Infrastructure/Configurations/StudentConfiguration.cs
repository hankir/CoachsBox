using CoachsBox.Coaching.StudentModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CoachsBox.Coaching.Infrastructure.Configurations
{
  internal class StudentConfiguration : IEntityTypeConfiguration<Student>
  {
    public void Configure(EntityTypeBuilder<Student> builder)
    {
      builder.HasKey(student => student.Id);

      builder
        .HasOne(student => student.Person)
        .WithMany()
        .HasForeignKey(student => student.PersonId)
        .IsRequired(true);

      builder.OwnsMany(student => student.Relatives, relativeBuilder =>
      {
        relativeBuilder.WithOwner().HasForeignKey("StudentId");
        relativeBuilder.Property<int>("Id").ValueGeneratedOnAdd();
        relativeBuilder.HasKey("Id");

        relativeBuilder.OwnsOne(relative => relative.Relationship);

        relativeBuilder
          .HasOne(relative => relative.Person)
          .WithMany()
          .IsRequired()
          .OnDelete(DeleteBehavior.Restrict);
      });
    }
  }
}