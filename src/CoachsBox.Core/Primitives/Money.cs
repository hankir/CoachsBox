using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace CoachsBox.Core.Primitives
{
  /// <summary>
  /// Представление денег в определенной валюте.
  /// </summary>
  [DebuggerDisplay("{Quantity} {Currency}")]
  public class Money : ValueObject
  {
    public static Money CreateRuble(decimal quantity)
    {
      return new Money()
      {
        Currency = ISOCurrency.RUB,
        Quantity = quantity
      };
    }

    public decimal Quantity { get; private set; }

    public ISOCurrency Currency { get; private set; }

    public bool IsNegative() => this.Quantity < 0;

    public Money Negate()
    {
      return new Money()
      {
        Currency = this.Currency,
        Quantity = decimal.Negate(this.Quantity)
      };
    }

    protected override IEnumerable<object> GetAtomicValues()
    {
      yield return this.Quantity;
      yield return this.Currency;
    }

    public Money Add(Money money)
    {
      if (money.Currency != this.Currency)
        throw new ArgumentException("Currency should be equal", nameof(money));

      return new Money()
      {
        Currency = this.Currency,
        Quantity = decimal.Add(this.Quantity, money.Quantity)
      };
    }

    public Money Substract(Money money)
    {
      if (money.Currency != this.Currency)
        throw new ArgumentException("Currency should be equal", nameof(money));

      return new Money()
      {
        Currency = this.Currency,
        Quantity = decimal.Subtract(this.Quantity, money.Quantity)
      };
    }

    private Money()
    {
      // Требует Entity framework core
    }
  }
}
