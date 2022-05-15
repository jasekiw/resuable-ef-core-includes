using NUnit.Framework;
using ReusableEfCoreIncludes.ExampleProject.Models;

namespace ReusableEfCoreIncludes.Tests.Util;

public static class Assertions
{
    public static void AssertCompany(Company? c)
    {
        Assert.NotNull(c);
        c!.Departments.ForEach(AssertDepartment);
    }
    public static void AssertDepartment(Department? d)
    {
        Assert.NotNull(d);
        d!.Users.ForEach(AssertUser);
        AssertUser(d.LeadUser!);
    }
    public static void AssertUser(User? u)
    {
        Assert.NotNull(u);
        AssertRole(u!.Role);
    }

    public static void AssertRole(Role? r) => Assert.NotNull(r);
}