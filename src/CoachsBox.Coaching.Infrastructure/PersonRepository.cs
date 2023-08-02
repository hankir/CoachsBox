using System.Threading.Tasks;
using CoachsBox.Coaching.PersonModel;
using CoachsBox.Infrastructure;

namespace CoachsBox.Coaching.Infrastructure
{
  public class PersonRepository : EfRepository<Person, CoachsBoxContext>, IPersonRepository
  {
    public override async Task<Person> GetByIdAsync(int id)
    {
      var person = await this.context.Persons.FindAsync(id);

      // Загрузим персону.
      if (person != null)
        await this.context.Entry(person).Reference(s => s.PersonalInformation).LoadAsync();

      return person;
    }

    public PersonRepository(CoachsBoxContext context)
        : base(context)
    {
    }
  }
}
