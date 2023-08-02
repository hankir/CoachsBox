using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CoachsBox.WebApp.Areas.Admin.Facade.DTO
{
  public class BranchDTO
  {
    public BranchDTO(int id, string state, string city, string street, string contactPersonFullName, int contactPersonId, string address, string phoneNumber)
    {
      this.Id = id;
      this.State = state;
      this.City = city;
      this.Street = street;
      this.ContactPersonId = contactPersonId;
      this.ContactPersonFullName = contactPersonFullName;
      this.Address = address;
      this.PhoneNumber = phoneNumber;
    }

    public int Id { get; }

    [Display(Name = "Регион")]
    public string State { get; }

    [Display(Name = "Город")]
    public string City { get; }

    [Display(Name = "Улица")]
    public string Street { get; }

    public int ContactPersonId { get; }

    [Display(Name = "Контактное лицо")]
    public string ContactPersonFullName { get; }

    [Display(Name = "Адрес")]
    public string Address { get; }

    [Display(Name = "Телефон")]
    public string PhoneNumber { get; }
  }
}
