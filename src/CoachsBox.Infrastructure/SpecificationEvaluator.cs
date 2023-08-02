using CoachsBox.Core;
using CoachsBox.Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace CoachsBox.Infrastructure
{
  public class SpecificationEvaluator<T> where T : BaseEntity
  {
    public static IQueryable<T> GetQuery(IQueryable<T> inputQuery, ISpecification<T> specification)
    {
      var query = inputQuery;

      // modify the IQueryable using the specification's criteria expression
      if (specification.Criteria != null)
      {
        query = query.Where(specification.Criteria);
      }

      // Includes all expression-based includes
      query = specification.Includes.Aggregate(query,
                              (current, include) => current.Include(include));

      // Include any string-based include statements
      query = specification.IncludeStrings.Aggregate(query,
                              (current, include) => current.Include(include));

      // Apply ordering if expressions are set
      if (specification.OrderBy.Any())
      {
        query = query.OrderBy(specification.OrderBy.First());
        foreach (var thenByExpression in specification.OrderBy.Skip(1))
        {
          query = ((IOrderedQueryable<T>)query).ThenBy(thenByExpression);
        }
      }
      else if (specification.OrderByDescending.Any())
      {
        query = query.OrderByDescending(specification.OrderByDescending.First());
        foreach (var thenByExpression in specification.OrderByDescending.Skip(1))
        {
          query = ((IOrderedQueryable<T>)query).ThenByDescending(thenByExpression);
        }
      }

      if (specification.GroupBy != null)
      {
        query = query.GroupBy(specification.GroupBy).SelectMany(x => x);
      }

      // Apply paging if enabled
      if (specification.isPagingEnabled)
      {
        query = query.Skip(specification.Skip)
                     .Take(specification.Take);
      }
      return query;
    }
  }
}
