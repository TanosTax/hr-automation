using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Automation.Models;

public class WorkSchedule
{
    [Key]
    public int Id { get; set; }
    
    public int EmployeeId { get; set; }
    [ForeignKey(nameof(EmployeeId))]
    public Employee Employee { get; set; } = null!;
    
    [Required]
    public DateTime Date { get; set; }
    
    public TimeSpan? CheckIn { get; set; }
    
    public TimeSpan? CheckOut { get; set; }
    
    public bool IsAbsent { get; set; } = false;
    
    [MaxLength(200)]
    public string? AbsenceReason { get; set; }
    
    [MaxLength(500)]
    public string? Notes { get; set; }
    
    [NotMapped]
    public TimeSpan? WorkedHours => CheckIn.HasValue && CheckOut.HasValue 
        ? CheckOut.Value - CheckIn.Value 
        : null;
}
