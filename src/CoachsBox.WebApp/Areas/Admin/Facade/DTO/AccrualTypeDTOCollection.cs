using System.Collections.Generic;
using System.Linq;
using CoachsBox.Coaching.Accounting.StudentAccountingEventModel;

namespace CoachsBox.WebApp.Areas.Admin.Facade.DTO
{
  public class AccrualTypeDTOCollection
  {
    private static readonly IReadOnlyCollection<AccrualTypeDTO> accrualTypeList = new List<AccrualTypeDTO>()
    {
      new AccrualTypeDTO() { AccrualTypeName = "За каждую тренировку", AccrualType = StudentAccountingEventType.PersonalTrainingAccrual.Name }
    };

    public IReadOnlyCollection<AccrualTypeDTO> ListAll() => accrualTypeList;

    public AccrualTypeDTO GetByAccrualType(StudentAccountingEventType accrualEventType) =>
      accrualTypeList.SingleOrDefault(e => e.AccrualType == accrualEventType.Name);

    public AccrualTypeDTO GetByAccrualType(string accrualEventType) =>
      accrualTypeList.SingleOrDefault(e => e.AccrualType == accrualEventType);
  }
}
