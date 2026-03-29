using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Automation.Models;

public class Position
{
    [Key]
    public int Id { get; set; }
    
    [Required, MaxLength(100)]
    public string Name { get; set; } = string.Empty;
    
    [Column(TypeName = "decimal(18,2)")]
    public decimal BaseSalary { get; set; }
    
    [MaxLength(500)]
    public string? Requirements { get; set; }
    
    // Навигационные свойства
    public ICollection<Employee> Employees { get; set; } = new List<Employee>();
}
