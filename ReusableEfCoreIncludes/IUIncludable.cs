namespace ReusableEfCoreIncludes;

public interface IUIncludable<out T, out P> : IUIncludable<T>
{
}

public interface IUIncludable<out T>
{
    public IQueryable<T> AsQueryable();
}