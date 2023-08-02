using System;
using System.Collections.Generic;
using System.Linq;
using CoachsBox.Core;

namespace CoachsBox.Coaching.Accounting.StudentAccountingEventModel
{
  /// <summary>
  /// Спецификация истории платежей.
  /// </summary>
  public class PaymentHistorySpecification : BaseSpecification<PaymentAccountingEvent>
  {
    /// <summary>
    /// Создать спецификацию историю платежей.
    /// </summary>
    /// <param name="count">Количество платежей.</param>
    public PaymentHistorySpecification(int count)
      : base(payment => true)
    {
      this.ApplyPaging(0, count);
      this.ApplyOrderByDescending(payment => payment.WhenOccured);
      this.AddInclude(payment => payment.Account);
    }

    /// <summary>
    /// Создать спецификацию историю платежей.
    /// </summary>
    /// <param name="count">Количество платежей.</param>
    /// <param name="isProcessed">Состояние обработки платежа.</param>
    public PaymentHistorySpecification(int count, bool isProcessed)
      : base(payment => payment.ProcessingState.IsProcessed == isProcessed)
    {
      this.ApplyPaging(0, count);
      this.ApplyOrderByDescending(payment => payment.WhenOccured);
      this.AddInclude(payment => payment.Account);
    }

    /// <summary>
    /// Создать спецификацию историю платежей.
    /// </summary>
    /// <param name="count">Количество платежей.</param>
    /// <param name="from">Дата, с которой были платежи.</param>
    /// <param name="to">Дата, до которой были платежи.</param>
    public PaymentHistorySpecification(int count, DateTime from, DateTime to)
      : base(payment => payment.WhenOccured >= from && payment.WhenOccured <= to)
    {
      this.ApplyPaging(0, count);
      this.ApplyOrderByDescending(payment => payment.WhenOccured);
      this.AddInclude(payment => payment.Account);
    }

    /// <summary>
    /// Создать спецификацию историю платежей.
    /// </summary>
    /// <param name="count">Количество платежей.</param>
    /// <param name="from">Дата, с которой были платежи.</param>
    /// <param name="to">Дата, до которой были платежи.</param>
    /// <param name="studentIds">Идентификаторы студентов, с которыми связаны платежи.</param>
    public PaymentHistorySpecification(int count, DateTime from, DateTime to, IEnumerable<int> studentIds)
      : base(payment => payment.WhenOccured >= from && payment.WhenOccured <= to && studentIds.Contains(payment.Account.StudentId))
    {
      this.ApplyPaging(0, count);
      this.ApplyOrderByDescending(payment => payment.WhenOccured);
      this.AddInclude(payment => payment.Account);
    }

    /// <summary>
    /// Создать спецификацию историю платежей.
    /// </summary>
    /// <param name="count">Количество платежей.</param>
    /// <param name="isProcessed">Состояние обработки платежа.</param>
    /// <param name="from">Дата, с которой были платежи.</param>
    /// <param name="to">Дата, до которой были платежи.</param>
    public PaymentHistorySpecification(int count, bool isProcessed, DateTime from, DateTime to)
      : base(payment => payment.ProcessingState.IsProcessed == isProcessed && payment.WhenOccured >= from && payment.WhenOccured <= to)
    {
      this.ApplyPaging(0, count);
      this.ApplyOrderByDescending(payment => payment.WhenOccured);
      this.AddInclude(payment => payment.Account);
    }
  }
}
