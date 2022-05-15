using Microsoft.EntityFrameworkCore.Query;

namespace ReusableEfCoreIncludes;

internal interface IUIncludeInternals<T, P> : IUIncludable<T, P>
{
    public IQueryable<T>? Queryable { get; set; }
    public IIncludableQueryable<T, P>?  IncludableQueryable { get; set; }
    public IIncludableQueryable<T, IEnumerable<P>>? IncludableManyQueryable { get; }
}