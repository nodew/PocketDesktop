using System.Linq.Expressions;

namespace PocketClient.Core.Specifications;

public class OrSpecification<T> : BaseSpecification<T>
{
    private readonly BaseSpecification<T> _left;
    private readonly BaseSpecification<T> _right;

    public OrSpecification(BaseSpecification<T> left, BaseSpecification<T> right)
    {
        _right = right;
        _left = left;
    }

    public override Expression<Func<T, bool>>? FilterCondition => this.GetFilterExpression();

    public Expression<Func<T, bool>>? GetFilterExpression()
    {
        var leftExpression = _left.FilterCondition;
        var rightExpression = _right.FilterCondition;

        if (leftExpression == null && rightExpression == null)
        {
            return null;
        }

        if (leftExpression == null)
        {
            return rightExpression;
        }

        if (rightExpression == null)
        {
            return leftExpression;
        }

        var paramExpr = Expression.Parameter(typeof(T));
        var exprBody = Expression.OrElse(leftExpression.Body, rightExpression.Body);
        exprBody = (BinaryExpression)new ParameterReplacer(paramExpr).Visit(exprBody);
        var finalExpr = Expression.Lambda<Func<T, bool>>(exprBody, paramExpr);

        return finalExpr;
    }
}