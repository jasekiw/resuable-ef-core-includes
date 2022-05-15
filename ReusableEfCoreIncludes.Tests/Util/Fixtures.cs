using System.Collections.Generic;
using System.Threading.Tasks;
using ReusableEfCoreIncludes.ExampleProject;
using ReusableEfCoreIncludes.ExampleProject.Models;

namespace ReusableEfCoreIncludes.Tests.Util;

public static class Fixtures
{
    public static async Task Make(ExampleContext context)
    {
        var department = new Department
        {
            Name = "Test Department",
            Company = new Company("Test Company"),
            Users = new List<User> { new() { Email = "test@test.com", Role = new Role("Base User") } }
        };
        context.Add(department);
        await context.SaveChangesAsync();
        var manager = new User { Email = "manager@test.com", Role = new Role("Manager Role"), Department = department };
        context.Add(manager);
        await context.SaveChangesAsync();
        department.LeadUser = manager;
        await context.SaveChangesAsync();
        context.ChangeTracker.Clear();
    }
}