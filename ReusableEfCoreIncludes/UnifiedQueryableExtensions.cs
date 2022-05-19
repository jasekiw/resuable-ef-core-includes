namespace ReusableEfCoreIncludes;

using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using static ExpressionUtil;

public delegate IIncludable<T, TProperty> Include<T, out TProperty>(IIncludable<T> arg) where T : class;

public static class UnifiedQueryableExtensions
{
    private static IIncludable<T, P> Factory<T, P>(IIncludableQueryable<T, P> include) => new Includable<T, P>(include);
    private static IIncludable<T, P> Factory<T, P>(IIncludableQueryable<T, IEnumerable<P>> include) => new Includable<T, P>(include);
    private static IIncludable<T, T> Factory<T>(IQueryable<T> source) => new Includable<T, T>(source);

    public static IIncludable<T, P> IncludeExpression<T, P>(this IIncludable<T> source, Include<T, P>? expression)
        where T : class
    {
        expression ??= Includes.FromBase<T, P>;
        return expression(source);
    }
    
    private static IIncludableInternals<T, P> DownCast<T, P>(IIncludable<T, P> q) => 
        q as IIncludableInternals<T, P> ?? throw new ArgumentException("Should be UnifiedQueryable instance");

    private static IIncludableInternals<T, T> DownCast<T>(IIncludable<T> q) => 
        q as IIncludableInternals<T, T> ?? throw new ArgumentException("Should be UnifiedQueryable instance");
    
    
    public static IIncludable<TEntity> BeginInclude<TEntity>(this IQueryable<TEntity> source) 
        where TEntity : class => Factory(source);

    
    public static IIncludable<T, TProp> ThenIncludeMany<T, TPrevProp, TProp>(
        this IIncludable<T, TPrevProp> source,
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
    
    public static IIncludable<T, TProp> IncludeMany<T, TProp>(
        this IIncludable<T> source,
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
    
    public static IIncludable<T, TProp> IncludeManyFrom<T, TPrevProp, TProp>(
        this IIncludable<T> source,
        Include<T, TPrevProp>? expression,
        Expression<Func<TPrevProp, IEnumerable<TProp>>> navigationPropertyPath
        )
        where T : class
    {
        expression ??= Includes.FromBase<T, TPrevProp>;
        var query = DownCast(expression(source));
        if (query.IncludableQueryable != null)
            return Factory(query.IncludableQueryable.ThenInclude(navigationPropertyPath));
        if (query.IncludableManyQueryable != null)
            return Factory(query.IncludableManyQueryable.ThenInclude(navigationPropertyPath));
        if (query.Queryable != null)
            return Factory(query.Queryable.Include(ConvertExpressionParamType<T, TPrevProp, IEnumerable<TProp>>(navigationPropertyPath)));
        throw new InvalidOperationException();
    }
    
    public static IIncludable<T, P> Include<T, P>(this IIncludable<T> source, Expression<Func<T, P>> navigationPropertyPath) where T : class
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
    
    public static IIncludable<T, TProp> ThenInclude<T, TPrevProp, TProp>(
        this IIncludable<T, TPrevProp> source, 
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
    
    public static IIncludable<T, TProp> IncludeFrom<T,TPrevProp, TProp>(
        this IIncludable<T> source, 
        Include<T, TPrevProp>? expression,
        Expression<Func<TPrevProp, TProp>> navigationPropertyPath)
        where T : class
    {
        expression ??= Includes.FromBase<T, TPrevProp>;
        var query = DownCast(expression(source));
        if (query.IncludableQueryable != null) 
            return Factory(query.IncludableQueryable.ThenInclude(navigationPropertyPath));
        if (query.IncludableManyQueryable != null)
            return Factory(query.IncludableManyQueryable.ThenInclude(navigationPropertyPath));
        if (query.Queryable != null)
            return Factory(query.Queryable.Include(ConvertExpressionParamType<T, TPrevProp, TProp>(navigationPropertyPath)));
        throw new InvalidOperationException();
    }

}