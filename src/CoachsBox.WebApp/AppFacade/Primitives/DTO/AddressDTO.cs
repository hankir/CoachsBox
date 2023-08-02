using System.ComponentModel.DataAnnotations;

namespace CoachsBox.WebApp.AppFacade.Primitives.DTO
{
  public class AddressDTO
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

    public string FullAddress()
    {
      var result = !string.IsNullOrWhiteSpace(this.State) ? this.State : string.Empty;
      if (!string.IsNullOrWhiteSpace(this.City))
        result = string.IsNullOrWhiteSpace(result) ? this.City : $"{result}, {this.City}";

      if (!string.IsNullOrWhiteSpace(this.Street))
        result += $", {this.Street}";

      return result;
    }
  }
}
