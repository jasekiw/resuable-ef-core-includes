namespace ReusableEfCoreIncludes;

using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using static ExpressionUtil;

public delegate IIncludable<T, TProperty> Include<T, out TProperty>(IIncludable<T> arg) where T : class;
public delegate IIncludable<T> AnonymousInclude<T>(IIncludable<T> arg) where T : class;

public static class IncludableExtensions
{
    private static IIncludable<T, TP> Factory<T, TP>(IIncludableQueryable<T, TP> include) => new Includable<T, TP>(include);
    private static IIncludable<T, TP> Factory<T, TP>(IIncludableQueryable<T, IEnumerable<TP>> include) => new Includable<T, TP>(include);
    private static IIncludable<T, T> Factory<T>(IQueryable<T> source) => new Includable<T, T>(source);

    public static IIncludable<T, TP> IncludeFrom<T, TP>(this IIncludable<T> source, Include<T, TP> expression) where T : class => expression(source);

    private static IIncludableInternals<T, TP> DownCast<T, TP>(IIncludable<T, TP> q) => 
        q as IIncludableInternals<T, TP> ?? throw new ArgumentException("Should be IIncludableInternals<T, TP> instance");

    private static IIncludableInternals<T, T> DownCast<T>(IIncludable<T> q) =>
        q as IIncludableInternals<T, T> ?? new Includable<T, T>(q.AsQueryable());

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
        expression ??= Including.FromBase<T, TPrevProp>;
        var query = DownCast(expression(source));
        if (query.IncludableQueryable != null)
            return Factory(query.IncludableQueryable.ThenInclude(navigationPropertyPath));
        if (query.IncludableManyQueryable != null)
            return Factory(query.IncludableManyQueryable.ThenInclude(navigationPropertyPath));
        if (query.Queryable != null)
            return Factory(query.Queryable.Include(ConvertExpressionParamType<T, TPrevProp, IEnumerable<TProp>>(navigationPropertyPath)));
        throw new InvalidOperationException();
    }
    
    public static IIncludable<T, TP> Include<T, TP>(this IIncludable<T> source, Expression<Func<T, TP>> navigationPropertyPath) where T : class
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
    
    public static IIncludable<T> IncludeIf<T>(
        this IIncludable<T> source, 
        bool condition,
        AnonymousInclude<T> expression)
        where T : class
    {
        return condition ? expression(source) : source;
    }
    
    
    public static IIncludable<T, TProp> IncludeFrom<T,TPrevProp, TProp>(
        this IIncludable<T> source, 
        Include<T, TPrevProp>? expression,
        Expression<Func<TPrevProp, TProp>> navigationPropertyPath)
        where T : class
    {
        expression ??= Including.FromBase<T, TPrevProp>;
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