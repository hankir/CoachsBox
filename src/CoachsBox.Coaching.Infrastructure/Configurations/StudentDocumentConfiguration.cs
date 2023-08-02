using CoachsBox.Coaching.StudentDocumentModel;
using CoachsBox.Coaching.StudentModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CoachsBox.Coaching.Infrastructure.Configurations
{
  internal class StudentDocumentConfiguration : IEntityTypeConfiguration<StudentDocument>
  {
    public void Configure(EntityTypeBuilder<StudentDocument> builder)
    {
      builder
        .HasOne(document => document.Student)
        .WithMany()
        .HasForeignKey(document => document.StudentId)
        .IsRequired()
        .OnDelete(DeleteBehavior.Cascade);
    }

    public void ConfigureDescedants(ModelBuilder modelBuilder)
    {
      modelBuilder.Entity<StudentDocumentModel.Application>().OwnsOne(application => application.Date, dateBuilder =>
      {
        dateBuilder.WithOwner();
        dateBuilder.OwnsOne(date => date.Month);
      });

      modelBuilder.Entity<MedicalCertificate>()
        .OwnsOne(medicalCertificate => medicalCertificate.Date, dateBuilder =>
        {
          dateBuilder.WithOwner();
          dateBuilder.OwnsOne(date => date.Month);
        })
        .OwnsOne(medicalCertificate => medicalCertificate.EndDate, dateBuilder =>
        {
          dateBuilder.WithOwner();
          dateBuilder.OwnsOne(date => date.Month);
        });

      modelBuilder.Entity<InsurancePolicy>()
        .OwnsOne(insurancePolicy => insurancePolicy.Date, dateBuilder =>
        {
          dateBuilder.WithOwner();
          dateBuilder.OwnsOne(date => date.Month);
        })
        .OwnsOne(insurancePolicy => insurancePolicy.EndDate, dateBuilder =>
        {
          dateBuilder.WithOwner();
          dateBuilder.OwnsOne(date => date.Month);
        });
    }
  }
}
