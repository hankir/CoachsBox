using System;
using System.Collections.Generic;
using System.Linq;

namespace CoachsBox.WebApp.Pages.Facade.DTO
{
  public class ScheduleTemplateDTO
  {
    private Func<IReadOnlyCollection<RepeatableTrainingTimeDTO>> createByTemplate;

    public static ScheduleTemplateDTO CreateEmpty()
    {
      return new ScheduleTemplateDTO(0, string.Empty, () => Enumerable.Empty<RepeatableTrainingTimeDTO>().ToList());
    }

    public ScheduleTemplateDTO(int id, string displayName, Func<IReadOnlyList<RepeatableTrainingTimeDTO>> createByTemplate)
    {
      this.Id = id;
      this.Name = displayName;
      this.createByTemplate = createByTemplate;
    }

    public int Id { get; }

    public string Name { get; }

    public IReadOnlyCollection<RepeatableTrainingTimeDTO> CreateTrainings()
    {
      return this.createByTemplate();
    }
  }
}