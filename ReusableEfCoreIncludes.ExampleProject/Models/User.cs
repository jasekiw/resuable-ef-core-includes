using System.ComponentModel.DataAnnotations.Schema;

namespace ReusableEfCoreIncludes.ExampleProject.Models;

[Table(nameof(User))]
public class User
{
    public int Id { get; set; }
    public string Email { get; set; } = "";
    public int RoleId { get; set; }
    [ForeignKey(nameof(RoleId))]
    public Role? Role { get; set; }
    
    public int DepartmentId { get; set; }
    [ForeignKey(nameof(DepartmentId))]
    public Department? Department {get; set; }
    
}