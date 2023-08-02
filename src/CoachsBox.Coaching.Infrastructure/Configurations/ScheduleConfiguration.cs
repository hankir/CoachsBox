using CoachsBox.Coaching.ScheduleModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CoachsBox.Coaching.Infrastructure.Configurations
{
  internal class ScheduleConfiguration : IEntityTypeConfiguration<Schedule>
  {
    public void Configure(EntityTypeBuilder<Schedule> builder)
    {
      builder.OwnsMany(schedule => schedule.TrainingList, trainingTimeBuilder =>
      {
        trainingTimeBuilder.OwnsOne(trainingTime => trainingTime.Start);
        trainingTimeBuilder.OwnsOne(trainingTime => trainingTime.End);
        trainingTimeBuilder.OwnsOne(trainingTime => trainingTime.Date, dateBuilder => dateBuilder.OwnsOne(date => date.Month));
        trainingTimeBuilder.WithOwner().HasForeignKey("ScheduleId");
        trainingTimeBuilder.Property<int>("Id").ValueGeneratedOnAdd();
        trainingTimeBuilder.HasKey("Id");
      });

      builder.OwnsOne(schedule => schedule.TrainingLocation, locationBuilder =>
      {
        locationBuilder.OwnsOne(location => location.Address);
      });

      builder
        .HasOne(schedule => schedule.Branch)
        .WithMany()
        .HasForeignKey(schedule => schedule.BranchId)
        .IsRequired()
        .OnDelete(DeleteBehavior.Restrict);

      builder
        .HasOne(schedule => schedule.Group)
        .WithMany()
        .HasPrincipalKey(group => group.Id)
        .HasForeignKey(schedule => schedule.GroupId)
        .IsRequired()
        .OnDelete(DeleteBehavior.Cascade);

      builder
        .HasOne(schedule => schedule.Coach)
        .WithMany()
        .HasForeignKey(schedule => schedule.CoachId)
        .IsRequired()
        .OnDelete(DeleteBehavior.Restrict);
    }
  }
}
