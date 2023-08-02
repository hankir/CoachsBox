using System.Threading;
using System.Threading.Tasks;
using CoachsBox.Coaching.StudentDocumentModel;
using CoachsBox.Core.Primitives;
using MediatR;

namespace CoachsBox.Coaching.Application.Commands
{
  /// <summary>
  /// Обработчик команды добавления заявления студента.
  /// </summary>
  public class AddApplicationCommandHandler : IRequestHandler<AddApplicationCommand, bool>
  {
    private readonly IStudentDocumentRepository studentDocumentRepository;

    public AddApplicationCommandHandler(IStudentDocumentRepository studentDocumentRepository)
    {
      this.studentDocumentRepository = studentDocumentRepository;
    }

    public async Task<bool> Handle(AddApplicationCommand request, CancellationToken cancellationToken)
    {
      if (request == null)
        return false;

      var studentId = request.StudentId;
      var date = Date.Create(request.Date);

      var application = new StudentDocumentModel.Application(studentId, date);

      await this.studentDocumentRepository.AddAsync(application);
      await this.studentDocumentRepository.SaveAsync();
      return true;
    }
  }
}
