namespace ReusableEfCoreIncludes;

public static class Includes
{
    public static IIncludable<T, T> FromBase<T>(IIncludable<T> q) => new Includable<T, T>(q.AsQueryable());
    public static IIncludable<T, P> FromBase<T, P>(IIncludable<T> q) => new Includable<T, P>(q.AsQueryable());
}