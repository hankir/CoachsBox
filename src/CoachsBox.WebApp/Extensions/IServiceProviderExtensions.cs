using System;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;

namespace CoachsBox.WebApp.Extensions
{
  public static class IServiceProviderExtensions
  {
    public static bool TryGetCachedData<T>(this IServiceProvider serviceProvider, string cacheKey, out T data)
    {
      var memoryCache = serviceProvider.GetRequiredService<IMemoryCache>();
      return memoryCache.TryGetValue(cacheKey, out data);
    }

    public static void SetCacheData<T>(this IServiceProvider serviceProvider, string cacheKey, T value, TimeSpan lifetime)
    {
      var memoryCache = serviceProvider.GetRequiredService<IMemoryCache>();
      memoryCache.Set(cacheKey, value, new MemoryCacheEntryOptions() { AbsoluteExpirationRelativeToNow = lifetime });
    }
  }
}
