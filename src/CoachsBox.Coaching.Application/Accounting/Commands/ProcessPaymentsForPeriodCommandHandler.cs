using System;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CoachsBox.Coaching.Accounting.StudentAccountingEventModel;
using CoachsBox.Core.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CoachsBox.Coaching.Application.Accounting.Commands
{
  public class ProcessPaymentsForPeriodCommandHandler : IRequestHandler<ProcessPaymentsForPeriodCommand, bool>
  {
    private readonly ILogger<ProcessPaymentsForPeriodCommandHandler> logger;
    private readonly IUnitOfWork unitOfWork;
    private readonly IStudentAccountingEventRepository eventRepository;

    public ProcessPaymentsForPeriodCommandHandler(
      ILogger<ProcessPaymentsForPeriodCommandHandler> logger,
      IUnitOfWork unitOfWork,
      IStudentAccountingEventRepository eventRepository)
    {
      this.logger = logger;
      this.unitOfWork = unitOfWork;
      this.eventRepository = eventRepository;
    }

    public async Task<bool> Handle(ProcessPaymentsForPeriodCommand request, CancellationToken cancellationToken)
    {
      if (request == null)
        throw new ArgumentNullException(nameof(request));

      this.logger.LogInformation("Processing payments for period {PeriodBeginning}-{PeriodEnding} starting", request.PeriodBeginning, request.PeriodEnding);
      var time = Stopwatch.StartNew();

      var previoslyUnprocessedPaymentsSpec = new PreviouslyUnprocessedPaymentsSpecification(request.PeriodBeginning, request.PeriodEnding);
      var unprocessedPayments = await this.eventRepository.ListAsync(previoslyUnprocessedPaymentsSpec);

      foreach (var payment in unprocessedPayments)
        payment.Process();

      if (unprocessedPayments.Any())
        await this.unitOfWork.SaveAsync();

      time.Stop();
      this.logger.LogInformation("Processing payments for period {PeriodBegining}-{PeriodEnding} finished at {Elapsed}", time.Elapsed, request.PeriodBeginning, request.PeriodEnding);

      return true;
    }
  }
}
