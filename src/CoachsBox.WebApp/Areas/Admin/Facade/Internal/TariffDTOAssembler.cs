using System.Collections.Generic;
using System.Linq;
using CoachsBox.Coaching.Accounting.CoachingServiceAgreementModel;
using CoachsBox.WebApp.Areas.Admin.Facade.DTO;

namespace CoachsBox.WebApp.Areas.Admin.Facade.Internal
{
  public class TariffDTOAssembler
  {
    public TariffDTO ToDTO(CoachingServiceAgreement coachingServiceAgreement)
    {
      var accrualType = new AccrualTypeDTOCollection().GetByAccrualType(coachingServiceAgreement.AccrualEventType);

      return new TariffDTO()
      {
        AgreementId = coachingServiceAgreement.Id,
        AccrualType = accrualType,
        Description = coachingServiceAgreement.Description,
        TrainingRate = coachingServiceAgreement.Rate.Quantity
      };
    }

    public List<TariffDTO> ToDTOList(IEnumerable<CoachingServiceAgreement> coachingServiceAgreements)
    {
      return coachingServiceAgreements.Select(this.ToDTO).ToList();
    }
  }
}
