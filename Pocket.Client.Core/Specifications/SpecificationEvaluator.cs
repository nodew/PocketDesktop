using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Pocket.Client.Core.Contracts.Specifications;
using Pocket.Client.Core.Models;

namespace Pocket.Client.Core.Specifications;

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
        else if (specification.OrderBy != null)
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