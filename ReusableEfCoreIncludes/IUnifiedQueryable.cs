namespace ReusableEfCoreIncludes;

public interface IUnifiedQueryable<out T, out P>
{
    public IQueryable<T> EndInclude();
}