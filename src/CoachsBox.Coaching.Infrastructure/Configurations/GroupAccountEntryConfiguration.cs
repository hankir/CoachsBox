using CoachsBox.Coaching.Accounting.GroupAccountEntryModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CoachsBox.Coaching.Infrastructure.Configurations
{
  internal class GroupAccountEntryConfiguration : IEntityTypeConfiguration<GroupAccountEntry>
  {
    public void Configure(EntityTypeBuilder<GroupAccountEntry> builder)
    {
      builder.OwnsOne(groupAccountEntry => (GroupAccountEntryType)groupAccountEntry.EntryType);
      builder.OwnsOne(groupAccountEntry => groupAccountEntry.Amount).WithOwner();
    }
  }
}
