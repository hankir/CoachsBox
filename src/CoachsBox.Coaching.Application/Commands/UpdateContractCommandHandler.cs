using System.Threading;
using System.Threading.Tasks;
using CoachsBox.Coaching.Accounting.StudentContractModel;
using CoachsBox.Coaching.StudentDocumentModel;
using CoachsBox.Core.Primitives;
using MediatR;

namespace CoachsBox.Coaching.Application.Commands
{
  /// <summary>
  /// Обработчик команды добавления заявления студента.
  /// </summary>
  public class UpdateContractCommandHandler : IRequestHandler<UpdateContractCommand, bool>
  {
    private readonly IStudentContractRepository studentContractRepository;

    public UpdateContractCommandHandler(IStudentContractRepository studentContractRepository)
    {
      this.studentContractRepository = studentContractRepository;
    }

    public async Task<bool> Handle(UpdateContractCommand request, CancellationToken cancellationToken)
    {
      if (request == null)
        return false;

      var date = Date.Create(request.Date);

      var contract = await this.studentContractRepository.GetByIdAsync(request.ContractId);

      contract.CorrectDate(date);
      contract.CorrectNumber(request.Number);

      await this.studentContractRepository.UpdateAsync(contract);
      await this.studentContractRepository.SaveAsync();
      return true;
    }
  }
}
