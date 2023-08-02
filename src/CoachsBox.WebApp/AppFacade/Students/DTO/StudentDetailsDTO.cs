using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CoachsBox.WebApp.AppFacade.Students.DTO
{
  public class StudentDetailsDTO
  {
    public StudentDetailsDTO()
    {
      this.Relatives = new List<StudentDetailsRelativeDTO>();
    }

    public int StudentId { get; set; }

    [Display(Name = "Имя")]
    public string FullName { get; set; }

    [Display(Name = "Телефон")]
    public string PhoneNumber { get; set; }

    [Display(Name = "Дата рождения")]
    public string Birthday { get; set; }

    [Display(Name = "Адрес")]
    public string Address { get; set; }

    [Display(Name = "Эл. почта")]
    public string Email { get; set; }

    [Display(Name = "Родственники")]
    public List<StudentDetailsRelativeDTO> Relatives { get; set; }

    [Display(Name = "Примечание")]
    public string Note { get; set; }
  }
}
