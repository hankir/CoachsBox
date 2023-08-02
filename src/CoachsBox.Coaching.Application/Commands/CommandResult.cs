using System;
using System.Collections.Generic;
using System.Text;
using CoachsBox.Core;

namespace CoachsBox.Coaching.Application.Commands
{
  public class CommandResult : ValueObject
  {
    protected CommandResult(string errorCode)
    {
      this.ErrorCode = errorCode;
    }

    public static CommandResult Success() => new CommandResult();

    public static CommandResult RequestNullError() => new CommandResult("REQUEST_NULL");

    public static CommandResult Fail(string errorCode) => new CommandResult(errorCode);

    public string ErrorCode { get; private set; }

    public bool IsSuccess() => string.IsNullOrWhiteSpace(this.ErrorCode);

    protected override IEnumerable<object> GetAtomicValues()
    {
      yield return this.ErrorCode;
    }

    protected CommandResult()
    {
    }
  }

  public class NewEntityCommandResult : CommandResult
  {
    protected NewEntityCommandResult(int newEntityId)
    {
      this.NewEntityId = newEntityId;
    }

    public static NewEntityCommandResult Success(int newEntityId) => new NewEntityCommandResult(newEntityId);

    public int NewEntityId { get; private set; }
  }
}
