using Microsoft.EntityFrameworkCore.Query;

namespace ReusableEfCoreIncludes;

public class Includable<T, P> : IIncludable<T, P>, IIncludableInternals<T,P>
{
    public IQueryable<T>? Queryable { get; set; }
    public IIncludableQueryable<T, P>?  IncludableQueryable { get; set; }
    public IIncludableQueryable<T, IEnumerable<P>>? IncludableManyQueryable { get; }
    public IQueryable<T> AsQueryable() => (Queryable ?? IncludableQueryable ?? (IQueryable<T>) IncludableManyQueryable) 
                                             ?? throw new InvalidOperationException();

    public Includable(IIncludableQueryable<T, IEnumerable<P>> includableManyQueryable) => 
        IncludableManyQueryable = includableManyQueryable;
    public Includable(IIncludableQueryable<T, P> includableQueryable) => IncludableQueryable = includableQueryable;
    public Includable(IQueryable<T> queryable) => Queryable = queryable;
}