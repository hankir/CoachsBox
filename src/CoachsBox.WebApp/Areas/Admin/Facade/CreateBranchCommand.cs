using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using CoachsBox.WebApp.AppFacade.Primitives.DTO;
using CoachsBox.WebApp.Areas.Admin.Facade.DTO;

namespace CoachsBox.WebApp.Areas.Admin.Facade
{
  public class CreateBranchCommand
  {
    /// <summary>
    /// Получить улицу.
    /// </summary>
    [Display(Name = "Улица")]
    [Required(ErrorMessage = "Улица обязательна для заполнения")]
    public string Street { get; set; }

    /// <summary>
    /// Получить город.
    /// </summary>
    [Display(Name = "Город")]
    [Required(ErrorMessage = "Населенный пункт обязателен для заполнения")]
    public string City { get; set; }

    /// <summary>
    /// Получить субъект.
    /// </summary>
    [Display(Name = "Субъект")]
    [Required(ErrorMessage = "Субъект обязателен для заполнения")]
    public string State { get; set; }

    /// <summary>
    /// Получить страну.
    /// </summary>
    [Display(Name = "Страна")]
    [Required(ErrorMessage = "Страна обязательна для заполнения")]
    public string Country { get; set; }

    /// <summary>
    /// Почтовый индекс.
    /// </summary>
    [Display(Name = "Почтовый индекс")]
    public string ZipCode { get; set; }


    [Display(Name = "Контактное лицо")]
    public int ContactPersonId { get; set; }
  }
}
