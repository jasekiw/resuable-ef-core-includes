using System.ComponentModel.DataAnnotations.Schema;

namespace ReusableEfCoreIncludes.ExampleProject.Models;

[Table(nameof(Company))]
public class Company
{
    public int Id { get; set; }
    public string Name { get; set; }
    
    [InverseProperty(nameof(Department.Company))]
    public List<Department> Departments { get; set; } = new();
}