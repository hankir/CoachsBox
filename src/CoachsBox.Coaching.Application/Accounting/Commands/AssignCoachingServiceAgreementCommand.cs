using System;
using System.Collections.Generic;
using System.Text;
using MediatR;

namespace CoachsBox.Coaching.Application.Accounting.Commands
{
  public class AssignCoachingServiceAgreementCommand : IRequest<bool>
  {
    public AssignCoachingServiceAgreementCommand(int serviceAgreementId, int groupId)
    {
      this.AgreementId = serviceAgreementId;
      this.GroupId = groupId;
    }

    public int AgreementId { get; }
    public int GroupId { get; }
  }
}
