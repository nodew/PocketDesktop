using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace PocketClient.Core.Contracts.Specifications;

public interface IBaseSpecification<T>
{
    Expression<Func<T, bool>>? FilterCondition
    {
        get;
    }

    Expression<Func<T, object>>? OrderBy
    {
        get;
    }

    Expression<Func<T, object>>? OrderByDescending
    {
        get;
    }

    List<Expression<Func<T, object>>> Includes
    {
        get;
    }
}
