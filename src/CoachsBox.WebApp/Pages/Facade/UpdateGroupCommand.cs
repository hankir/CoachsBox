using System.ComponentModel.DataAnnotations;

namespace CoachsBox.WebApp.Pages.Facade
{
  public class UpdateGroupCommand
  {
    public UpdateGroupCommand(int groupId, string name, int programId)
    {
      this.GroupId = groupId;
      this.Name = name;
      this.ProgramId = programId;
    }

    public UpdateGroupCommand(int groupId, string name, int programId, int tariffId)
      : this(groupId, name, programId)
    {
      this.TariffId = tariffId;
    }

    public int GroupId { get; set; }

    [Display(Name = "Имя группы")]
    public string Name { get; set; }

    [Display(Name = "Программа тренировок")]
    public int ProgramId { get; set; }

    [Display(Name = "Стоимость тренировки")]
    public int TariffId { get; set; }

    public UpdateGroupCommand()
    {
      // Требует Razor Pages.
    }
  }
}
