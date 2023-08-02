using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoachsBox.WebApp.AppFacade.Accounting.DTO;
using CoachsBox.WebApp.Areas.Admin.Facade.DTO;

namespace CoachsBox.WebApp.Areas.Admin.Facade
{
  public interface IAccountingServiceFacade
  {
    void CreateTariff(TariffDTO tariff);

    void UpdateTariff(TariffDTO tariff);

    IReadOnlyCollection<TariffDTO> ListTariffs();

    IReadOnlyCollection<PaymentDTO> ListRecent(int count);

    IReadOnlyCollection<PaymentDTO> FindPaymentsByStudentName(int count, string studentName);

    IReadOnlyCollection<PaymentDTO> ListRecent(int count, DateTime from);

    TariffDTO ViewTariff(int tariffId);

    Task<IReadOnlyCollection<GroupBalanceDTO>> ListGroupsBalance(DateTime from, DateTime to);

    Task<IReadOnlyCollection<PaymentDTO>> ListStudentPayments(int studentId, int count, DateTime from);
  }
}
