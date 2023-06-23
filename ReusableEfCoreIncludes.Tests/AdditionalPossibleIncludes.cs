using ReusableEfCoreIncludes.ExampleProject.Models;

namespace ReusableEfCoreIncludes.Tests;

public static class AdditionalPossibleIncludes
{
    public static IIncludable<T> IncludeLeadUser<T>(this IIncludable<T> source, Include<T, Department> include)
        where T : class =>
        source
            .IncludeFrom(include).ThenInclude(d => d.LeadUser);
}