using Microsoft.EntityFrameworkCore;
using PocketClient.Core.Contracts.Specifications;
using PocketClient.Core.Models;

namespace PocketClient.Core.Specifications;

public class SpecificationEvaluator<TEntity> where TEntity : Entity
{
    public static IQueryable<TEntity> GetQuery(IQueryable<TEntity> query, IBaseSpecification<TEntity> specification)
    {
        // Do not apply anything if specifications is null
        if (specification == null)
        {
            return query;
        }

        query = specification.Includes
                    .Aggregate(query, (current, include) => current.Include(include));

        if (specification.FilterCondition != null)
        {
            query = query.Where(specification.FilterCondition);
        }

        if (specification.OrderBy != null)
        {
            query = query.OrderBy(specification.OrderBy);
        }
        else if (specification.OrderByDescending != null)
        {
            query = query.OrderByDescending(specification.OrderByDescending);
        }

        return query;
    }
}