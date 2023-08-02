using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using CoachsBox.Core.Interfaces;

namespace CoachsBox.Core
{
  public sealed class PaginalSpecification<T> : ISpecification<T>
  {
    private readonly ISpecification<T> specification;

    public PaginalSpecification(ISpecification<T> specification)
    {
      this.specification = specification ?? throw new ArgumentNullException(nameof(specification));
    }

    public Expression<Func<T, bool>> Criteria => this.specification.Criteria;

    public List<Expression<Func<T, object>>> Includes => this.specification.Includes;

    public List<string> IncludeStrings => this.specification.IncludeStrings;

    public List<Expression<Func<T, object>>> OrderBy => this.specification.OrderBy;

    public List<Expression<Func<T, object>>> OrderByDescending => this.specification.OrderByDescending;

    public Expression<Func<T, object>> GroupBy => this.specification.GroupBy;

    public int Take { get; private set; }

    public int Skip { get; private set; }

    public bool isPagingEnabled { get; private set; }

    public bool IsReadOnly => this.specification.IsReadOnly;

    public void ApplyPaging(int skip, int take)
    {
      Skip = skip;
      Take = take;
      isPagingEnabled = true;
    }
  }
}
