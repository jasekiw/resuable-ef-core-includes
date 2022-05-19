namespace ReusableEfCoreIncludes;

public interface IIncludable<out T, out P> : IIncludable<T>
{
}

public interface IIncludable<out T>
{
    public IQueryable<T> AsQueryable();
}