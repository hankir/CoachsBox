using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using CoachsBox.Core.Interfaces;

namespace CoachsBox.Core
{
  public abstract class BaseSpecification<T> : ISpecification<T>
  {
    protected BaseSpecification(Expression<Func<T, bool>> criteria)
    {
      Criteria = criteria;
    }

    public Expression<Func<T, bool>> Criteria { get; }

    public List<Expression<Func<T, object>>> Includes { get; } = new List<Expression<Func<T, object>>>();

    public List<string> IncludeStrings { get; } = new List<string>();

    public List<Expression<Func<T, object>>> OrderBy { get; } = new List<Expression<Func<T, object>>>();

    public List<Expression<Func<T, object>>> OrderByDescending { get; } = new List<Expression<Func<T, object>>>();

    public Expression<Func<T, object>> GroupBy { get; private set; }

    public int Take { get; private set; }

    public int Skip { get; private set; }

    public bool isPagingEnabled { get; private set; } = false;

    public bool IsReadOnly { get; private set; } = false;

    public ISpecification<T> AsReadOnly()
    {
      this.IsReadOnly = true;
      return this;
    }

    protected virtual void AddInclude(Expression<Func<T, object>> includeExpression)
    {
      Includes.Add(includeExpression);
    }

    protected virtual void AddInclude(string includeString)
    {
      IncludeStrings.Add(includeString);
    }

    protected virtual void ApplyPaging(int skip, int take)
    {
      Skip = skip;
      Take = take;
      isPagingEnabled = true;
    }

    protected virtual void ApplyOrderBy(Expression<Func<T, object>> orderByExpression)
    {
      this.OrderBy.Add(orderByExpression);
    }

    protected virtual void ApplyOrderByDescending(Expression<Func<T, object>> orderByDescendingExpression)
    {
      this.OrderByDescending.Add(orderByDescendingExpression);
    }

    //Not used anywhere at the moment, but someone requested an example of setting this up.
    protected virtual void ApplyGroupBy(Expression<Func<T, object>> groupByExpression)
    {
      GroupBy = groupByExpression;
    }
  }
}
