using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using ReusableEfCoreIncludes.ExampleProject;
using ReusableEfCoreIncludes.Tests.Util;
using static ReusableEfCoreIncludes.Tests.Util.Assertions;

namespace ReusableEfCoreIncludes.Tests;

public class MainTest : DatabaseTest
{
    [SetUp]
    public override async Task SetupAsync()
    {
        await base.SetupAsync();
        await Fixtures.Make(_context!);
    }

    [Test]
    public async Task TestIncludeCompany()
    {
        var company = await _context!.Companies
            .BeginInclude()
            .IncludeCompany(Include.FromBase)
            .AsQueryable()
            .FirstOrDefaultAsync();
        AssertCompany(company);
    }
    
    [Test]
    public async Task TestIncludeDepartments()
    {
        var department = await _context!.Departments
            .BeginInclude()
            .IncludeDepartment(Include.FromBase)
            .AsQueryable()
            .FirstOrDefaultAsync();
        AssertDepartment(department);
    }
    
    [Test]
    public async Task TestIncludeBase()
    {
        var department = await _context!.Departments
            .AsQueryable()
            .BeginInclude()
            .IncludeMany(d => d.Users)
            .AsQueryable()
            .FirstOrDefaultAsync();
        Assert.NotNull(department);
        Assert.IsNotEmpty(department!.Users);

        var company = await _context.Companies
            .BeginInclude()
            .IncludeMany(c => c.Departments)
            .ThenIncludeMany(d => d.Users)
            .AsQueryable()
            .FirstOrDefaultAsync();
            Assert.NotNull(company);
            Assert.IsNotEmpty(department.Users);

            var user = await _context.Users.BeginInclude()
                .Include(u => u.Department)
                .ThenInclude(u => u.Company)
                .AsQueryable()
                .FirstOrDefaultAsync();
            Assert.NotNull(user);
            Assert.NotNull(user!.Department);
            Assert.NotNull(user.Department.Company);
    }

    
    [Test]
    public async Task TestIncludeUser()
    {
        var user = await _context!.Users
            .BeginInclude()
            .IncludeUser(Include.FromBase)
            .AsQueryable()
            .FirstOrDefaultAsync();
        AssertUser(user);
    }
    
    [Test]
    public async Task TestIncludeUserThenIncludeBase()
    {
        var user = await _context!.Users
            .BeginInclude()
            .Include(u => u.Department)
            .AsQueryable()
            .FirstOrDefaultAsync();
        Assert.NotNull(user);
    }
}