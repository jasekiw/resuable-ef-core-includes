using Microsoft.EntityFrameworkCore;
using ReusableEfCoreIncludes.ExampleProject.Models;

namespace ReusableEfCoreIncludes.ExampleProject;

public class ExampleContext  : DbContext
{
    public ExampleContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<Company> Companies { get; set; }
    public DbSet<Department> Departments { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<Role> Roles { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        var connection = "Server=localhost;Database=ExampleDatabase;User=root;Password=;";
        optionsBuilder.UseMySql(connection, ServerVersion.AutoDetect(connection));
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
      
    }
}