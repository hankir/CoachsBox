using System.Threading;
using System.Threading.Tasks;
using CoachsBox.Coaching.Accounting.StudentAccountModel;
using CoachsBox.Coaching.StudentModel;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CoachsBox.Coaching.Application.DomainEventHandlers
{
  public class CreatedStudentEventHandler : INotificationHandler<CreatedStudentEvent>
  {
    private readonly IStudentAccountRepository studentAccountRepository;
    private readonly ILogger<CreatedStudentEventHandler> logger;

    public CreatedStudentEventHandler(IStudentAccountRepository studentAccountRepository, ILogger<CreatedStudentEventHandler> logger)
    {
      this.studentAccountRepository = studentAccountRepository;
      this.logger = logger;
    }

    public async Task Handle(CreatedStudentEvent notification, CancellationToken cancellationToken)
    {
      if (notification == null)
        return;

      var studentId = notification.Student.Id;
      var studentAccountSpecification = new StudentAccountSpecification(studentId);
      if (await this.studentAccountRepository.CountAsync(studentAccountSpecification) == 0)
      {
        var account = new StudentAccount(studentId);
        await this.studentAccountRepository.AddAsync(account);
        await this.studentAccountRepository.SaveAsync();
        this.logger.LogDebug("Student account for student with id:{StudentId} created", studentId);
      }
    }
  }
}
