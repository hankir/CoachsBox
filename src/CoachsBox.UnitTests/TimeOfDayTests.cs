using System;
using CoachsBox.Core.Primitives;
using Xunit;

namespace CoachsBox.UnitTests
{
  public class TimeOfDayTests
  {
    [Fact]
    public void CreateFrom0To23Hours()
    {
      for (byte hours = 0; hours < 24; hours++)
      {
        var testValue = new TimeOfDay(hours, 0);
        Assert.Equal(hours, testValue.Hours);
        Assert.Equal(0, testValue.Minutes);
      }
    }

    [Fact]
    public void CreateFrom0To59Minutes()
    {
      for (byte minutes = 0; minutes < 60; minutes++)
      {
        var testValue = new TimeOfDay(0, minutes);
        Assert.Equal(0, testValue.Hours);
        Assert.Equal(minutes, testValue.Minutes);
      }
    }

    [Fact]
    public void CreateFrom0To23HoursAnd0To59Minutes()
    {
      for (byte hours = 0; hours < 23; hours++)
      {
        for (byte minutes = 0; minutes < 60; minutes++)
        {
          var testValue = new TimeOfDay(hours, minutes);
          Assert.Equal(hours, testValue.Hours);
          Assert.Equal(minutes, testValue.Minutes);
        }
      }
    }

    [Fact]
    public void CreateGreaterThan23HoursThrowException()
    {
      for (byte hours = 24; hours < 30; hours++)
      {
        var exception = Assert.Throws<ArgumentOutOfRangeException>(() =>
        {
          var testValue = new TimeOfDay(hours, 0);
        });
        Assert.NotNull(exception);
        Assert.Equal(hours, exception.ActualValue);
      }
    }

    [Fact]
    public void CreateGreaterThan59MinutesThrowException()
    {
      for (byte minutes = 60; minutes < 90; minutes++)
      {
        var exception = Assert.Throws<ArgumentOutOfRangeException>(() =>
        {
          var testValue = new TimeOfDay(0, minutes);
        });
        Assert.NotNull(exception);
        Assert.Equal(minutes, exception.ActualValue);
      }
    }

    [Fact]
    public void CreateGreaterThan23HoursAnd59MinutesThrowException()
    {
      for (byte hours = 24; hours < 30; hours++)
      {
        for (byte minutes = 60; minutes < 90; minutes++)
        {
          var exception = Assert.Throws<ArgumentOutOfRangeException>(() =>
          {
            var testValue = new TimeOfDay(hours, minutes);
          });
          Assert.NotNull(exception);
          Assert.Equal(hours, exception.ActualValue);
        }
      }
    }

    [Fact]
    public void TimeOfDayEquals()
    {
      var a = new TimeOfDay(13, 30);
      var b = new TimeOfDay(13, 30);
      Assert.Equal(a, b);
      Assert.True(a >= b);
      Assert.True(a <= b);
      Assert.True(a != b);
    }

    [Fact]
    public void TimeOfDayNotEquals()
    {
      var a = new TimeOfDay(13, 35);
      var b = new TimeOfDay(13, 30);
      Assert.NotEqual(a, b);
      Assert.True(a != b);
      Assert.False(a == b);
    }

    [Fact]
    public void TimeOfDayReferenceEquals()
    {
      var a = new TimeOfDay(13, 35);
      var b = a;
      Assert.Equal(a, b);
      Assert.False(a != b);
      Assert.True(a == b);
    }

    [Fact]
    public void TimeOfDayReferenceNotEquals()
    {
      var a = new TimeOfDay(13, 35);
      var b = new TimeOfDay(13, 35);
      Assert.Equal(a, b);
      Assert.True(a != b);
      Assert.False(a == b);
    }

    [Fact]
    public void TimeOfDayGreaterOrLessThanOtherByHours()
    {
      var a = new TimeOfDay(14, 30);
      var b = new TimeOfDay(13, 30);
      Assert.True(a > b);
      Assert.True(b < a);
      Assert.True(a >= b);
      Assert.True(b <= a);
    }
  }
}
