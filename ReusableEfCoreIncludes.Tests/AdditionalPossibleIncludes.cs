using ReusableEfCoreIncludes.ExampleProject.Models;

namespace ReusableEfCoreIncludes.Tests;

public static class AdditionalPossibleIncludes
{
    public static IIncludable<T> IncludeLeadUser<T>(this IIncludable<T> source, Include<T, Department>? include = null)
        where T : class =>
        source
            .IncludeExpression(include).ThenInclude(d => d.LeadUser);
}