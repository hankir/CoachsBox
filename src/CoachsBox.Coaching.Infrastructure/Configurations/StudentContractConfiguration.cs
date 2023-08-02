using CoachsBox.Coaching.Accounting.StudentContractModel;
using CoachsBox.Coaching.StudentModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CoachsBox.Coaching.Infrastructure.Configurations
{
  internal class StudentContractConfiguration : IEntityTypeConfiguration<StudentContract>
  {
    public void Configure(EntityTypeBuilder<StudentContract> builder)
    {
      builder
        .HasOne<Student>()
        .WithOne()
        .IsRequired()
        .OnDelete(DeleteBehavior.Cascade);

      builder.OwnsOne(contract => contract.Date, dateBuilder =>
      {
        dateBuilder.WithOwner();
        dateBuilder.OwnsOne(date => date.Month);
      });
    }
  }
}
