using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Pocket.Client.Core.Contracts.Specifications;

namespace Pocket.Client.Core.Specifications;

public class BaseSpecification<T> : IBaseSpecification<T>
{
    private readonly List<Expression<Func<T, object>>> _includeCollection = new();

    private Expression<Func<T, bool>>? _filterCondition;
    private Expression<Func<T, object>>? _orderBy;
    private Expression<Func<T, object>>? _orderByDescending;

    public BaseSpecification()
    {
        
    }
    
    public BaseSpecification(Expression<Func<T, bool>> filterCondition)
    {
        _filterCondition = filterCondition;
    }

    public bool IsSatisfiedBy(T entity)
    {
        if (FilterCondition == null)
        {
            return true;
        }
        
        var predicate = FilterCondition.Compile();
        return predicate(entity);
    }

    public BaseSpecification<T> And(BaseSpecification<T> specification)
    {
        return new AndSpecification<T>(this, specification);
    }

    public BaseSpecification<T> Or(BaseSpecification<T> specification)
    {
        return new OrSpecification<T>(this, specification);
    }
    
    public virtual Expression<Func<T, bool>>? FilterCondition => _filterCondition;

    public Expression<Func<T, object>>? OrderBy => _orderBy;

    public Expression<Func<T, object>>? OrderByDescending => _orderByDescending;

    public List<Expression<Func<T, object>>> Includes => _includeCollection;

    public void AddInclude(Expression<Func<T, object>> includeExpression)
    {
        Includes.Add(includeExpression);
    }

    public void ApplyOrderBy(Expression<Func<T, object>> orderByExpression)
    {
        _orderBy = orderByExpression;
    }

    public void ApplyOrderByDescending(Expression<Func<T, object>> orderByDescendingExpression)
    {
        _orderByDescending = orderByDescendingExpression;
    }

    public void SetFilterCondition(Expression<Func<T, bool>> filterExpression)
    {
        _filterCondition = filterExpression;
    }
}