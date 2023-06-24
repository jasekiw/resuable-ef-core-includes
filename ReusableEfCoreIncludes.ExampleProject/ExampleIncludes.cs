using ReusableEfCoreIncludes.ExampleProject.Models;

namespace ReusableEfCoreIncludes.ExampleProject;

// example of specifying options for your includes
public struct CompanyIncludeOptions
{
    public bool ExcludeDepartments { get; set; }
}

public static class ExampleIncludes
{
    public static IIncludable<Company> IncludeCompany(this IIncludable<Company> source, CompanyIncludeOptions options = default) => 
        source.IncludeCompany(Including.FromBase, options);
    
    public static IIncludable<T> IncludeCompany<T, TP>(this IIncludable<T> source, Include<T, TP> include, CompanyIncludeOptions options = default) 
        where T : class where TP : Company =>
        source.IncludeIf(!options.ExcludeDepartments, s => s.IncludeDepartment(q => q.IncludeManyFrom(include, r => r.Departments)));

    public static IIncludable<Department> IncludeDepartment(this IIncludable<Department> source) => source.IncludeDepartment(Including.FromBase);
    public static IIncludable<T> IncludeDepartment<T>(this IIncludable<T> source, Include<T, Department> include)
        where T : class =>
        source
            .IncludeUser(q => q.IncludeManyFrom(include, r => r.Users))
            .IncludeUser(q => q.IncludeFrom(include, e => e.LeadUser!))
            .IncludeManyFrom(include, r => r.Users).ThenInclude(r => r.Role);

    public static IIncludable<T> IncludeDepartmentTestDownCast<T>(this IIncludable<T> source,
        Include<T, Department> include)
        where T : class =>
        source
            .IncludeManyFrom(include, q => q.Users)
            .IncludeFrom(include, q => q.LeadUser);

    public static IIncludable<T> IncludeUser<T>(this IIncludable<T> source, Include<T, User> include)
        where T : class => 
        source.IncludeFrom(include, e => e.Role);
    public static IIncludable<User> IncludeUser(this IIncludable<User> source) => source.IncludeUser(Including.FromBase);
}