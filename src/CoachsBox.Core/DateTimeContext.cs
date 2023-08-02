using System;
using System.Collections.Generic;
using System.Text;

namespace CoachsBox.Core
{
  public sealed class DateTimeContext : IDisposable
  {
    private IDateTimeProvider originalDateTimeProvider;

    public DateTimeContext(IDateTimeProvider provider)
    {
      this.originalDateTimeProvider = Watch.CurrentProvider;
      Watch.CurrentProvider = provider;
    }

    public void Dispose()
    {
      Watch.CurrentProvider = this.originalDateTimeProvider;
    }
  }
}
