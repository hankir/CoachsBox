using System;

namespace CoachsBox.WebApp.Pages.Shared
{
  [Serializable]
  public class PageMessage
  {
    public PageMessageType MessageType { get; set; }

    public string Message { get; set; }

    public override string ToString()
    {
      return $"{this.MessageType}:{this.Message}";
    }
  }

  public enum PageMessageType
  {
    Success,
    Information,
    Error,
    Warning
  }
}
