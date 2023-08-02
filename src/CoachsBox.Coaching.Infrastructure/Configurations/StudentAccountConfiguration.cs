using CoachsBox.Coaching.Accounting.StudentAccountEntryModel;
using CoachsBox.Coaching.Accounting.StudentAccountModel;
using CoachsBox.Coaching.StudentModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CoachsBox.Coaching.Infrastructure.Configurations
{
  internal class StudentAccountConfiguration : IEntityTypeConfiguration<StudentAccount>
  {
    public void Configure(EntityTypeBuilder<StudentAccount> builder)
    {
      builder.ToTable($"{nameof(CoachsBoxContext.StudentAccounts)}");

      builder
        .HasMany<StudentAccountEntry>(nameof(StudentAccount.Entries))
        .WithOne(studentAccount => studentAccount.StudentAccount)
        .OnDelete(DeleteBehavior.Cascade);

      // Связь счета со студентом.
      builder
        .HasOne(typeof(Student))
        .WithOne()
        .HasPrincipalKey(typeof(Student), nameof(Student.Id))
        .HasForeignKey(typeof(StudentAccount), nameof(StudentAccount.StudentId))
        .IsRequired()
        .OnDelete(DeleteBehavior.Cascade);
    }
  }
}
