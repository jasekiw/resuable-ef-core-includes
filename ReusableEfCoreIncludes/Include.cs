namespace ReusableEfCoreIncludes;

public class Include
{
    public static IUIncludable<T, T> FromBase<T>(IUIncludable<T> q) => (IUIncludable<T, T>)q;
}