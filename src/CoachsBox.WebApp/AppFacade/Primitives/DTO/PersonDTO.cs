using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CoachsBox.WebApp.AppFacade.Primitives.DTO
{
  public class PersonDTO
  {
    public int Id { get; set; }

    [Display(Name = "Имя")]
    public string FullName { get; set; }

    public string ShortName { get; set; }

    [Display(Name = "Телефон")]
    public string PhoneNumber { get; set; }

    [Display(Name = "Дата рождения")]
    public string Birthday { get; set; }

    [Display(Name = "Адрес")]
    public string Address { get; set; }

    [Display(Name = "Эл. почта")]
    public string Email { get; set; }
  }
}
