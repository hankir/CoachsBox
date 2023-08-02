using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace CoachsBox.WebApp.Jobs.Data
{
  public class BackgroundServiceDbContext : DbContext
  {
    public DbSet<ServiceInfo> ServiceInfos { get; set; }

    public DbSet<ServiceEvent> ServiceEvents { get; set; }

    public DateTime GetServiceUtcLastRun(string serviceId)
    {
      return this.ServiceEvents
        .AsTracking(QueryTrackingBehavior.NoTracking)
        .Where(serviceEvent => serviceEvent.ServiceId == serviceId)
        .OrderByDescending(serviceEvent => serviceEvent.UtcLastRun)
        .Select(serviceEvent => serviceEvent.UtcLastRun)
        .FirstOrDefault();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
      base.OnModelCreating(modelBuilder);
      modelBuilder.Entity<ServiceInfo>().HasKey(serviceInfo => serviceInfo.ServiceId);

      var serviceEventEntity = modelBuilder.Entity<ServiceEvent>();
      serviceEventEntity.HasKey(nameof(ServiceEvent.ServiceId), nameof(ServiceEvent.UtcLastRun));
      serviceEventEntity
        .HasOne(serviceEvent => serviceEvent.Service)
        .WithMany()
        .HasForeignKey(serviceEvent => serviceEvent.ServiceId)
        .IsRequired(true);
    }

    public BackgroundServiceDbContext(DbContextOptions<BackgroundServiceDbContext> options)
      : base(options)
    {
    }

    public BackgroundServiceDbContext()
    {
    }
  }
}
