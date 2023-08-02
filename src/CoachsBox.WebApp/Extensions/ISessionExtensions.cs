using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoachsBox.WebApp.Pages.Shared;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace CoachsBox.WebApp.Extensions
{
  public static class ISessionExtensions
  {
    public static void Put<T>(this ISession session, T value) where T : class
    {
      if (session == null)
        throw new ArgumentNullException(nameof(session));

      var key = GetSessionKey<T>();

      if (value != null)
        session.SetString(key, JsonConvert.SerializeObject(value));
      else if (session.Keys.Contains(key))
        session.Remove(key);
    }

    public static T Peek<T>(this ISession session) where T : class
    {
      if (session == null)
        throw new ArgumentNullException(nameof(session));

      var key = GetSessionKey<T>();

      var result = session.Keys.Contains(key) ?
        JsonConvert.DeserializeObject<T>(session.GetString(key)) :
        null;
      session.Remove(key);
      return result;
    }

    private static string GetSessionKey<T>()
    {
      return typeof(T).FullName;
    }
  }
}
