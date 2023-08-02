using CoachsBox.Coaching.PersonModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CoachsBox.Coaching.Infrastructure.Configurations
{
  internal class PersonConfiguration : IEntityTypeConfiguration<Person>
  {
    public void Configure(EntityTypeBuilder<Person> builder)
    {
      builder.HasKey(person => person.Id);
      builder.Property(person => person.Id).ValueGeneratedOnAdd();

      builder.OwnsOne(s => s.Name);
      builder.OwnsOne(s => s.PersonalInformation, c =>
      {
        c.OwnsOne(p => p.PhoneNumber);
        c.OwnsOne(p => p.Email);
        c.OwnsOne(p => p.Address);
      });

      builder.OwnsOne(s => s.Birthday, b => b.OwnsOne(p => p.Month));
      builder.OwnsOne(s => s.Gender);
    }
  }
}