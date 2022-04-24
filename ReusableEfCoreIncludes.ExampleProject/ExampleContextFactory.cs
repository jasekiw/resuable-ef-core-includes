using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace ReusableEfCoreIncludes.ExampleProject;

public class ExampleContextFactory : IDesignTimeDbContextFactory<ExampleContext>
{
    private const string DefaultConnectionString = "Server=localhost;Database=ExampleDatabase;User=root;Password=;";
    public ExampleContext CreateDbContext(string[] args) => MakeContext(DefaultConnectionString);

    public static ExampleContext MakeContext(string connectionString)
    {
        var connection = "Server=localhost;Database=ExampleDatabase;User=root;Password=;";
        var optionsBuilder = new DbContextOptionsBuilder<ExampleContext>();
        optionsBuilder.UseMySql(connection, ServerVersion.AutoDetect(connection));
        return new ExampleContext(optionsBuilder.Options);
    }
}