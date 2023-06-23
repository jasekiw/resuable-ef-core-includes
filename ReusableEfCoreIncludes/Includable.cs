using Microsoft.EntityFrameworkCore.Query;

namespace ReusableEfCoreIncludes;

internal class Includable<T, TP> : IIncludableInternals<T,TP>
{
    public IQueryable<T>? Queryable { get; set; }
    public IIncludableQueryable<T, TP>?  IncludableQueryable { get; set; }
    public IIncludableQueryable<T, IEnumerable<TP>>? IncludableManyQueryable { get; }
    public IQueryable<T> AsQueryable() => (Queryable ?? IncludableQueryable ?? (IQueryable<T>) IncludableManyQueryable) 
                                             ?? throw new InvalidOperationException();

    public Includable(IIncludableQueryable<T, IEnumerable<TP>> includableManyQueryable) => 
        IncludableManyQueryable = includableManyQueryable;
    public Includable(IIncludableQueryable<T, TP> includableQueryable) => IncludableQueryable = includableQueryable;
    public Includable(IQueryable<T> queryable) => Queryable = queryable;
}