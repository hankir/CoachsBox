using System.Threading;
using System.Threading.Tasks;
using CoachsBox.Coaching.StudentModel;
using MediatR;

namespace CoachsBox.Coaching.Application.Commands
{
  public class UpdateStudentNoteCommandHandler : IRequestHandler<UpdateStudentNoteCommand, string>
  {
    private readonly IStudentRepository studentRepository;

    public UpdateStudentNoteCommandHandler(IStudentRepository studentRepository)
    {
      this.studentRepository = studentRepository;
    }

    public async Task<string> Handle(UpdateStudentNoteCommand request, CancellationToken cancellationToken)
    {
      if (request == null)
        return string.Empty;

      var studentSpecification = new StudentByIdSpecification(request.StudentId);
      var student = await this.studentRepository.GetBySpecAsync(studentSpecification);
      student.ChangeNote(request.Note);
      await this.studentRepository.SaveAsync();
      return student.Note;
    }
  }
}
