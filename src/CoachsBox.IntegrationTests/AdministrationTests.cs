using System;
using System.Collections.Generic;
using System.Text;
using CoachsBox.Coaching.CoachModel;
using CoachsBox.Coaching.Infrastructure;
using CoachsBox.Coaching.PersonModel;
using CoachsBox.Infrastructure;
using Xunit;
using Xunit.Abstractions;

namespace CoachsBox.IntegrationTests
{
  public class AdministrationTests : IntegrationTestsBase
  {
    [Fact]
    public void CreateCoach()
    {
      var personName = new PersonName("surname", "name");
      var person = new Person(personName);

      var coach = new Coach(person);

      using var context = this.CreateContext();
      var personRepository = new PersonRepository(context);
      var coachRepository = new CoachRepository(context);

      personRepository.AddAsync(person).Wait();
      coachRepository.AddAsync(coach).Wait();

      context.SaveChanges();

      Assert.True(person.Id > 0);
      Assert.True(coach.Id > 0);
      Assert.True(coach.PersonId == person.Id);
    }

    public AdministrationTests(ITestOutputHelper output) : base(output) { }
  }
}
