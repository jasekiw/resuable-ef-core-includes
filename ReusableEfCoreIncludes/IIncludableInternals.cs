using Microsoft.EntityFrameworkCore.Query;

namespace ReusableEfCoreIncludes;

internal interface IIncludableInternals<T, TP> : IIncludable<T, TP>
{
    public IQueryable<T>? Queryable { get; set; }
    public IIncludableQueryable<T, TP>?  IncludableQueryable { get; set; }
    public IIncludableQueryable<T, IEnumerable<TP>>? IncludableManyQueryable { get; }
}