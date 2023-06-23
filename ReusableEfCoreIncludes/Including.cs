namespace ReusableEfCoreIncludes;

public static class Including
{
    public static IIncludable<T, T> FromBase<T>(IIncludable<T> q) => new Includable<T, T>(q.AsQueryable());
    public static IIncludable<T, TP> FromBase<T, TP>(IIncludable<T> q) => new Includable<T, TP>(q.AsQueryable());
}