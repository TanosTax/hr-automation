using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Automation.Models;

public enum ContractType
{
    Permanent,     // Бессрочный
    Fixed,         // Срочный
    PartTime       // Совместительство
}

public class Contract
{
    [Key]
    public int Id { get; set; }
    
    public int EmployeeId { get; set; }
    [ForeignKey(nameof(EmployeeId))]
    public Employee Employee { get; set; } = null!;
    
    [Required, MaxLength(50)]
    public string ContractNumber { get; set; } = string.Empty;
    
    [Required]
    public DateTime SignDate { get; set; }
    
    public DateTime? EndDate { get; set; }
    
    [Required]
    public ContractType Type { get; set; }
    
    [Column(TypeName = "decimal(18,2)")]
    public decimal Salary { get; set; }
    
    [MaxLength(1000)]
    public string? Notes { get; set; }
    
    public bool IsActive { get; set; } = true;
}
