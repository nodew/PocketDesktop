using System.Linq.Expressions;

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
