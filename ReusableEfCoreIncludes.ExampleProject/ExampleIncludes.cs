using ReusableEfCoreIncludes.ExampleProject.Models;

namespace ReusableEfCoreIncludes.ExampleProject;

public static class ExampleIncludes
{
    public static IUIncludable<T> IncludeCompany<T>(this IUIncludable<T> source, Include<T, Company> include) where T : class =>
        source.IncludeDepartment(q => q.ThenIncludeMany(include, r => r.Departments));
    
    public static IUIncludable<T> IncludeDepartment<T, P>(this IUIncludable<T> source, Include<T, P> include)
        where T : class where P : Department =>
        source
            .IncludeUser(q => q.ThenIncludeMany(include, r => r.Users))
            .ThenIncludeFrom(include, e => e.LeadUser);
    
    
    public static IUIncludable<T, T> IncludeUser<T, P>(this IUIncludable<T> source, Include<T, P> include) 
        where T : class where P : User =>
        source
            .ThenIncludeFrom(include, e => e.Role)
            .ToBase();
}