using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using ReusableEfCoreIncludes.ExampleProject.Models;

namespace ReusableEfCoreIncludes.Tests;

public class ReusableIncludesTest : DatabaseTest
{
    public override async Task SetupAsync()
    {
        await base.SetupAsync();
        var department = new Department
        {
            Name = "Test Department",
            Company = new Company
            {
                Name = "Test Company"
            },
            Users = new List<User>
            {
                new()
                {
                    Email = "test@test.com",
                    Role = new Role
                    {
                        Name = "Base User"
                    }
                }
            }
        };
        _context!.Add(department);
        await _context.SaveChangesAsync();
        var manager = new User
        {
            Email = "manager@test.com",
            Role = new Role
            {
                Name = "Manager Role"
            },
            Department = department
        };
        _context.Add(manager);
        await _context.SaveChangesAsync();
        department.LeadUser = manager;
        await _context.SaveChangesAsync();
        _context.ChangeTracker.Clear();
        
    }

    [Test]
    public async Task TestIncludeCompany()
    {
        var company = await _context!.Companies
            .StartInclude()
            .IncludeCompany(q => q)
            .EndInclude()
            .FirstOrDefaultAsync();
        AssertCompany(company);
    }
    
    [Test]
    public async Task TestIncludeDepartments()
    {
        var department = await _context!.Departments
            .StartInclude()
            .IncludeDepartment(q => q)
            .EndInclude()
            .FirstOrDefaultAsync();
        AssertDepartment(department);
    }

    
    [Test]
    public async Task TestIncludeUser()
    {
        var user = await _context!.Users
            .StartInclude()
            .IncludeUser(q => q)
            .EndInclude()
            .FirstOrDefaultAsync();
        AssertUser(user);
    }

    private void AssertCompany(Company? c)
    {
        Assert.NotNull(c);
        c!.Departments.ForEach(AssertDepartment);
    }
    private void AssertDepartment(Department? d)
    {
        Assert.NotNull(d);
        d!.Users.ForEach(AssertUser);
        AssertUser(d.LeadUser!);
    }
    private void AssertUser(User? u)
    {
        Assert.NotNull(u);
        AssertRole(u!.Role);
    }

    private void AssertRole(Role? r)
    {
        Assert.NotNull(r);
        // any additional asserts go here
    }
}


public static class TestQueryExtensions
{
    public static IUnifiedQueryable<TEntity, TEntity> IncludeCompany<TEntity, TProperty>(
        this IUnifiedQueryable<TEntity, TEntity> source,
        Func<IUnifiedQueryable<TEntity, TEntity>, IUnifiedQueryable<TEntity, TProperty>> include
    ) where TEntity : class where TProperty : Company => 
        source
            .IncludeDepartment(q => q.IncludeExpression(include).ThenIncludeMany(r => r.Departments))
            .ToBase();
    
    public static IUnifiedQueryable<TEntity, TEntity> IncludeDepartment<TEntity, TProperty>(
        this IUnifiedQueryable<TEntity, TEntity> source,
        Func<IUnifiedQueryable<TEntity, TEntity>, IUnifiedQueryable<TEntity, TProperty>> include
    ) where TEntity : class where TProperty : Department => 
        source
            .IncludeUser(q => q.IncludeExpression(include).ThenIncludeMany(r => r.Users))
            .IncludeExpression(include)
            .ThenInclude(e => e.LeadUser)
            .ToBase();
    
    
    public static IUnifiedQueryable<TEntity, TEntity> IncludeUser<TEntity, TProperty>(
        this IUnifiedQueryable<TEntity, TEntity> source,
        Func<IUnifiedQueryable<TEntity, TEntity>, IUnifiedQueryable<TEntity, TProperty>> include
    ) where TEntity : class where TProperty : User =>
            source
            .IncludeExpression(include)
            .ThenInclude(e => e.Role)
            .ToBase();
    
}