using ReusableEfCoreIncludes.ExampleProject.Models;

namespace ReusableEfCoreIncludes.ExampleProject;

public static class ExampleIncludes
{
    public static IUIncludable<T> IncludeCompany<T>(this IUIncludable<T> source, Include<T, Company> include) where T : class =>
        source.IncludeDepartment(q => q.IncludeManyFrom(include, r => r.Departments));
    
    public static IUIncludable<T> IncludeDepartment<T, P>(this IUIncludable<T> source, Include<T, P> include)
        where T : class where P : Department =>
        source
            .IncludeUser(q => q.IncludeManyFrom(include, r => r.Users))
            .IncludeFrom(include, e => e.LeadUser);


    public static IUIncludable<T> IncludeUser<T, P>(this IUIncludable<T> source, Include<T, P> include)
        where T : class where P : User =>
        source.IncludeFrom(include, e => e.Role);
}