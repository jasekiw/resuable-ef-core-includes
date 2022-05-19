using Microsoft.EntityFrameworkCore.Query;

namespace ReusableEfCoreIncludes;

internal interface IIncludableInternals<T, P> : IIncludable<T, P>
{
    public IQueryable<T>? Queryable { get; set; }
    public IIncludableQueryable<T, P>?  IncludableQueryable { get; set; }
    public IIncludableQueryable<T, IEnumerable<P>>? IncludableManyQueryable { get; }
}