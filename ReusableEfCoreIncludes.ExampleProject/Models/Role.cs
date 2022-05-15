using System.ComponentModel.DataAnnotations.Schema;

namespace ReusableEfCoreIncludes.ExampleProject.Models;

[Table(nameof(Role))]
public class Role
{
    public int Id { get; set; }
    public string Name { get; set; } = "";

    public Role(string name) => Name = name;

    public Role() { }
}