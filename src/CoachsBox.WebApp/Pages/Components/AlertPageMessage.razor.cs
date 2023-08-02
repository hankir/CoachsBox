using System;
using CoachsBox.WebApp.Extensions;
using CoachsBox.WebApp.Pages.Shared;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Http;

namespace CoachsBox.WebApp.Pages.Components
{
  public partial class AlertPageMessage : OwningComponentBase<IHttpContextAccessor>
  {
    private string messageCacheKey = Guid.NewGuid().ToString();

    private PageMessage displayMessage;

    [Parameter]
    public PageMessage Message { get; set; }

    protected override void OnInitialized()
    {
      if (this.Message == null)
      {
        var cacheKey = this.messageCacheKey;
        if (!this.ScopedServices.TryGetCachedData(cacheKey, out this.displayMessage))
        {
          this.displayMessage = this.Service.HttpContext.Session.Peek<PageMessage>();
          if (this.displayMessage != null)
            this.ScopedServices.SetCacheData(cacheKey, this.displayMessage, TimeSpan.FromSeconds(5));
        }
      }
      else
        this.displayMessage = this.Message;
    }
  }
}
