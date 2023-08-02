using System.Collections.Generic;
using CoachsBox.Core;
using CoachsBox.Core.Primitives;

namespace CoachsBox.Coaching.ScheduleModel
{
  /// <summary>
  /// Место проведения тренировок.
  /// </summary>
  public class TrainingLocation : ValueObject
  {
    public static TrainingLocation NotDefined => new TrainingLocation()
    {
      Name = string.Empty,
      Address = Address.Empty
    };

    public TrainingLocation(string name, Address adress)
    {
      this.Name = name;
      this.Address = adress;
    }

    public string Name { get; private set; }

    public Address Address { get; private set; }

    protected override IEnumerable<object> GetAtomicValues()
    {
      yield return this.Name;
      yield return this.Address;
    }

    private TrainingLocation()
    {
      // Требует Entity framework core
    }
  }
}
