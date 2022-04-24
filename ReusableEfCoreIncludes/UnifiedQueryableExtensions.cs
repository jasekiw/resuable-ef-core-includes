namespace ReusableEfCoreIncludes;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

public static class UnifiedQueryableExtensions
{
    public static IUnifiedQueryable<TEntity, TProperty> IncludeExpression<TEntity, TProperty>(
        this IUnifiedQueryable<TEntity, TEntity> source,
        Func<IUnifiedQueryable<TEntity, TEntity>, IUnifiedQueryable<TEntity, TProperty>> expression
    ) where TEntity : class => expression(source);

    private static UnifiedQueryable<T, P> DownCast<T, P>(IUnifiedQueryable<T, P> q) => q is UnifiedQueryable<T, P> uq
        ? uq
        : throw new ArgumentException("Should be UnifiedQueryable instance");


    public static IUnifiedQueryable<TEntity, TEntity> StartInclude<TEntity>(this IQueryable<TEntity> source)
        where TEntity : class => new UnifiedQueryable<TEntity, TEntity>(source);

    public static IUnifiedQueryable<TEntity, TProperty> ThenInclude<TEntity, TPreviousProperty, TProperty>(
        this IUnifiedQueryable<TEntity, TPreviousProperty> source,
        Expression<Func<TPreviousProperty, TProperty>> navigationPropertyPath)
        where TEntity : class
    {
        var query = DownCast(source);
        if (query.IncludableQueryable != null)
            return new UnifiedQueryable<TEntity, TProperty>(
                query.IncludableQueryable.ThenInclude(navigationPropertyPath));
        if (query.IncludableManyQueryable != null)
            return new UnifiedQueryable<TEntity, TProperty>(
                query.IncludableManyQueryable.ThenInclude(navigationPropertyPath));
        if (query.Queryable != null)
        {
            var newExpression =
                ConvertExpressionParamType<TEntity, TPreviousProperty, TProperty>(navigationPropertyPath);
            return new UnifiedQueryable<TEntity, TProperty>(query.Queryable.Include(newExpression));
        }

        throw new InvalidOperationException();
    }

    public static Expression<Func<T, TProperty>> BuildExpression<T, TProperty>(string paramName, string propertyName)
    {
        ParameterExpression argParam = Expression.Parameter(typeof(T), paramName);
        Expression propertyExpression = Expression.Property(argParam, propertyName);
        return Expression.Lambda<Func<T, TProperty>>(propertyExpression, argParam);
    }

    public static Expression<Func<TParam, TProperty>> ConvertExpressionParamType<TParam, ToldParam, TProperty>(
        Expression<Func<ToldParam, TProperty>> source)
    {
        return BuildExpression<TParam, TProperty>(
            source.Parameters.First().Name,
            source.Body is MemberExpression me
                ? me.Member.Name
                : throw new ArgumentException(nameof(source.Body))
        );
    }

    public static IUnifiedQueryable<TEntity, TProperty> ThenIncludeMany<TEntity, TPreviousProperty, TProperty>(
        this IUnifiedQueryable<TEntity, TPreviousProperty> source,
        Expression<Func<TPreviousProperty, IEnumerable<TProperty>>> navigationPropertyPath)
        where TEntity : class
    {
        var query = DownCast(source);
        if (query.IncludableQueryable != null)
            return new UnifiedQueryable<TEntity, TProperty>(
                query.IncludableQueryable.ThenInclude(navigationPropertyPath));
        if (query.IncludableManyQueryable != null)
            return new UnifiedQueryable<TEntity, TProperty>(
                query.IncludableManyQueryable.ThenInclude(navigationPropertyPath));
        if (query.Queryable != null)
        {
            var newExpression =
                ConvertExpressionParamType<TEntity, TPreviousProperty, IEnumerable<TProperty>>(navigationPropertyPath);
            return new UnifiedQueryable<TEntity, TProperty>(query.Queryable.Include(newExpression));
        }

        throw new InvalidOperationException();
    }

  
    public static IUnifiedQueryable<TEntity, TProperty> ThenInclude<TEntity, TProperty>(
        this IUnifiedQueryable<TEntity, TEntity> source,
        Expression<Func<TEntity, TProperty>> navigationPropertyPath)
        where TEntity : class
    {
        var query = DownCast(source);
        if (query.Queryable != null)
            return new UnifiedQueryable<TEntity, TProperty>(query.Queryable.Include(navigationPropertyPath));
        if (query.IncludableQueryable != null)
            return new UnifiedQueryable<TEntity, TProperty>(
                query.IncludableQueryable.ThenInclude(navigationPropertyPath));
        if (query.IncludableManyQueryable != null)
            return new UnifiedQueryable<TEntity, TProperty>(
                query.IncludableManyQueryable.ThenInclude(navigationPropertyPath));
        throw new InvalidOperationException();
    }
    

    public static IUnifiedQueryable<TEntity, TEntity> OptionalThenInclude<TEntity, TPreviousProperty, TProperty>(
        this IUnifiedQueryable<TEntity, TPreviousProperty> source,
        Expression<Func<TPreviousProperty, TProperty>> navigationPropertyPath,
        bool performInclude)
        where TEntity : class => !performInclude
            ? (IUnifiedQueryable<TEntity, TEntity>)source
            : source.ThenInclude(navigationPropertyPath).ToBase();
    

    public static IUnifiedQueryable<TEntity, TProperty> IncludeMany<TEntity, TProperty>(
        this IUnifiedQueryable<TEntity, TEntity> source,
        Expression<Func<TEntity, IEnumerable<TProperty>>> navigationPropertyPath)
        where TEntity : class => 
        new UnifiedQueryable<TEntity, TProperty>(DownCast(source).EndInclude().Include(navigationPropertyPath));
    

    public static IUnifiedQueryable<TEntity, TEntity> ToBase<TEntity, TPreviousProperty>(
        this IUnifiedQueryable<TEntity, TPreviousProperty> source)
        where TEntity : class => new UnifiedQueryable<TEntity, TEntity>(source.EndInclude());
    
}