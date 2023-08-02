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
  public class UpdateApplicationCommandHandler : IRequestHandler<UpdateApplicationCommand, bool>
  {
    private readonly IStudentDocumentRepository studentDocumentRepository;

    public UpdateApplicationCommandHandler(IStudentDocumentRepository studentDocumentRepository)
    {
      this.studentDocumentRepository = studentDocumentRepository;
    }

    public async Task<bool> Handle(UpdateApplicationCommand request, CancellationToken cancellationToken)
    {
      if (request == null)
        return false;

      var date = Date.Create(request.Date);

      var application = await this.studentDocumentRepository.GetByIdAsync(request.ApplicationId) as StudentDocumentModel.Application;
      application.CorrectDate(date);

      await this.studentDocumentRepository.UpdateAsync(application);
      await this.studentDocumentRepository.SaveAsync();
      return true;
    }
  }
}
