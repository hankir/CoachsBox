using System.Threading;
using System.Threading.Tasks;
using CoachsBox.Coaching.Accounting.StudentContractModel;
using CoachsBox.Core.Primitives;
using MediatR;

namespace CoachsBox.Coaching.Application.Commands
{
  /// <summary>
  /// Обработчик команды добавления заявления студента.
  /// </summary>
  public class AddContractCommandHandler : IRequestHandler<AddContractCommand, bool>
  {
    private readonly IStudentContractRepository studentContractRepository;

    public AddContractCommandHandler(IStudentContractRepository studentContractRepository)
    {
      this.studentContractRepository = studentContractRepository;
    }

    public async Task<bool> Handle(AddContractCommand request, CancellationToken cancellationToken)
    {
      if (request == null)
        return false;

      var studentId = request.StudentId;
      var date = Date.Create(request.Date);
      var number = request.Number;

      var contract = new StudentContract(studentId, date, number);

      await this.studentContractRepository.AddAsync(contract);
      await this.studentContractRepository.SaveAsync();
      return true;
    }
  }
}
