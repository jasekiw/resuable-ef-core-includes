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
            // including a generic needs a helper if the types are specified directly
            .BeginInclude().IncludeCompany().AsQueryable()
            .AsNoTracking()
            .FirstOrDefaultAsync();
        AssertCompany(company);
        
        // can execute it again without issues
        company = await _context!.Companies
            // including a generic needs a helper if the types are specified directly
            .BeginInclude().IncludeCompany().AsQueryable()
            .AsNoTracking()
            .FirstOrDefaultAsync();
        AssertCompany(company);
        Assert.IsNotEmpty(company!.Departments);
    }
    
    [Test]
    public async Task TestIncludeCompanyExclusion()
    {
        var company = await _context!.Companies
            .BeginInclude()
            .IncludeCompany(new CompanyIncludeOptions {ExcludeDepartments = true})
            .AsQueryable()
            .AsNoTracking()
            .FirstOrDefaultAsync();
        Assert.NotNull(company);
        Assert.IsEmpty(company!.Departments);
    }
    
    [Test]
    public async Task TestIncludeDepartments()
    {
        var department = await _context!.Departments
            .BeginInclude()
            .IncludeDepartment()
            .AsQueryable()
            .AsNoTracking()
            .FirstOrDefaultAsync();
        AssertDepartment(department);
        Assert.IsNotEmpty(department!.Users);
    }


    [Test]
    public async Task TestIncludeUser()
    {
        var user = await _context!.Users
            .BeginInclude()
            .IncludeUser()
            .AsQueryable()
            .AsNoTracking()
            .FirstOrDefaultAsync();
        AssertUser(user);
    }
    
       
    [Test]
    public async Task TestIncludeBase()
    {
        var department = await _context!.Departments
            .AsQueryable()
            .BeginInclude()
            .IncludeMany(d => d.Users)
            .AsQueryable()
            .AsNoTracking()
            .FirstOrDefaultAsync();
        Assert.NotNull(department);
        Assert.IsNotEmpty(department!.Users);

        var company = await _context.Companies
            .BeginInclude()
            .IncludeMany(c => c.Departments.Where(d => d.Name.Contains("f")))
            .ThenIncludeMany(d => d.Users)
            .AsQueryable()
            .AsNoTracking()
            .FirstOrDefaultAsync();
        Assert.NotNull(company);
        Assert.IsEmpty(company!.Departments);

        var user = await _context.Users
            .BeginInclude()
            .Include(u => u.Department)
            .ThenInclude(u => u!.Company)
            .AsQueryable()
            .AsNoTracking()
            .FirstOrDefaultAsync();
        Assert.NotNull(user);
        Assert.NotNull(user!.Department);
        Assert.NotNull(user.Department!.Company);
        
        department = await _context!.Departments
            .AsQueryable()
            .BeginInclude()
            .IncludeLeadUser(Including.FromBase)
            .AsQueryable()
            .AsNoTracking()
            .FirstOrDefaultAsync();
        Assert.NotNull(department);
        Assert.NotNull(department!.LeadUser);
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