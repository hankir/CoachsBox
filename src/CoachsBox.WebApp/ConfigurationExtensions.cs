using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace CoachsBox.WebApp
{
  public static class ConfigurationExtensions
  {
    public const string DaDataAPIKey = nameof(DaDataAPIKey);

    public static string GetDaDataAPIKey(this IConfiguration configuration)
    {
      return configuration.GetValue<string>(DaDataAPIKey);
    }
  }
}
