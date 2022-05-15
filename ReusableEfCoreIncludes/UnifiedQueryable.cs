using Microsoft.EntityFrameworkCore.Query;

namespace ReusableEfCoreIncludes;

public class IUInclude<T, P> : IUIncludable<T, P>, IUIncludeInternals<T,P>
{
    public IQueryable<T>? Queryable { get; set; }
    public IIncludableQueryable<T, P>?  IncludableQueryable { get; set; }
    public IIncludableQueryable<T, IEnumerable<P>>? IncludableManyQueryable { get; }
    public IQueryable<T> AsQueryable() => (Queryable ?? IncludableQueryable ?? (IQueryable<T>) IncludableManyQueryable) 
                                             ?? throw new InvalidOperationException();

    public IUInclude(IIncludableQueryable<T, IEnumerable<P>> includableManyQueryable) => 
        IncludableManyQueryable = includableManyQueryable;
    public IUInclude(IIncludableQueryable<T, P> includableQueryable) => IncludableQueryable = includableQueryable;
    public IUInclude(IQueryable<T> queryable) => Queryable = queryable;
}