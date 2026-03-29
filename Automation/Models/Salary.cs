using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Automation.Models;

public class Salary
{
    [Key]
    public int Id { get; set; }
    
    public int EmployeeId { get; set; }
    [ForeignKey(nameof(EmployeeId))]
    public Employee Employee { get; set; } = null!;
    
    [Required]
    public int Year { get; set; }
    
    [Required]
    public int Month { get; set; }
    
    [Column(TypeName = "decimal(18,2)")]
    public decimal BaseSalary { get; set; }
    
    [Column(TypeName = "decimal(18,2)")]
    public decimal Bonus { get; set; } = 0;
    
    [Column(TypeName = "decimal(18,2)")]
    public decimal Deductions { get; set; } = 0;
    
    [Column(TypeName = "decimal(18,2)")]
    public decimal Total { get; set; }
    
    public DateTime PaymentDate { get; set; }
    
    public bool IsPaid { get; set; } = false;
    
    [MaxLength(500)]
    public string? Notes { get; set; }
}
