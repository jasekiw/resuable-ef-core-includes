using System.Linq.Expressions;

namespace ReusableEfCoreIncludes;

internal static class ExpressionUtil
{
    public static Expression<Func<T, TProperty>> BuildExpression<T, TProperty>(string paramName, string propertyName)
    {
        ParameterExpression argParam = Expression.Parameter(typeof(T), paramName);
        Expression propertyExpression = Expression.Property(argParam, propertyName);
        return Expression.Lambda<Func<T, TProperty>>(propertyExpression, argParam);
    }

    public static Expression<Func<TParam, TProperty>> ConvertExpressionParamType<TParam, ToldParam, TProperty>(
        Expression<Func<ToldParam, TProperty>> source) => 
        BuildExpression<TParam, TProperty>(
            source.Parameters.FirstOrDefault()?.Name ?? 
            throw new ArgumentException("expression must have one parameter", nameof(source)),
            source.Body is MemberExpression me ? me.Member.Name : throw new ArgumentException(nameof(source.Body))
        );
}