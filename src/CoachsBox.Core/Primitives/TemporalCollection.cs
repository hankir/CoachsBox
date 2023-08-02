using System;
using System.Collections.Generic;

namespace CoachsBox.Core.Primitives
{
  public class TemporalCollection<T>
  {
    private readonly SortedList<DateTime, T> valuesMap;

    public TemporalCollection()
    {
      this.valuesMap = new SortedList<DateTime, T>();
    }

    public TemporalCollection(IComparer<DateTime> comparer)
    {
      this.valuesMap = new SortedList<DateTime, T>(comparer);
    }

    public T Get(DateTime dateTime)
    {
      foreach (var milestone in this.valuesMap)
      {
        if (milestone.Key < dateTime || milestone.Key == dateTime)
          return milestone.Value;
      }
      throw new ArgumentException("No records that early");
    }

    public void Add(DateTime dateTime, T value)
    {
      this.valuesMap.TryAdd(dateTime, value);
    }
  }
}
