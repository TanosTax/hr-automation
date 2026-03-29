using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Automation.Models;

public class Department
{
    [Key]
    public int Id { get; set; }
    
    [Required, MaxLength(100)]
    public string Name { get; set; } = string.Empty;
    
    [MaxLength(500)]
    public string? Description { get; set; }
    
    public int? ManagerId { get; set; }
    
    // Навигационные свойства
    public ICollection<Employee> Employees { get; set; } = new List<Employee>();
}
