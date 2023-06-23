using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ReusableEfCoreIncludes.ExampleProject.Models;

[Table(nameof(Department))]
public class Department
{
    public int Id { get; set; }
    public string Name { get; set; } = "";

    [InverseProperty(nameof(User.Department))]
    public List<User> Users { get; set; } = new();

    public int? LeadUserId { get; set; }

    [ForeignKey(nameof(LeadUserId))] 
    public User? LeadUser { get; set; }
    public int CompanyId { get; set; }
    [ForeignKey(nameof(CompanyId))] 
    public Company? Company { get; set; }
}