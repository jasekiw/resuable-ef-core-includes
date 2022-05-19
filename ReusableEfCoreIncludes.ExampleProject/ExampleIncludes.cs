using ReusableEfCoreIncludes.ExampleProject.Models;

namespace ReusableEfCoreIncludes.ExampleProject;

public static class ExampleIncludes
{
    public static IIncludable<Company> IncludeCompany(this IIncludable<Company> source) => source.IncludeCompany<Company, Company>(null);
    
    public static IIncludable<T> IncludeCompany<T, P>(this IIncludable<T> source, Include<T, P>? include) where T : class where P : Company =>
        source.IncludeDepartment(q => q.IncludeManyFrom(include, r => r.Departments));
    
    public static IIncludable<T> IncludeDepartment<T>(this IIncludable<T> source, Include<T, Department>? include = null)
        where T : class =>
        source
            .IncludeUser(q => q.IncludeManyFrom(include, r => r.Users))
            .IncludeUser(q => q.IncludeFrom(include, e => e.LeadUser!));


    public static IIncludable<T> IncludeUser<T, P>(this IIncludable<T> source, Include<T, P>? include)
        where T : class where P : User => 
        source.IncludeFrom(include, e => e.Role);
}