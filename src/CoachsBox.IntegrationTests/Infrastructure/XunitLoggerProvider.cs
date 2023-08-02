using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Logging;
using Xunit.Abstractions;

namespace CoachsBox.IntegrationTests.Infrastructure
{
  public class XunitLoggerProvider : ILoggerProvider
  {
    private readonly ITestOutputHelper testOutputHelper;

    public XunitLoggerProvider(ITestOutputHelper testOutputHelper)
    {
      this.testOutputHelper = testOutputHelper;
    }

    public ILogger CreateLogger(string categoryName) => new XunitLogger(testOutputHelper, categoryName);

    public void Dispose() { }
  }
}
