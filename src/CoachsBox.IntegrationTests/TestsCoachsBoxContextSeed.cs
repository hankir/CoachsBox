using System.Threading.Tasks;
using CoachsBox.Coaching.BranchModel;
using CoachsBox.Coaching.CoachModel;
using CoachsBox.Coaching.Infrastructure;
using CoachsBox.Coaching.PersonModel;
using CoachsBox.Core.Primitives;

namespace CoachsBox.IntegrationTests
{
  class TestsCoachsBoxContextSeed : CoachsBoxContextSeed
  {
    public static class PersonConstants
    {
      public static readonly PersonName Name = new PersonName("Иванов", "Иван", "Иванович");
      public static readonly Address Address = new Address("Пушкинская 15-18", "Ижевск", "Удмуртия", "Российская Федерация", "426000");
      public static readonly PhoneNumber Phone = new PhoneNumber("+79511995461");
      public static readonly EmailAddress EmailAddress = new EmailAddress("ivan-ii@mail.ru");
      public static readonly PersonalInformation PersonalInformation = new PersonalInformation(Phone, EmailAddress, Address);
    }

    public static class CoachConstants
    {
      public static readonly PersonName Name = new PersonName("Семенов", "Семен", "Семенович");
      public static readonly Address Address = new Address("Спортивная 21-15", "Ижевск", "Удмуртия", "Российская Федерация", "426000");
      public static readonly PhoneNumber Phone = new PhoneNumber("+79121234561");
      public static readonly EmailAddress EmailAddress = new EmailAddress("coach@live.ru");
      public static readonly PersonalInformation PersonalInformation = new PersonalInformation(Phone, EmailAddress, Address);
    }

    protected override async Task SeedOverrideAsync(CoachsBoxContext context)
    {
      await this.SeedPersonsAsync(context);
      await this.SeedCoachsAsync(context);
      await this.SeedBranchesAsync(context);
    }

    private async Task SeedBranchesAsync(CoachsBoxContext context)
    {
      var person = new Person(new PersonName("Строгая", "Светлана", "Васильевна"), Gender.Female, new Date(10, Month.November, 1970));
      await context.Persons.AddAsync(person);

      var address = new Address("ул.Кирова 11 офис 2", "Ижевск", "Удмуртская республика", "Россия", "426000");
      var branch = new Branch(address, person);
      await context.Branches.AddAsync(branch);
    }

    private async Task SeedCoachsAsync(CoachsBoxContext context)
    {
      var person = new Person(CoachConstants.Name, Gender.Male, new Date(21, Month.July, 1974));
      person.AssignPersonalInformation(CoachConstants.PersonalInformation);
      await context.Persons.AddAsync(person);

      var coach = new Coach(person);
      await context.Coaches.AddAsync(coach);
    }

    private async Task SeedPersonsAsync(CoachsBoxContext context)
    {
      var person = new Person(PersonConstants.Name, Gender.Male, new Date(7, Month.July, 2011));
      person.AssignPersonalInformation(PersonConstants.PersonalInformation);
      await context.Persons.AddAsync(person);
    }
  }
}
