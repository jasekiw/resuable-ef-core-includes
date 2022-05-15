using Microsoft.EntityFrameworkCore.Query;

namespace ReusableEfCoreIncludes;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

public delegate IUIncludable<T, TProperty> Include<T, out TProperty>(IUIncludable<T> arg) where T : class;
public delegate IUIncludable<T, TProperty> ThenInclude<T, TPrevProp, out TProperty>(IUIncludable<T, TPrevProp> arg) where T : class;
public delegate IUIncludable<T, TProperty> IncludeBase<T, out TProperty>(IUIncludable<T, T> arg) where T : class;


public static class UnifiedQueryableExtensions
{
    
    public static IUIncludable<T, P> IncludeExpression<T, P>(this IUIncludable<T> source, Include<T, P> expression) 
        where T : class => expression(source);
    
    private static IUIncludeInternals<T, P> DownCast<T, P>(IUIncludable<T, P> q) => 
        q as IUIncludeInternals<T, P> ?? throw new ArgumentException("Should be UnifiedQueryable instance");

    private static IUIncludeInternals<T, T> DownCast<T>(IUIncludable<T, T> q) => 
        q as IUIncludeInternals<T, T> ?? throw new ArgumentException("Should be UnifiedQueryable instance");
    
    private static IUIncludeInternals<T, T> DownCast<T>(IUIncludable<T> q) => 
        q as IUIncludeInternals<T, T> ?? throw new ArgumentException("Should be UnifiedQueryable instance");
    
    
    public static IUIncludable<TEntity> BeginInclude<TEntity>(this IQueryable<TEntity> source) where TEntity : class => 
        new IUInclude<TEntity, TEntity>(source);

    public static Expression<Func<T, TProperty>> BuildExpression<T, TProperty>(string paramName, string propertyName)
    {
        ParameterExpression argParam = Expression.Parameter(typeof(T), paramName);
        Expression propertyExpression = Expression.Property(argParam, propertyName);
        return Expression.Lambda<Func<T, TProperty>>(propertyExpression, argParam);
    }

    public static Expression<Func<TParam, TProperty>> ConvertExpressionParamType<TParam, ToldParam, TProperty>(
        Expression<Func<ToldParam, TProperty>> source) => 
        BuildExpression<TParam, TProperty>(
            source.Parameters.First().Name,
            source.Body is MemberExpression me
                ? me.Member.Name
                : throw new ArgumentException(nameof(source.Body))
        );
    

    public static IUIncludable<T, TProp> ThenIncludeMany<T, TPrevProp, TProp>(
        this IUIncludable<T, TPrevProp> source,
        Expression<Func<TPrevProp, IEnumerable<TProp>>> navigationPropertyPath)
        where T : class
    {
        var query = DownCast(source);
        if (query.IncludableQueryable != null)
            return Factory(query.IncludableQueryable.ThenInclude(navigationPropertyPath));
        if (query.IncludableManyQueryable != null)
            return Factory(query.IncludableManyQueryable.ThenInclude(navigationPropertyPath));
        if (query.Queryable != null)
            return Factory(query.Queryable.Include(ConvertExpressionParamType<T, TPrevProp, IEnumerable<TProp>>(navigationPropertyPath)));
        throw new InvalidOperationException();
    }
    
    public static IUIncludable<T, TProp> IncludeMany<T, TProp>(
        this IUIncludable<T> source,
        Expression<Func<T, IEnumerable<TProp>>> navigationPropertyPath)
        where T : class
    {
        var query = DownCast(source);
        if (query.IncludableQueryable != null)
            return Factory(query.IncludableQueryable.Include(navigationPropertyPath));
        if (query.IncludableManyQueryable != null)
            return Factory(query.IncludableManyQueryable.Include(navigationPropertyPath));
        if (query.Queryable != null)
            return Factory(query.Queryable.Include(navigationPropertyPath));
        throw new InvalidOperationException();
    }
    
    public static IUIncludable<T, TProp> IncludeManyFrom<T, TPrevProp, TProp>(
        this IUIncludable<T> source,
        Include<T, TPrevProp> expression,
        Expression<Func<TPrevProp, IEnumerable<TProp>>> navigationPropertyPath)
        where T : class
    {
        var query = DownCast(expression(source));
        if (query.IncludableQueryable != null)
            return Factory(query.IncludableQueryable.ThenInclude(navigationPropertyPath));
        if (query.IncludableManyQueryable != null)
            return Factory(query.IncludableManyQueryable.ThenInclude(navigationPropertyPath));
        if (query.Queryable != null)
            return Factory(query.Queryable.Include(ConvertExpressionParamType<T, TPrevProp, IEnumerable<TProp>>(navigationPropertyPath)));
        throw new InvalidOperationException();
    }

    private static IUIncludable<T, P> Factory<T, P>(IIncludableQueryable<T, P> include) => new IUInclude<T, P>(include);
    private static IUIncludable<T, P> Factory<T, P>(IIncludableQueryable<T, IEnumerable<P>> include) => new IUInclude<T, P>(include);
    
    public static IUIncludable<T, P> Include<T, P>(this IUIncludable<T> source, Expression<Func<T, P>> navigationPropertyPath) where T : class
    {
        var query = DownCast(source);
        if (query.Queryable != null)
            return Factory(query.Queryable.Include(navigationPropertyPath));
        if (query.IncludableQueryable != null)
            return Factory(query.IncludableQueryable.Include(navigationPropertyPath));
        if (query.IncludableManyQueryable != null)
            return Factory(query.IncludableManyQueryable.Include(navigationPropertyPath));
        throw new InvalidOperationException();
    }
    
    public static IUIncludable<T, TProp> ThenInclude<T, TPrevProp, TProp>(
        this IUIncludable<T, TPrevProp> source, 
        Expression<Func<TPrevProp, TProp>> navigationPropertyPath)
        where T : class
    {
        var query = DownCast(source);
        if (query.IncludableQueryable != null) 
            return Factory(query.IncludableQueryable.ThenInclude(navigationPropertyPath));
        if (query.IncludableManyQueryable != null)
            return Factory(query.IncludableManyQueryable.ThenInclude(navigationPropertyPath));
        if (query.Queryable != null)
            return Factory(query.Queryable.Include(ConvertExpressionParamType<T, TPrevProp, TProp>(navigationPropertyPath)));
        throw new InvalidOperationException();
    }
    
    public static IUIncludable<T, TProp> IncludeFrom<T,TPrevProp, TProp>(
        this IUIncludable<T> source, 
        Include<T, TPrevProp> expression,
        Expression<Func<TPrevProp, TProp>> navigationPropertyPath)
        where T : class
    {
        var query = DownCast(expression(source));
        if (query.IncludableQueryable != null) 
            return Factory(query.IncludableQueryable.ThenInclude(navigationPropertyPath));
        if (query.IncludableManyQueryable != null)
            return Factory(query.IncludableManyQueryable.ThenInclude(navigationPropertyPath));
        if (query.Queryable != null)
            return Factory(query.Queryable.Include(ConvertExpressionParamType<T, TPrevProp, TProp>(navigationPropertyPath)));
        throw new InvalidOperationException();
    }

    public static IUIncludable<TEntity, TEntity> ToBase<TEntity, TPreviousProperty>(
        this IUIncludable<TEntity, TPreviousProperty> source)
        where TEntity : class => new IUInclude<TEntity, TEntity>(source.AsQueryable());
    
}