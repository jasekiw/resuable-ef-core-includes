using Microsoft.EntityFrameworkCore.Query;

namespace ReusableEfCoreIncludes;

public class UnifiedQueryable<T, P> : IUnifiedQueryable<T, P>
{
    public IQueryable<T>? Queryable { get; set; }
    public IIncludableQueryable<T, P>?  IncludableQueryable { get; set; }
    public IIncludableQueryable<T, IEnumerable<P>>? IncludableManyQueryable { get; }
    public IQueryable<T> EndInclude() => (Queryable ?? IncludableQueryable ?? (IQueryable<T>) IncludableManyQueryable) 
                                             ?? throw new InvalidOperationException();

    public UnifiedQueryable(IIncludableQueryable<T, IEnumerable<P>> includableManyQueryable) => 
        IncludableManyQueryable = includableManyQueryable;
    public UnifiedQueryable(IIncludableQueryable<T, P> includableQueryable) => IncludableQueryable = includableQueryable;
    public UnifiedQueryable(IQueryable<T> queryable) => Queryable = queryable;
}