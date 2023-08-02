using System;
using System.Collections.Generic;
using System.Text;
using CoachsBox.Core;

namespace CoachsBox.Coaching.GroupModel
{
  // Программа обучения. Бывают следующие:
  // 4-6 лет
  // 6-9 лет (есть вариации с 6 лет + 2 год обучения) - это видимо одно и тоже.
  // 10-17 лет
  // Взрослые (18+)
  // В Ижевск есть еще классификации:
  // 4-5 лет
  // 6 лет
  // 7-8 лет
  public class TrainingProgramSpecification : ValueObject
  {
    public TrainingProgramSpecification(int minimumAge, int maximumAge)
    {
      if (minimumAge > maximumAge)
        throw new ArgumentException($"Minimum age ({minimumAge}) should be greater or equal than maximum age ({maximumAge}).", nameof(minimumAge));

      this.MinimumAge = minimumAge;
      this.MaximumAge = maximumAge;
    }

    /// <summary>
    /// Получить минимальный возраст программы.
    /// </summary>
    public int MinimumAge { get; private set; }

    /// <summary>
    /// Получить максимальный возраст программы.
    /// </summary>
    public int MaximumAge { get; private set; }

    protected override IEnumerable<object> GetAtomicValues()
    {
      yield return this.MinimumAge;
      yield return this.MaximumAge;
    }

    private TrainingProgramSpecification()
    {
      // Требует Entity framework code
    }
  }
}
