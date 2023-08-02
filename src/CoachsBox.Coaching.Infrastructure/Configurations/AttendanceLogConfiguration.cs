using CoachsBox.Coaching.AttendanceLogModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CoachsBox.Coaching.Infrastructure.Configurations
{
  internal class AttendanceLogConfiguration : IEntityTypeConfiguration<AttendanceLog>
  {
    public void Configure(EntityTypeBuilder<AttendanceLog> builder)
    {
      builder.OwnsMany(attendanceLog => attendanceLog.Entries, attendanceLogEntryBuilder =>
      {
        attendanceLogEntryBuilder.WithOwner(attendanceLogEntry => attendanceLogEntry.AttendanceLog).HasForeignKey("AttendanceLogId");
        attendanceLogEntryBuilder.Property<int>("Id").ValueGeneratedOnAdd();
        attendanceLogEntryBuilder.HasKey("Id");

        attendanceLogEntryBuilder.OwnsOne(attendanceLogEntry => attendanceLogEntry.Date, entryDateBuilder =>
        {
          entryDateBuilder.OwnsOne(entryDate => entryDate.Month);
        });

        attendanceLogEntryBuilder.OwnsOne(attendanceLog => attendanceLog.Start);
        attendanceLogEntryBuilder.OwnsOne(attendanceLog => attendanceLog.End);
        attendanceLogEntryBuilder.OwnsOne(attendanceLog => attendanceLog.AbsenceReason);

        attendanceLogEntryBuilder.Property(attendanceLog => attendanceLog.IsTrialTraining).HasDefaultValue(false);

        attendanceLogEntryBuilder
          .HasOne(attendanceLogEntry => attendanceLogEntry.Student)
          .WithMany()
          .HasForeignKey(attendanceLogEntry => attendanceLogEntry.StudentId)
          .IsRequired(true)
          .OnDelete(DeleteBehavior.Restrict);

        attendanceLogEntryBuilder
          .HasOne(attendanceLogEntry => attendanceLogEntry.Coach)
          .WithMany()
          .HasForeignKey(attendanceLogEntry => attendanceLogEntry.CoachId)
          .IsRequired(true)
          .OnDelete(DeleteBehavior.Restrict);
      });

      builder
        .HasOne(attendanceLog => attendanceLog.Group)
        .WithMany()
        .HasForeignKey(attendanceLog => attendanceLog.GroupId)
        .IsRequired(true)
        .OnDelete(DeleteBehavior.Restrict);
    }
  }
}
