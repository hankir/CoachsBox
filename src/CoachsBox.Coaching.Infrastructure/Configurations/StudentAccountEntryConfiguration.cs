using CoachsBox.Coaching.Accounting.StudentAccountEntryModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CoachsBox.Coaching.Infrastructure.Configurations
{
  internal class StudentAccountEntryConfiguration : IEntityTypeConfiguration<StudentAccountEntry>
  {
    public void Configure(EntityTypeBuilder<StudentAccountEntry> builder)
    {
      builder.OwnsOne(studentAccountEntry => (StudentAccountEntryType)studentAccountEntry.EntryType);
      builder.OwnsOne(studentAccountEntry => studentAccountEntry.Amount).WithOwner();
    }
  }
}
