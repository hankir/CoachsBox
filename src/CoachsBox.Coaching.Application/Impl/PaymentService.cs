using System.Diagnostics;
using System.Linq;
using CoachsBox.Coaching.Accounting.StudentAccountingEventModel;
using CoachsBox.Core.Interfaces;
using Microsoft.Extensions.Logging;

namespace CoachsBox.Coaching.Application.Impl
{
  public class PaymentService : IPaymentService
  {
    private readonly ILogger<PaymentService> logger;
    private readonly IUnitOfWork unitOfWork;
    private readonly IStudentAccountingEventRepository eventRepository;

    public PaymentService(
      ILogger<PaymentService> logger,
      IUnitOfWork unitOfWork,
      IStudentAccountingEventRepository eventRepository)
    {
      this.logger = logger;
      this.unitOfWork = unitOfWork;
      this.eventRepository = eventRepository;
    }

    public void ProcessPayments()
    {
      this.logger.LogInformation("Processing payments starting");
      var time = Stopwatch.StartNew();

      var previoslyUnprocessedPaymentsSpec = new PreviouslyUnprocessedPaymentsSpecification(500);
      var unprocessedPayments = this.eventRepository.ListAsync(previoslyUnprocessedPaymentsSpec).Result;

      foreach (var payment in unprocessedPayments)
        payment.Process();

      if (unprocessedPayments.Any())
        this.unitOfWork.Save();

      time.Stop();
      this.logger.LogInformation("Processing payments finished at {Elapsed}", time.Elapsed);
    }
  }
}
