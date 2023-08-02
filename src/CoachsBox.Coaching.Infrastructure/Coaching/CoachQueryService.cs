using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoachsBox.Coaching.CoachModel;
using CoachsBox.Coaching.PersonModel;
using Microsoft.EntityFrameworkCore;

namespace CoachsBox.Coaching.Infrastructure.Coaching
{
  public class CoachQueryService : ICoachQueryService
  {
    private readonly ReadOnlyCoachsBoxContext readOnlyCoachsBox;

    public CoachQueryService(ReadOnlyCoachsBoxContext readOnlyCoachsBox)
    {
      this.readOnlyCoachsBox = readOnlyCoachsBox;
    }

    public async Task<PersonName> GetName(int coachId)
    {
      var person = await this.readOnlyCoachsBox
        .Coaches
        .Where(coach => coach.Id == coachId)
        .Include(coach => coach.Person)
        .Select(coach => coach.Person)
        .SingleOrDefaultAsync();
      return person?.Name;
    }

    public async Task<IReadOnlyList<int>> ListAllCoachIdsAsync()
    {
      return await this.readOnlyCoachsBox
        .Coaches
        .Select(coach => coach.Id)
        .ToListAsync();
    }

    public async Task<IReadOnlyDictionary<int, PersonName>> ListNames(int[] coachIds)
    {
      var persons = await this.readOnlyCoachsBox
        .Coaches
        .Include(coach => coach.Person)
        .Where(coach => coachIds.Contains(coach.Id))
        .Select(coach => new { coachId = coach.Id, Person = coach.Person })
        .ToListAsync();

      var result = new Dictionary<int, PersonName>();
      foreach (var person in persons)
        result[person.coachId] = person.Person.Name;

      return result;
    }
  }
}
