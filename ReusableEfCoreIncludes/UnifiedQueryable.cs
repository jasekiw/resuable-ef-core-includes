using Microsoft.EntityFrameworkCore.Query;

namespace ReusableEfCoreIncludes;

public class UInclude<T, P> : IUIncludable<T, P>, IUIncludeInternals<T,P>
{
    public IQueryable<T>? Queryable { get; set; }
    public IIncludableQueryable<T, P>?  IncludableQueryable { get; set; }
    public IIncludableQueryable<T, IEnumerable<P>>? IncludableManyQueryable { get; }
    public IQueryable<T> AsQueryable() => (Queryable ?? IncludableQueryable ?? (IQueryable<T>) IncludableManyQueryable) 
                                             ?? throw new InvalidOperationException();

    public UInclude(IIncludableQueryable<T, IEnumerable<P>> includableManyQueryable) => 
        IncludableManyQueryable = includableManyQueryable;
    public UInclude(IIncludableQueryable<T, P> includableQueryable) => IncludableQueryable = includableQueryable;
    public UInclude(IQueryable<T> queryable) => Queryable = queryable;
}