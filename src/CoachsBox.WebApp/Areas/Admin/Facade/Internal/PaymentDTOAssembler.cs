using CoachsBox.Coaching.Accounting.StudentAccountingEventModel;
using CoachsBox.Coaching.GroupModel;
using CoachsBox.Coaching.StudentModel;
using CoachsBox.WebApp.Areas.Admin.Facade.DTO;

namespace CoachsBox.WebApp.Areas.Admin.Facade.Internal
{
  public class PaymentDTOAssembler
  {
    public PaymentDTO ToDTO(PaymentAccountingEvent payment, Group group, Student student)
    {
      return new PaymentDTO()
      {
        PaymentId = payment.Id,
        Amount = payment.Amount.Quantity,
        WhenOccured = payment.WhenOccured,
        WhenNoticed = payment.WhenNoticed,
        WhenProcessed = payment.ProcessingState.WhenProcessed,
        IsProcessed = payment.ProcessingState.IsProcessed,
        StudentId = student.Id,
        StudentName = student.Person.Name.FullName(),
        GroupId = group.Id,
        GroupName = group.Name
      };
    }
  }
}
