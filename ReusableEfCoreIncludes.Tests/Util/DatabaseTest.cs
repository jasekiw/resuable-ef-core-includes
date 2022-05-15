using ReusableEfCoreIncludes.ExampleProject;

namespace ReusableEfCoreIncludes.Tests.Util;

using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using NUnit.Framework;


public abstract class DatabaseTest
{
    public const string ConnectionString = "Server=127.0.0.1;Database=TestingExampleDatabase;User=root;Password=;";
    protected ExampleContext? _context;
    private IDbContextTransaction? _transaction;
    protected bool _withTransaction = true;
    
    [SetUp]
    public virtual Task SetupAsync()
    {
        Setup();
        return Task.CompletedTask;
    }
    
    public virtual void Setup()
    {
       _context = ExampleContextFactory.MakeContext(ConnectionString);
       _context.Database.Migrate();
        if(_withTransaction)
            _transaction = _context.Database.BeginTransaction();
    }
    
    [TearDown]
    public virtual Task TearDownAsync()
    {
        TearDown();
        return Task.CompletedTask;
    }
    
    public virtual void TearDown()
    {
        if(_withTransaction)
            _transaction?.Rollback();
        _context?.ChangeTracker.Clear();
    }
}