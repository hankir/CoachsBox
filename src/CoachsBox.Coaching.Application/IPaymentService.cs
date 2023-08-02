namespace CoachsBox.Coaching.Application
{
  public interface IPaymentService
  {
    /// <summary>
    /// Обработать поступившие платежи.
    /// </summary>
    void ProcessPayments();
  }
}
