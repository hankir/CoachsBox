using CoachsBox.Coaching.Accounting.GroupAccountEntryModel;
using CoachsBox.Coaching.Accounting.GroupAccountModel;
using CoachsBox.Coaching.GroupModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CoachsBox.Coaching.Infrastructure.Configurations
{
  internal class GroupAccountConfiguration : IEntityTypeConfiguration<GroupAccount>
  {
    public void Configure(EntityTypeBuilder<GroupAccount> builder)
    {
      builder.ToTable($"{nameof(CoachsBoxContext.GroupAccounts)}");

      builder
        .HasMany<GroupAccountEntry>(nameof(GroupAccount.Entries))
        .WithOne(groupAccountEntry => groupAccountEntry.GroupAccount)
        .OnDelete(DeleteBehavior.Cascade);

      // Связь счета с группой.
      builder
        .HasOne(typeof(Group))
        .WithOne()
        .HasPrincipalKey(typeof(Group), nameof(Group.Id))
        .HasForeignKey(typeof(GroupAccount), nameof(GroupAccount.GroupId))
        .IsRequired()
        .OnDelete(DeleteBehavior.Cascade);
    }
  }
}
