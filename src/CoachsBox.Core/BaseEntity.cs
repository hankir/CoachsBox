using System;
using System.Collections.Generic;
using System.Text;
using MediatR;

namespace CoachsBox.Core
{
  public class BaseEntity
  {
    private int? requestedHashCode;

    private List<INotification> domainEvents;

    public int Id { get; set; }

    public IReadOnlyCollection<INotification> DomainEvents => domainEvents?.AsReadOnly();

    public void AddDomainEvent(INotification eventItem)
    {
      this.domainEvents = this.domainEvents ?? new List<INotification>();
      this.domainEvents.Add(eventItem);
    }

    public void RemoveDomainEvent(INotification eventItem)
    {
      this.domainEvents?.Remove(eventItem);
    }

    public void ClearDomainEvents()
    {
      this.domainEvents?.Clear();
    }

    public bool IsTransient()
    {
      return this.Id == default;
    }

    public override bool Equals(object obj)
    {
      if (obj == null || !(obj is BaseEntity))
        return false;

      if (Object.ReferenceEquals(this, obj))
        return true;

      if (this.GetType() != obj.GetType())
        return false;

      BaseEntity item = (BaseEntity)obj;

      if (item.IsTransient() || this.IsTransient())
        return false;
      else
        return item.Id == this.Id;
    }

    public override int GetHashCode()
    {
      if (!IsTransient())
      {
        if (!requestedHashCode.HasValue)
          requestedHashCode = this.Id.GetHashCode() ^ 31; // XOR for random distribution (http://blogs.msdn.com/b/ericlippert/archive/2011/02/28/guidelines-and-rules-for-gethashcode.aspx)

        return requestedHashCode.Value;
      }
      else
        return base.GetHashCode();

    }
    public static bool operator ==(BaseEntity left, BaseEntity right)
    {
      if (Object.Equals(left, null))
        return (Object.Equals(right, null)) ? true : false;
      else
        return left.Equals(right);
    }

    public static bool operator !=(BaseEntity left, BaseEntity right)
    {
      return !(left == right);
    }
  }
}