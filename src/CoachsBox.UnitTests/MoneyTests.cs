using System;
using CoachsBox.Core.Primitives;
using Xunit;

namespace CoachsBox.UnitTests
{
  public class MoneyTests
  {
    [Fact]
    public void CreateRuble()
    {
      var money = Money.CreateRuble(100);
      Assert.Equal(ISOCurrency.RUB, money.Currency);
      Assert.Equal(100, money.Quantity);
    }

    [Fact]
    public void ShouldBeCorrectAfterAdded()
    {
      var oneHundred = Money.CreateRuble(100);
      var threeHundred = Money.CreateRuble(300);
      var fourHundred = oneHundred.Add(threeHundred);
      Assert.Equal(400, fourHundred.Quantity);
    }

    [Fact]
    public void ShouldBeCorrectAfterSubstract()
    {
      var oneHundred = Money.CreateRuble(100);
      var threeHundred = Money.CreateRuble(300);
      var hundred = threeHundred.Substract(oneHundred);
      Assert.Equal(200, hundred.Quantity);
    }

    [Fact]
    public void ShouldBeNegativeAfterSubtractionResultLessThanZero()
    {
      var oneHundred = Money.CreateRuble(100);
      var threeHundred = Money.CreateRuble(300);
      var result = oneHundred.Substract(threeHundred);
      Assert.True(result.IsNegative());
      Assert.Equal(-200, result.Quantity);
    }
  }
}
